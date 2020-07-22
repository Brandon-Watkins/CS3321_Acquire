using Acquire.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// In charge of handling buying/selling/trading/holding stocks
    /// </summary>
    public class Bank : StockContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="corporations">Corporations in the game</param>
        /// <param name="stockQty">number of stocks (25)</param>
        public Bank(Corporation[] corporations, int stockQty) : base(corporations, stockQty)
        {

        }

        /// <summary>
        /// Gives a free stock to the player.
        /// </summary>
        /// <param name="player">player to give stock</param>
        /// <param name="corporation">corporation that has stock</param>
        /// <returns>true if bank has stock and is given to player</returns>
        public bool giveFreeStock(Player player, ECorporation corporation)
        {
            if (this.removeStock(corporation, 1))
            {
                player.addStock(corporation, 1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Trades stocks at a rate of 2:1 for the overtaking corporation stock.
        /// </summary>
        /// <param name="player">The player trading stocks</param>
        /// <param name="defunct">The defunct corporation</param>
        /// <param name="overtaker">The overtaking corporation</param>
        /// <param name="qty">The quantity of stocks to get in the overtaking corporation</param>
        /// <returns></returns>
        public bool tradeStock(Player player, ECorporation defunct, ECorporation overtaker, int qty)
        {
            // Does the bank have enough stocks?
            if (this.countStocks(overtaker) < qty)
                return false;
            // Does the player have the stocks to trade?
            int defunctQty = qty * 2;
            if (player.countStocks(defunct) < defunctQty)
            {
                return false;
            }
            // Do the trade
            player.removeStock(defunct, defunctQty);
            this.addStock(defunct, defunctQty);
            this.removeStock(overtaker, qty);
            player.addStock(overtaker, qty);
            return true;
        }
    }
}
