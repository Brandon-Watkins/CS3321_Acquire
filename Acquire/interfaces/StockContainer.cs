using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Collection of Stockpiles
    /// </summary>
    public abstract class StockContainer
    {
        /// <summary>
        /// The collection of Stockpiles
        /// </summary>
        protected Dictionary<ECorporation, StockPile> stocks;
        /// <summary>
        /// Creates the Stockpile
        /// </summary>
        /// <param name="corporations">The corporations in the game</param>
        /// <param name="stockqty">number of stocks. The bank is the only object to override the default param of 0</param>
        public StockContainer(Corporation[] corporations, int stockqty = 0)
        {
            stocks = new Dictionary<ECorporation, StockPile>();
            foreach (Corporation x in corporations)
            {
                stocks.Add(x.Name, new StockPile(x, stockqty));
            }
        }
        /// <summary>
        /// Add stocks to a StockPile
        /// </summary>
        /// <param name="corp">The corporation the stocks belong to</param>
        /// <param name="qty">number of stocks to add</param>
        public void addStock(ECorporation corp, int qty)
        {
            stocks[corp].add(qty);
        }
        /// <summary>
        /// Removes stocks from a StockPile
        /// </summary>
        /// <param name="corp">The coporation the stocks belong to</param>
        /// <param name="qty">Number of stocks to remove</param>
        /// <returns></returns>
        public bool removeStock(ECorporation corp, int qty)
        {
            return stocks[corp].remove(qty);
        }
        /// <summary>
        /// Returns the number of stocks in a Stockpile
        /// </summary>
        /// <param name="corp">The corporation the stocks belong to</param>
        /// <returns></returns>
        public int countStocks(ECorporation corp)
        {
            return stocks[corp].NumStocks;
        }
    }
}

