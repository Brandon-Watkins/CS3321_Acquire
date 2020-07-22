using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Interface for getting a players information 
    /// </summary>
    public interface IPlayerInfo
    {
        /// <summary>
        /// Gets the player's name
        /// </summary>
        /// <returns>string</returns>
        string getPlayerName();
        /// <summary>
        /// Gets the player hand
        /// </summary>
        /// <returns>List of tiles</returns>
        List<Tile> getPlayerHand();
        /// <summary>
        /// Gets the players stock count
        /// </summary>
        /// <param name="corporation">The corporation associated with the stock</param>
        /// <returns>The number of stocks</returns>
        int getPlayerStocks(ECorporation corporation);
        /// <summary>
        /// Gets the player money
        /// </summary>
        /// <returns>integer value</returns>
        int getPlayerMoney();
                
    }
}
