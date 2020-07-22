using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// GameModel interface: controls and gets information from the game.
    /// </summary>
    public interface GameModel : Observable, PlayerInfo, CorporationInfo, StockCorpHandler, ChooseCorpEvent, ChooseCorpHandler
    {
        // Operators on GameModel
        /// <summary>
        /// Plays a tile at the position.
        /// </summary>
        /// <param name="position">string of the form '1A'</param>
        void playTile(string position);


        /// <summary>
        /// Purchases stocks for the current player
        /// </summary>
        /// <param name="corporation">The corporation to purchase stocks in</param>
        /// <param name="qty">The number of stocks to purchase</param>
        void purchaseStocks(ECorporation corporation, int qty);


        /// <summary>
        /// Advances to the next player's turn.
        /// </summary>
        void nextPlayer();


        // Getters
        /// <summary>
        /// Gets the game state.
        /// </summary   >
        /// <returns>GameState, an enumeration of states.</returns>
        GameState getGameState();

        /// <summary>
        /// Gets a tile on the board.
        /// </summary>
        /// <param name="position">string of the form '1A'</param>
        /// <returns>Tile object</returns>
        Tile getTileOnBoard(string position);

        /// <summary>
        /// Gets the number of stocks in the bank for a given corporation.
        /// </summary>
        /// <param name="corporation">The corporation</param>
        /// <returns>Number of stocks</returns>
        int getBankStocks(ECorporation corporation);

        /// <summary>
        /// Are the end game conditions met? all safe corporations or 41+ tiles in one corporation.
        /// </summary>
        /// <returns>True if the game can be ended</returns>
        bool canEndGame();
        /// <summary>
        /// Ends the game if the end game conditions are met, and determines winner.
        /// </summary>
        Player[][] endGame();
    }
}
