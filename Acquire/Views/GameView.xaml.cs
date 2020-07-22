using Acquire.interfaces;
using Acquire.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Acquire
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : Window, Observer
    {
        #region Member variables
        /// <summary>
        /// Holds the instance of the <see cref="GameModel"/> with which the view communicates.
        /// </summary>
        private GameModel gameModel;

        /// <summary>
        /// Holds the instance of the style class from which colors are referenced.
        /// </summary>
        private AquireStyle aquireStyle;

        /// <summary>
        /// Contains the buttons which make up the hand of the current <see cref="Player"/>.
        /// </summary>
        private List<Button> playerHandView;

        /// <summary>
        /// Associates <see cref="Corporation"/> buttons with each member of <see cref="ECorporation"/>.
        /// </summary>
        private Dictionary<ECorporation, Button> corporationsView;
        
        /// <summary>
        /// Associates <see cref="Bank"/> stock labels with each member of <see cref="ECorporation"/>.
        /// </summary>
        private Dictionary<ECorporation, Label> bankStocksView;

        /// <summary>
        /// Associates <see cref="Player"/> stock labels with each member of <see cref="ECorporation"/>.
        /// </summary>
        private Dictionary<ECorporation, Label> playerStocksView;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="GameView"/> view.
        /// </summary>
        /// <param name="playerNames">List of player names to initialize the <see cref="GameModel"/>.</param>
        public GameView(List<string> playerNames)
        {
            InitializeComponent();

            // Initialize game model and subscribe to it.
            gameModel = new Game(playerNames.ToArray());
            gameModel.subscribe(this);

            // Initialize style class;
            aquireStyle = new AquireStyle();

            // Collect certain view controls into groups and initialize color dictionaries.
            InitializePlayerStockLabels();
            InitializeBankStockLabels();
            InitializePlayerHandButtons();
            InitializeCorporationButtons();

            // Create and update view.
            CreateGameBoardView();
            update();
        }

        #endregion

        #region Member methods
        /// <summary>
        /// Defines the associations between <see cref="Player"/> stock labels and the members of <see cref="ECorporation"/>.
        /// </summary>
        private void InitializePlayerStockLabels()
        {
            playerStocksView = new Dictionary<ECorporation, Label>
            {
                { ECorporation.American, lbl_Player_American },
                { ECorporation.Imperial, lbl_Player_Imperial },
                { ECorporation.Continental, lbl_Player_Continental },
                { ECorporation.Festival, lbl_Player_Festival },
                { ECorporation.WorldWide, lbl_Player_Worldwide },
                { ECorporation.Sackson, lbl_Player_Sackson },
                { ECorporation.Tower, lbl_Player_Tower }
            };
        }

        /// <summary>
        /// Defines the associations between <see cref="Bank"/> stock labels and the members of <see cref="ECorporation"/>.
        /// </summary>
        private void InitializeBankStockLabels()
        {
            bankStocksView = new Dictionary<ECorporation, Label>
            {
                { ECorporation.American, lbl_Bank_American },
                { ECorporation.Imperial, lbl_Bank_Imperial },
                { ECorporation.Continental, lbl_Bank_Continental },
                { ECorporation.Festival, lbl_Bank_Festival },
                { ECorporation.WorldWide, lbl_Bank_Worldwide },
                { ECorporation.Sackson, lbl_Bank_Sackson },
                { ECorporation.Tower, lbl_Bank_Tower }
            };
        }

        /// <summary>
        /// Defines the associations between <see cref="Corporation"/> buttons and the members of <see cref="ECorporation"/>.
        /// </summary>
        private void InitializeCorporationButtons()
        {
            corporationsView = new Dictionary<ECorporation, Button>
            {
                { ECorporation.American, btn_CorpAmerican },
                { ECorporation.Imperial, btn_CorpImperial },
                { ECorporation.Continental, btn_CorpContinental },
                { ECorporation.Festival, btn_CorpFestival },
                { ECorporation.WorldWide, btn_CorpWorldwide },
                { ECorporation.Sackson, btn_CorpSackson },
                { ECorporation.Tower, btn_CorpTower }
            };

            SetCorpButtonContent();
        }

        /// <summary>
        /// Sets the content of each <see cref="Corporation"/> button with the name
        /// of that corporation.
        /// </summary>
        private void SetCorpButtonContent()
        {
            foreach (KeyValuePair<ECorporation, Button> corpEntry in corporationsView)
            {
                ECorporation currentCorp = corpEntry.Key;
                Button currentButton = corpEntry.Value;

                currentButton.Content = currentCorp;
            }
        }

        /// <summary>
        /// Groups <see cref="Player"/> hand buttons together for convenient access.
        /// </summary>
        private void InitializePlayerHandButtons()
        {
            playerHandView = new List<Button>
            {
                btn_Hand1,
                btn_Hand2,
                btn_Hand3,
                btn_Hand4,
                btn_Hand5,
                btn_Hand6
            };
        }

        /// <summary>
        /// Initiates all game board view initialization methods.
        /// </summary>
        private void CreateGameBoardView()
        {
            CreateGameBoardRowsAndColumns();
            CreateGameBoardButtons();
        }

        /// <summary>
        /// Fills game board grid with buttons for each possible <see cref="Tile"/>.
        /// </summary>
        private void CreateGameBoardButtons()
        {
            // Create button for each space
            for (int i = 0; i < Board.MAX_ROWS; i++)
            {
                for (int j = 0; j < Board.MAX_COLUMNS; j++)
                {

                    Button newButton = new Button()
                    {
                        Name = $"btn_{j + 1}{(char)(i + 65)}",
                        Content = $"{(char)(i + 65)}-{j + 1}",
                        FontSize = 18,
                        FontFamily = new FontFamily("PenultimateLight"),
                        FontWeight = FontWeights.ExtraBold,
                        Background = Brushes.SlateGray
                    };

                    grd_GameBoard.Children.Add(newButton);
                    Grid.SetRow(newButton, i);
                    Grid.SetColumn(newButton, j);
                }
            }
        }

        /// <summary>
        /// Defines rows and columns for the game board view.
        /// </summary>
        private void CreateGameBoardRowsAndColumns()
        {
            CreateGameBoardRows();
            CreateGameBoardColumns();
        }

        /// <summary>
        /// Defines the columns for the game board view.
        /// </summary>
        private void CreateGameBoardColumns()
        {
            // Create necessary column definitions
            for (int i = 0; i < Board.MAX_COLUMNS; i++)
            {
                grd_GameBoard.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        /// <summary>
        /// Defines the rows for the game board view.
        /// </summary>
        private void CreateGameBoardRows()
        {
            // Create necessary row definitions
            for (int i = 0; i < Board.MAX_ROWS; i++)
            {
                grd_GameBoard.RowDefinitions.Add(new RowDefinition());
            }
        }

        /// <summary>
        /// Updates the amount of stocks owned by the current <see cref="Player"/>.
        /// </summary>
        private void UpdatePlayerStocks()
        {
            foreach (ECorporation corpName in Enum.GetValues(typeof(ECorporation)))
            {
                playerStocksView[corpName].Content = gameModel.getCurrentPlayerStocks(corpName);
            }
        }

        /// <summary>
        /// Updates the amount of stocks owned by the <see cref="Bank"/>.
        /// </summary>
        private void UpdateBankStocks()
        {
            foreach (ECorporation corpName in Enum.GetValues(typeof(ECorporation)))
            {
                bankStocksView[corpName].Content = gameModel.getBankStocks(corpName);
            }
        }

        /// <summary>
        /// Updates the name of the current <see cref="Player"/>.
        /// </summary>
        private void UpdatePlayerNameView()
        {
            string playerName = gameModel.getCurrentPlayerName();
            grp_PlayerInfo.Header = playerName;
            lbl_PlayerMoney.Content = "$" + gameModel.getCurrentPlayerMoney();
        }

        /// <summary>
        /// Queries the <see cref="GameModel"/> for hand of the current <see cref="Player"/>
        /// and initiate methods to update them.
        /// </summary>
        private void UpdatePlayerHandView()
        {
            List<Tile> playerHand = gameModel.getCurrentPlayerHand();
            DrawPlayerHand(playerHand);
        }

        /// <summary>
        /// Updates each button representing the hand of the current <see cref="Player"/>
        /// with the appropriate <see cref="Tile"/> position.
        /// </summary>
        /// <param name="playerHand"></param>
        private void DrawPlayerHand(List<Tile> playerHand)
        {
            // First disable each button so extra spaces aren't clickable.
            foreach (Button button in playerHandView)
            {
                button.Content = "";
                button.IsEnabled = false;
            }

            // Change the content and enable for each Tile in the player's hand.
            for (int i = 0; i < playerHand.Count; i++)
            {
                string positionString = playerHand[i].getPosition();

                string row = positionString.Last().ToString();
                string column = positionString.Replace(row, "");

                playerHandView[i].Content = $"{ row }-{ column }";

                if (GameStateIsWaitPlaytile())
                {
                    playerHandView[i].IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Toggles appearance of corporation buttons to signify whether a 
        /// <see cref="Corporation"/> is active or inactive.
        /// </summary>
        private void UpdateActiveCorpView()
        {
            foreach (ECorporation corpName in Enum.GetValues(typeof(ECorporation)))
            {
                // By default all corporation buttons are shown to be inactive.
                Button button = corporationsView[corpName];
                button.Background = aquireStyle.GetInactiveCorpColor(corpName);
                button.BorderThickness = new Thickness(8);

                // Change the appearance of buttons corresponding to active corporations.
                if (gameModel.isActiveCorporation(corpName))
                {
                    button.Background = aquireStyle.GetActiveCorpColor(corpName);
                    button.BorderThickness = new Thickness(2);
                }
            }
        }

        /// <summary>
        /// Updates the options for the current <see cref="Player"/>
        /// in choosing a <see cref="Corporation"/>.
        /// </summary>
        private void UpdateCorpChoices()
        {
            // Are we in the choosing state?
            if(gameModel.getGameState() == GameState.WaitCorpChoice)
            {
                // get the valid corporations
                List<ECorporation> validCorps = gameModel.getValidCorporationChoices();
                foreach(KeyValuePair<ECorporation, Button> keyValuePair in corporationsView)
                {
                    // Activate buttons that are valid choices
                    if (validCorps.Contains(keyValuePair.Key))
                        keyValuePair.Value.IsEnabled = true;
                    // Deactivate all others
                    else
                        keyValuePair.Value.IsEnabled = false;
                }
            } 
            else   // enable all (for appearances)
            {
                foreach (KeyValuePair<ECorporation, Button> keyValuePair in corporationsView)
                {
                    keyValuePair.Value.IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Initiates methods to update game board view.
        /// </summary>
        private void UpdateGameBoardView()
        {
            DrawGameBoard();
        }

        /// <summary>
        /// Updates the appearance of each cell in the game board according to it's
        /// corresponding <see cref="Tile"/>'s state.
        /// </summary>
        private void DrawGameBoard()
        {
            foreach (Button button in grd_GameBoard.Children)
            {
                string buttonName = button.Name;

                // Button name format: "btn_{row}{col}"
                string correspondingTilePos = buttonName.Replace("btn_", "");
                Tile correspondingTile = gameModel.getTileOnBoard(correspondingTilePos);
                if (correspondingTile != null)
                {
                    if (correspondingTile.Corp != null)
                    {
                        // Retrieve correct enum member for color lookup.
                        ECorporation corpName = correspondingTile.Corp.Name;
                        button.Background = aquireStyle.GetActiveCorpColor(corpName);

                        if (gameModel.getCorporationTiles(corpName).Count >= 11)
                        {
                            button.BorderBrush = Brushes.Black;
                            button.BorderThickness = new Thickness(4);
                        }
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                }
                else button.Background = Brushes.SlateGray;
            }
        }

        /// <summary>
        /// Retrieves a button on the board view based on a button in the hand view.
        /// </summary>
        /// <param name="handButtonContent">The hand button with which to find a 
        /// corresponding board button.</param>
        /// <returns>A button in the board grid corresponding to the hand button.</returns>
        private Button FindBoardButtonWithHandButton(string handButtonContent)
        {
            // Hand button content format: "{row}-{column}"

            // 0: row; 1:column
            string[] splitContent = handButtonContent.Split('-');

            // Board button name format: "btn_{column}{row}"
            string correspondingBoardButtonName = $"btn_{splitContent[1]}{splitContent[0]}";

            foreach (Button button in grd_GameBoard.Children)
            {
                if (button.Name == correspondingBoardButtonName)
                {
                    return button;
                }
            }

            throw new IndexOutOfRangeException($"Button: {correspondingBoardButtonName} could not be found!");
        }

        /// <summary>
        /// Applies a color to the background of a button.
        /// </summary>
        /// <param name="button">Button to highlight.</param>
        /// <param name="highlightColor">Color to highlight with.</param>
        private void ApplyColorToButton(Button button, Color highlightColor)
        {
            string handButtonContent = button.Content.ToString();

            if (handButtonContent != "")
            {
                Button correspondingBoardButton = FindBoardButtonWithHandButton(handButtonContent);
                correspondingBoardButton.Background = new SolidColorBrush(highlightColor); 
            }
        }

        /// <summary>
        /// Toggles whether end turn is enabled based on <see cref="GameModel"/> state.
        /// </summary>
        private void UpdateEndTurn()
        {
            if (GameStateIsWaitEndTurn())
            {
                btn_EndTurn.IsEnabled = true;
                btn_PurchaseStocks.IsEnabled = true;
            }
            else
            {
                btn_EndTurn.IsEnabled = false;
                btn_PurchaseStocks.IsEnabled = false;
            }
        }

        /// <summary>
        /// Displays the winners of the game in a message box.
        /// </summary>
        /// <param name="winners"></param>
        private void DisplayWinners(Player[][] winners)
        {
            string firstPlaceNames = ExtractPlaceWinnerNamesAndScores(1, winners);
            string secondPlaceNames = ExtractPlaceWinnerNamesAndScores(2, winners);
            string thirdPlaceNames = ExtractPlaceWinnerNamesAndScores(3, winners);

            MessageBox.Show(
                $"1st: {firstPlaceNames}\n" +
                $"2nd: {secondPlaceNames}\n" +
                $"3rd: {thirdPlaceNames}\n",
                "Winner!");
        }

        /// <summary>
        /// Retrieves winner names from a certian placement.
        /// </summary>
        /// <param name="place">Which place to get names from.</param>
        /// <param name="winners">Group from which to extract names.</param>
        /// <returns>The names of the winners in the specified place.</returns>
        private string ExtractPlaceWinnerNamesAndScores(int place, Player[][] winners)
        {
            int index = place - 1;
            string placeNames = "";

            if (index < winners.Length)
            {
                for (int i = 0; i < winners[index].Length; i++)
                {
                    placeNames += winners[index][i].getName();
                    placeNames += $" - (${winners[index][i].getMoney()})";

                    if (i < winners[index].Length - 1)
                    {
                        placeNames += ", ";
                    }
                } 
            }

            return placeNames;
        }
    
        /// <summary>
        /// Creates a <see cref="StockMerger"/> view if a merger is happening.
        /// </summary>
        private void UpdateHandleStocks()
        {
            if (GameStateIsWaitHandleStocks())
            {
                if (!IsWindowOpen<Window>("StockMerger"))
                {
                    StockMerger stockMerger = new StockMerger(gameModel);
                    stockMerger.Show();
                }
            }
        }

        /// <summary>
        /// Updates the End Game button based on end game conditions.
        /// </summary>
        private void UpdateCanEndGame()
        {
            if (gameModel.canEndGame())
            {
                btn_EndGame.IsEnabled = true;
            }
            else btn_EndGame.IsEnabled = false;
        }

        #endregion



        #region Event Listeners
        /// <summary>
        /// Allows the current <see cref="Player"/> to choose a <see cref="Corporation"/>.
        /// </summary>
        private void CorporationButton_Click(object sender, RoutedEventArgs e)
        {
            if (GameStateIsWaitCorpChoice())
            {
                Button button = (Button)sender;

                // determine the button associated with corporation
                foreach (KeyValuePair<ECorporation, Button> corporation in corporationsView)
                {
                    if (corporation.Value == button)
                    {
                        gameModel.chooseCorporation(corporation.Key); // Choose the corporation
                        break; // choose only one
                    }
                }
            }
        }

        /// <summary>
        /// Allows the current <see cref="Player"/> to quit the game.
        /// </summary>
        private void btn_Quit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you would like to quit? Your progress will not be saved!",
                "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                IntroScreen introScreen = new IntroScreen();
                introScreen.Show();
                this.Close();
            }
        }

        /// <summary>
        /// Allows the current <see cref="Player"/> to choose a <see cref="Tile"/> in their hand.
        /// </summary>
        private void HandTileButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            string[] buttonPosition = button.Content.ToString().Split('-');
            string tilePosition = $"{buttonPosition[1]}{buttonPosition[0]}";
            try
            {
                gameModel.playTile(tilePosition);
            } 
            catch (TemporaryUnplayableTileException)
            {
                MessageBox.Show("The tile " + tilePosition
                    + " is temporarily unplayable because it would form a corporation but none are available!");
            }
            catch (PermanentUnplayableTileException)
            {
                MessageBox.Show("The tile " + tilePosition + " isn't playable. Please, play another.");
            }
        }

        /// <summary>
        /// Allows the current <see cref="Player"/> to end their turn.
        /// </summary>
        private void btn_EndTurn_Click(object sender, RoutedEventArgs e)
        {
            gameModel.nextPlayer();
        }

        private void btn_Purchase_Click(object sender, RoutedEventArgs e)
        {
            if (GameStateIsWaitEndTurn())
            {
                BuyStocks buyStocks = new BuyStocks(gameModel);
                buyStocks.Show();
            }
        }

        /// <summary>
        /// Allows the current <see cref="Player"/> to end the game.
        /// </summary>
        private void btn_EndGame_Click(object sender, RoutedEventArgs e)
        {
            Player[][] winners = gameModel.endGame();

            DisplayWinners(winners);

            IntroScreen introScreen = new IntroScreen();
            introScreen.Show();
            this.Close();
        }

        /// <summary>
        /// Highlights a <see cref="Tile"/> on the game board when the mouse enters
        /// a <see cref="Tile"/> in the hand of the current <see cref="Player"/>.
        /// </summary>
        private void HighlightTileOnBoard_MouseEnter(object sender, RoutedEventArgs e)
        {
            ApplyColorToButton((Button)sender, Colors.White);
        }

        /// <summary>
        /// Returns a <see cref="Tile"/> to it's original color when the mouse leaves
        /// a <see cref="Tile"/> in the hand of the current <see cref="Player"/>.
        /// </summary>
        private void UnhighlightTileOnBoard_MouseLeave(object sender, RoutedEventArgs e)
        {
            ApplyColorToButton((Button)sender, Colors.SlateGray);
        }

        /// <summary>
        /// Determines if a window is open.
        /// </summary>
        /// <typeparam name="T">The specific type of the window inheritting from 
        /// <see cref="Window"/>.</typeparam>
        /// <param name="name">The name of the window.</param>
        /// <returns>True if open</returns>
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        /// <summary>
        /// Determines if the <see cref="Game"/> is waiting for the <see cref="Player"/>
        /// to play a tile.
        /// </summary>
        /// <returns>Whether the game state is <see cref="GameState.WaitPlayTile"/>.</returns>
        private bool GameStateIsWaitPlaytile()
        {
            return gameModel.getGameState() == GameState.WaitPlayTile;
        }

        /// <summary>
        /// Determines if the <see cref="Game"/> is waiting to handle stocks.
        /// </summary>
        /// <returns>Whether the game state is <see cref="GameState.WaitHandleStocks"/>.</returns>
        private bool GameStateIsWaitHandleStocks()
        {
            return gameModel.getGameState() == GameState.WaitHandleStocks;
        }

        /// <summary>
        /// Determines if the <see cref="Game"/> is waiting for the <see cref="Player"/>
        /// to choose a <see cref="Corporation"/>.
        /// </summary>
        /// <returns>Whether the game state is <see cref="GameState.WaitCorpChoice"/>.</returns>
        private bool GameStateIsWaitCorpChoice()
        {
            return gameModel.getGameState() == GameState.WaitCorpChoice;
        }

        /// <summary>
        /// Determines if the <see cref="Game"/> is waiting for the <see cref="Player"/>
        /// to buy stocks or end their turn.
        /// </summary>
        /// <returns>Whether the game state is <see cref="GameState.WaitEndTurn"/>.</returns>
        private bool GameStateIsWaitEndTurn()
        {
            return gameModel.getGameState() == GameState.WaitEndTurn;
        }

        #endregion

        #region Observer implementation
        /// <summary>
        /// Updates all aspects of view.
        /// </summary>
        public void update()
        {
            UpdateHandleStocks();
            UpdatePlayerNameView();
            UpdatePlayerHandView();
            UpdatePlayerStocks();
            UpdateBankStocks();
            UpdateActiveCorpView();
            UpdateGameBoardView();
            UpdateEndTurn();
            UpdateCorpChoices();
            UpdateCanEndGame();
        }
        #endregion
    }
}
