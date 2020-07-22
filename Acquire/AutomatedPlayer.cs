using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    class AutomatedPlayer : Player, Observer
    {
        private GameModel game;
        private static readonly Random random = new Random();
        /// <summary>
        /// Constructor for AI.
        /// </summary>
        /// <param name="name">Name of AI</param>
        /// <param name="game">The game to get notifications and control</param>
        public AutomatedPlayer(string name, Game game) : base(name) 
        {
            game.subscribe(this);
            this.game = game;
        }

        /// <summary>
        /// Receives updates from the game and plays randomly if its their turn
        /// </summary>
        public void update()
        {
            if(game.getCurrentPlayerName() == getPlayerName()) // Is it our turn???
            {
                GameState state = game.getGameState();
                if(state == GameState.WaitPlayTile)
                {
                    var hand = getPlayerHand();
                    bool played = false;
                    int attempts = 0;
                    while (!played && attempts < 20)
                    {
                        try
                        {
                            int rand = random.Next(0, 5);
                            game.playTile(hand[rand].getPosition());
                        }
                        catch
                        {

                        }
                        attempts++;
                    }
                }
                else if(state == GameState.WaitCorpChoice)
                {
                    var choices = game.getValidCorporationChoices();
                    int rand = random.Next(0, choices.Count - 1);
                    game.chooseCorporation(choices[rand]);
                }
                else if(state == GameState.WaitHandleStocks)
                {
                    // Todo handle trading/selling stocks
                }
                else if(state == GameState.WaitEndTurn)
                {
                    // Todo buy stocks

                    game.nextPlayer(); // advance
                } 
            }
        }

    }
}
