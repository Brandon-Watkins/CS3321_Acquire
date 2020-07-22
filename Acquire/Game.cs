using Acquire;
using Acquire.interfaces;
using System;
using System.Collections.Generic;

namespace Acquire
{
    /// <summary>
    /// Represents the game model to play Acquire.
    /// </summary>
    public class Game : GameModel
    {
        // Member variables
        private Player currentPlayer;
        private Player[] players;
        private List<Tile> drawPile;
        private Corporation[] corporations;
        private Board board;
        private List<Observer> subscribers;
        private static Random rnd = new Random();
        private Bank bank;
        private GameState state;
        private ChooseCorpEvent chooseCorpEvent;
        private ChooseCorpHandler chooseCorpHandler;
        private IMerger merger;
        private bool endConditionsMet;

        /// <summary>
        /// Property that gets the list of tiles representing the draw pile.
        /// </summary>
        public List<Tile> DrawPile { get => drawPile; set { } }


        /// <summary>
        /// Creates the Game: initializes players, board, draw pile, corporations, and subscriber list.
        /// </summary>
        /// <param name="playerNames">An array of strings consisting of each player's name.</param>
        public Game(string [] playerNames)
        {
            subscribers = new List<Observer>(); // initialize subscribers
            //Create all tiles and place in drawPile
            CreateDrawPile();
            
            // initialize corporations
            this.corporations = new Corporation[7];
            corporations[(int)ECorporation.American] = new Corporation(ECorporation.American, 100);
            corporations[(int)ECorporation.Continental] = new Corporation(ECorporation.Continental, 200);
            corporations[(int)ECorporation.Festival] = new Corporation(ECorporation.Festival, 100);
            corporations[(int)ECorporation.Imperial] = new Corporation(ECorporation.Imperial, 100);
            corporations[(int)ECorporation.Sackson] = new Corporation(ECorporation.Sackson);// newer versions of the game have changed this to ECorporation.Sackson
            corporations[(int)ECorporation.Tower] = new Corporation(ECorporation.Tower, 200);
            corporations[(int)ECorporation.WorldWide] = new Corporation(ECorporation.WorldWide);
            foreach (Corporation c in corporations)
                c.addHandler(this);

            state = GameState.WaitPlayTile;

            bank = new Bank(corporations, 25);
            
            // Create player array and give tiles
            players = new Player[playerNames.Length];
            for (int i = 0; i < playerNames.Length; i++)
            {
                players[i] = new Player(playerNames[i], corporations);
                currentPlayer = players[i]; // needed for drawTile call
                drawTile();
            }
            currentPlayer = players[0];
            // initialize board
            board = new Board();
            endConditionsMet = false;
        }
        
        /// <summary>
        /// Creates all tiles in the draw pile
        /// </summary>
        private void CreateDrawPile()
        {
            drawPile = new List<Tile>();
            char row = 'A';
            for (int x = 0; x < 9; x++)
            {

                for (int y = 1; y < 13; y++)
                {
                    string tileString = y.ToString() + row;
                    Tile tileToAdd = new Tile(tileString);
                    drawPile.Add(tileToAdd);
                }
                int rowConvert = row + 1;
                row = (char)rowConvert;
            }
        }

        /// <summary>
        /// Retrieves the tile placed on the board at position.
        /// </summary>
        /// <param name="position">string in the form "1A"</param>
        /// <returns>Tile if its on board, otherwise null</returns>
        public Tile getTileOnBoard(string position)
        {
            return board.getTileAt(position);
        }

        /// <summary>
        /// Gets current player.
        /// </summary>
        /// <returns>Player</returns>
        public Player getCurrentPlayer()
        {
            return this.currentPlayer;
        }

        /// <summary>
        /// Advances to the next player.
        /// </summary>
        /// <exception cref="Exception">Thrown if the current player is unknown.</exception>
        public void nextPlayer()
        {
            if (state != GameState.WaitEndTurn && state != GameState.WaitHandleStocks)
                throw new InvalidOperationException("Can't advance players till the current player finishes.");
            // Find current player index
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == currentPlayer)
                {
                    bool isLastPlayer = (i == players.Length - 1);
                    currentPlayer = isLastPlayer ? players[0] : players[i + 1];
                    break;
                }
            }
            if (state != GameState.WaitHandleStocks)
            {
                state = GameState.WaitPlayTile;
                this.notifySubscribers();
            }
        }

        /// <summary>
        /// Plays a tile in the current player's hand on the board.
        /// Draws tiles after to maintain 6.
        /// </summary>
        /// <exception cref="NullReferenceException">Thrown if the Tile at position isn't in player's hand.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the game state isn't waiting to play a tile.</exception>
        /// <exception cref="PermanentUnplayableTileException">Thrown if the tile would merge safe corporations (illegal move)</exception>
        /// <exception cref="TemporaryUnplayableTileException">Thrown if the tile would create an 8th corporation (temporarily illegal move)</exception>
        /// <param name="position">A string in the form "1A"</param>
        public void playTile(string position)
        {
            if (state != GameState.WaitPlayTile)
            {
                throw new InvalidOperationException("The player can't play a tile now.");
            }
            Tile playedTile = currentPlayer.playTile(position);
            TileIntersection intersection = board.playTile(playedTile);
            bool isResolved = false;
            if (intersection != null)
            {
                try // Attempt to resolve intersection
                {
                    intersection.setHandler(this);
                    this.chooseCorpEvent = intersection;
                    isResolved = intersection.resolveIntersection(corporations);
                }
                // Permanently unplayable exception
                catch (PermanentUnplayableTileException e)
                {
                    // Unable to play tile - remove
                    board.removeTileAt(position);
                    drawTile();
                    notifySubscribers();
                    throw e; // pass it along to the view
                }
                // Temporarily unplayable
                catch (TemporaryUnplayableTileException e)
                {
                    // Unable to play tile - revert tile to player hand
                    currentPlayer.giveTile(board.removeTileAt(position));
                    notifySubscribers();
                    throw e; // pass it along to the view?
                }
            }
            this.drawTile();
            // Update the state to end turn, unless The intersection is not resolved
            if (intersection == null || isResolved) 
            {
                state = GameState.WaitEndTurn;
                this.notifySubscribers();
            }
        }

        /// <summary>
        /// Will pick a random tile from the pile and give it to the player. Calls recursively if player's hand is less than 6
        /// </summary>
        public void drawTile()
        {
            if (CanDrawTile())
            {
                int randomDraw = rnd.Next(drawPile.Count);
                Tile theTile = drawPile[randomDraw];
                bool checkHand = currentPlayer.giveTile(theTile);
                if (checkHand == true)
                {
                    drawPile.RemoveAt(randomDraw);
                    //RECURSIVE CALL
                    drawTile();
                } 
            }
        }

        /// <summary>
        /// Determines if a tile can be drawn from the draw pile.
        /// </summary>
        /// <returns>A bool representing whether a tile can be drawn.</returns>
        private bool CanDrawTile()
        {
            return drawPile.Count > 0;
        }

        /// <summary>
        /// Subscribes an observer to be notified when game is updated.
        /// </summary>
        /// <param name="observer"></param>
        public void subscribe(Observer observer)
        {
            if(!subscribers.Contains(observer))
            {
                subscribers.Add(observer);
            }
        }

        /// <summary>
        /// Unsubscribes an observer from the game model.
        /// </summary>
        /// <param name="observer"></param>
        public void unsubscribe(Observer observer)
        {
            subscribers.Remove(observer);
        }
        
        /// <summary>
        /// Notifies subscribers of game model when the model changes.
        /// </summary>
        public void notifySubscribers()
        {
            foreach (Observer subscribed in subscribers)
            {
                subscribed.update();
            }
        }

        /// <summary>
        /// Get current player name
        /// </summary>
        /// <returns>string</returns>
        public string getCurrentPlayerName()
        {
            return currentPlayer.getName();
        }

        /// <summary>
        /// Get's the current player's tiles
        /// </summary>
        /// <returns>List of Tile objects</returns>
        public List<Tile> getCurrentPlayerHand()
        {
            return currentPlayer.getHand();
        }
        /// <summary>
        /// Get's the current player's money
        /// </summary>
        /// <returns></returns>
        public int getCurrentPlayerMoney()
        {
            return currentPlayer.getMoney();
        }

        /// <summary>
        /// Is active corporation
        /// </summary>
        /// <param name="corporation">corporation Enumerator</param>
        /// <returns>true if active</returns>
        public bool isActiveCorporation(ECorporation corporation)
        {
            return corporations[(int)corporation].Active;
        }

        /// <summary>
        /// Get Corporation tiles
        /// </summary>
        /// <param name="corporation">corporation enumerator</param>
        /// <returns>List of tiles</returns>
        public List<Tile> getCorporationTiles(ECorporation corporation)
        {
            return corporations[(int)corporation].Tiles;
        }

        /// <summary>
        /// Get game state
        /// </summary>
        /// <returns>GameState Enumerator</returns>
        public GameState getGameState()
        {
            return state;
        }

        /// <summary>
        /// Handles choosing the corporation by updating the state and notifying subscribers
        /// </summary>
        /// <param name="validCorps">Valid corporation list</param>
        public void handleChooseCorp(List<ECorporation> validCorps)
        {
            state = GameState.WaitCorpChoice;
            this.notifySubscribers();
        }

        /// <summary>
        /// Choose the corporation
        /// </summary>
        /// <param name="corporation">A corporation name</param>
        public void chooseCorporation(ECorporation corporation)
        {
            chooseCorpEvent.chooseCorporation(corporation);
            // event handled, free object
            chooseCorpEvent = null;
            if (this.state != GameState.WaitHandleStocks)
            {
                this.state = GameState.WaitEndTurn;
                this.notifySubscribers();
            }
        }

        /// <summary>
        /// Gets the valid corporation choices
        /// </summary>
        /// <returns>List of corporation enumerations</returns>
        public List<ECorporation> getValidCorporationChoices()
        {
            return chooseCorpEvent.getValidCorporationChoices();
        }

        /// <summary>
        /// Sets the handler for choosing corporation
        /// </summary>
        /// <param name="handler">ChooseCorpHandler</param>
        public void setHandler(ChooseCorpHandler handler)
        {
            this.chooseCorpHandler = handler;
        }

        /// <summary>
        /// Gets player stocks
        /// </summary>
        /// <param name="corporation">The corporation associated</param>
        /// <returns>Number of stocks</returns>
        public int getCurrentPlayerStocks(ECorporation corporation)
        {
            return currentPlayer.countStocks(corporation);
        }

        /// <summary>
        /// Gets the current stock value
        /// </summary>
        /// <param name="corporation">Corporation associated</param>
        /// <returns>Stock value</returns>
        public int getStockValue(ECorporation corporation)
        {
            return corporations[(int) corporation].StockPrice;
        }

        /// <summary>
        /// Handler for Stock Corporation, handles the notification to deal with defunct stock
        /// </summary>
        /// <param name="merger">The current merger taking place.</param>
        public void handleStocks(IMerger merger)
        {
            if(this.merger == null)
            {
                this.merger = merger;
                this.merger.setMergeMaker(currentPlayer);
            }
            state = GameState.WaitHandleStocks;
            MajorityAndMinorityBonuses(merger.getDefunct());
            // Deal with merge-maker's stocks
            if (currentPlayer.countStocks(merger.getDefunct()) > 0)
            {
                notifySubscribers();
            } else
                holdStocks();     
        }
        /// <summary>
        /// Pays players majority and minority stockholder bonuses
        /// </summary>
        /// <param name="defunctCorporation">The defunct corporation in the merger</param>
        private void MajorityAndMinorityBonuses(ECorporation defunctCorporation)
        {
            //Majority bonus == stockprice * 10
            int majorityPayout = corporations[(int)defunctCorporation].StockPrice * 10;
            //Minority bonus == stockprice * 5
            int minorityPayout = corporations[(int)defunctCorporation].StockPrice * 5;
            List<Player> majorityHolder = new List<Player>();
            List<Player> minorityHolder = new List<Player>();
            int majority = 0;
            int minority = 0;

            //This loop creates lists for majority and minority stockholders.
            //The list will contain multiple players if they have the same amount of stocks.
            foreach (Player player in players)
            {
                int stocks = player.countStocks(defunctCorporation);

                 if (stocks == majority && stocks != 0)
                {
                    majorityHolder.Add(player);
                }
                else if (stocks > majority)
                {
                    minorityHolder.Clear();
                    foreach (Player nowMinor in majorityHolder)
                    {
                        minorityHolder.Add(nowMinor);
                    }
                    majorityHolder.Clear();
                    majorityHolder.Add(player);
                    minority = majority;
                    majority = stocks;
                }
                else if (stocks == minority && stocks != 0)
                {
                    minorityHolder.Add(player);
                }
                else if (stocks > minority)
                {
                    minorityHolder.Clear();
                    minorityHolder.Add(player);
                    minority = stocks;
                }
            }
            //Pays out the majority players and if there is no minority players then the minority bonus is given to the majority holders as well
            foreach (Player player in majorityHolder)
            {
                player.adjustMoney(majorityPayout / majorityHolder.Count);
                if (minorityHolder.Count == 0)
                {
                    player.adjustMoney(minorityPayout / majorityHolder.Count);
                }
            }
           //Pays out the minority players
            foreach (Player player in minorityHolder)
            {
                player.adjustMoney(minorityPayout / minorityHolder.Count);
            }
        }
        /// <summary>
        /// Sells defunct stock at the current price
        /// </summary>
        /// <param name="quantity"></param>
        public void sellStocks(int quantity)
        {
            if (!currentPlayer.sellStock(bank, merger.getDefunct(), quantity))
            {
                throw new InvalidOperationException("Unable to sell stocks! Insufficient stocks");
            }
        }

        /// <summary>
        /// Trades stocks at a rate of 2:1 for the overtaking corporation stock.
        /// </summary>
        /// <param name="quantity">The number of overtaking stocks to trade for</param>
        public void tradeStocks(int quantity)
        {
            if(!bank.tradeStock(currentPlayer, merger.getDefunct(), merger.getOvertaker(), quantity))
            {
                throw new InvalidOperationException("Unable to trade stocks! Insufficient quantity in bank or in player hand.");
            }
        }

        /// <summary>
        /// Holds the defunct stocks in the players hand, and advances to next player.
        /// </summary>
        public void holdStocks()
        {
            // Advance to the next player that has stock unless we reach the mergemaker
            nextPlayer();  // Always advance at least one
            while (currentPlayer != merger.getMergeMaker())
            {
                if (currentPlayer.countStocks(merger.getDefunct()) > 0)
                {
                    state = GameState.WaitHandleStocks; // Still handling stocks
                    notifySubscribers();
                    return;
                }
                nextPlayer();
            }
            // Finish out the merger
            if(!merger.mergeNext()) 
            {
                this.merger = null;
                state = GameState.WaitEndTurn;
                notifySubscribers();
                return;
            }
        }

        /// <summary>
        /// Gets the defunct Corporation
        /// </summary>
        /// <returns>defunct corporation</returns>
        public ECorporation getDefunctCorporation()
        {
            return merger.getDefunct();
        }

        /// <summary>
        /// Gets the overtaking corporation
        /// </summary>
        /// <returns>the overtaking corporation</returns>
        public ECorporation getOvertaker()
        {
            return merger.getOvertaker();
        }

        /// <summary>
        /// Purchases stocks at the current price
        /// </summary>
        /// <param name="corporation">corporation associated</param>
        /// <param name="qty">The number of stocks (less than 3)</param>
        public void purchaseStocks(ECorporation corporation, int qty)
        {
            if (!currentPlayer.purchaseStock(bank, corporation, qty))
                throw new InvalidOperationException("Insufficient stocks in bank or not enough money!");
            notifySubscribers(); // Update view with stock changes
        }

        /// <summary>
        /// Gets the number of stocks in the bank
        /// </summary>
        /// <param name="corporation">corporation</param>
        /// <returns>number of stocks</returns>
        public int getBankStocks(ECorporation corporation)
        {
            return bank.countStocks(corporation);
        }


        /// <summary>
        /// Determines whether the end game conditions have been met
        /// </summary>
        /// <returns>(bool) true if end game conditions have been met</returns>
        public bool canEndGame()
        {
            bool isMegaCorp = false;
            int index = 0;
            int activeCorps = 0;
            int safeCorps = 0;

            // while no corporation has acquired 41 tiles, and we haven't checked all corporations...
            while (isMegaCorp == false && index < corporations.Length)
            {
                // if a corporation has 41+ tiles, end game conditions have been met
                if (corporations[index].Size > 40) isMegaCorp = true;
                // if the current corporation (at index) is safe, increase the number of safe corporations
                if (corporations[index].Active)
                {
                    activeCorps++;

                    if (corporations[index].IsSafe == true) safeCorps++;
                }
                    index++;
            }

            bool anyActiveCorps = activeCorps > 0;
            bool allActiveCorpsSafe = activeCorps == safeCorps;
            bool canEnd = isMegaCorp || (anyActiveCorps && allActiveCorpsSafe);

            return canEnd;
        }

        /// <summary>
        /// This will sell all of the players' stocks, determines the winner(s), and outputs the winners in an array(Player[]).
        /// </summary>
        /// <returns>
        /// An array of the winning player(s), with each index representing a score position,
        /// ie. index 0 contains and array with the player(s) in first place.
        /// </returns>
        public Player[][] endGame()
        {
            if (canEndGame() == true)
            {
                sellAllStocks();
                verifyAllStocksHaveBeenSold();
                sortPlayerArrayByScore();
                int numberOfDifferentScores = findNumberOfDifferentScores();
                return createScorePositionsArray(numberOfDifferentScores);
            }
            //  else the player should not have been able to initiate an end game yet
            else throw new Exception("Cannot end game until end game conditions have been met.");
        }

        /// <summary>
        /// Sells all player's stocks to the bank, used when End Game has been called for.
        /// note: Corporations already keep track of whether or not theyre active, in determining the stock price.
        /// </summary>
        private void sellAllStocks()
        {
            foreach (Player player in players)
            {
                foreach (Corporation corp in corporations)
                {
                    player.sellStock(bank, corp.Name, player.countStocks(corp.Name));
                }
            }
        }

        /// <summary>
        /// Verifies that all stocks have been sold, at the end of the game
        /// </summary>
        private void verifyAllStocksHaveBeenSold()
        {
            foreach (Corporation corp in corporations)
            {
                if (bank.countStocks(corp.Name) < 25) throw new Exception("Not all stocks were sold back to the bank.");
                if (bank.countStocks(corp.Name) > 25) throw new Exception("Bank bought back more stocks than should be possible.");
            }
        }

        /// <summary>
        /// Sorts the array of players by score (descending order)
        /// </summary>
        private void sortPlayerArrayByScore()
        {
            for (int i = 0; i < players.Length; i++)
            {
                int highScorePlayerIndex = i;
                for (int j = i + 1; j < players.Length; j++)
                {
                    if (players[j].getMoney() > players[highScorePlayerIndex].getMoney()) highScorePlayerIndex = j;
                }
                if (highScorePlayerIndex != i)
                {
                    Player temp = players[i];
                    players[i] = players[highScorePlayerIndex];
                    players[highScorePlayerIndex] = temp;
                }
            }
        }

        /// <summary>
        /// Finds the number of different scores, to help in determining tthe size to make the arrays
        /// </summary>
        /// <returns>(int) number of different scores</returns>
        private int findNumberOfDifferentScores()
        {
            int numberOfScores = 1;
            int scoreLastChecked = players[0].getMoney();
            for (int i = 1; i < players.Length; i++)
            {
                if (players[i].getMoney() != scoreLastChecked)
                {
                    numberOfScores++;
                    scoreLastChecked = players[i].getMoney();
                }
            }
            return numberOfScores;
        }

        /// <summary>
        /// creates the jagged array of players, outter array containing each bracket of scores, 
        /// inner array containing all the players within that bracket.
        /// </summary>
        /// <param name="numberOfDifferentScores">(int) number of different scores</param>
        /// <returns>(Player[][]) jagged array containing final results</returns>
        private Player[][] createScorePositionsArray(int numberOfDifferentScores)
        {
            Player[][] scorePositions = new Player[numberOfDifferentScores][];
            Player[] innerScorePositions;
            int indexForPlayerArray = 0;
            for (int i = 0; i < numberOfDifferentScores; i++)
            {
                int sizeOfInnerArray = 1;
                int j = indexForPlayerArray + 1;
                while (j < players.Length && players[j].getMoney() == players[j - 1].getMoney())
                {
                    sizeOfInnerArray++;
                    j++;
                }

                innerScorePositions = new Player[sizeOfInnerArray];
                for (int k = 0; k < sizeOfInnerArray; k++)
                {
                    innerScorePositions[k] = players[indexForPlayerArray];
                    indexForPlayerArray++;
                }
                scorePositions[i] = new Player[sizeOfInnerArray];
                scorePositions[i] = innerScorePositions;
            }
            return scorePositions;
        }


        /// <summary>
        /// Handles giving a free stock to the player during a corporation foundation
        /// </summary>
        /// <param name="corporation">Corporation</param>
        public void handleGiveFreeStock(ECorporation corporation)
        {
            bank.giveFreeStock(currentPlayer, corporation);
        }
    }



    /// <summary>
    /// An enumeration of the state the game is in.
    /// </summary>
    public enum GameState
    {
        /// <summary>
        /// State when waiting for player to select tile to play.
        /// </summary>
        WaitPlayTile,
        /// <summary>
        /// State when waiting for player to buy stocks or end turn.
        /// </summary>
        WaitEndTurn,
        /// <summary>
        /// State when waiting for corporation choice.
        /// </summary>
        WaitCorpChoice,
        /// <summary>
        /// State when waiting to pick what to do with stocks during a merger.
        /// </summary>
        WaitHandleStocks,
        /// <summary>
        /// State when the game is over.
        /// </summary>
        GameOver
    }
}
