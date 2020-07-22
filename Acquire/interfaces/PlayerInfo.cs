using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Interface for getting various information about the player.
    /// </summary>
    public interface PlayerInfo
    {
        /// <summary>
        /// Gets the current player name
        /// </summary>
        /// <returns>name</returns>
        string getCurrentPlayerName();

        /// <summary>
        /// Gets the current player hand
        /// </summary>
        /// <returns>hand</returns>
        List<Tile> getCurrentPlayerHand();

        /// <summary>
        /// Gets the current player's number of stocks in a given corporation.
        /// </summary>
        /// <param name="corporation">The corporation</param>
        /// <returns>number of stocks</returns>
        int getCurrentPlayerStocks(ECorporation corporation);

        /// <summary>
        /// Gets the current player's money
        /// </summary>
        /// <returns>number of money's</returns>
        int getCurrentPlayerMoney();
    }
}
