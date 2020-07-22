using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// StockPiles are a collection of stocks for an individual corporation.
    /// Each player and the bank have their own StockPiles
    /// </summary>
    public class StockPile
    {
        private int numStocks;
        private Corporation corp;
        /// <summary>
        /// Getter
        /// </summary>
        public int NumStocks
        {
            get => numStocks;
        }
        /// <summary>
        /// Constructor for StockPile
        /// </summary>
        /// <param name="c">The Coporation for the StockPile</param>
        /// <param name="nStocks">The number of stocks in the StockPile</param>
        public StockPile(Corporation c, int nStocks = 0)
        {
            corp = c;
            numStocks = nStocks;
        }
        /// <summary>
        /// Remove stocks
        /// </summary>
        /// <param name="qty">Number of stocks to remove</param>
        /// <returns>True if removing qty of stocks is succesfull</returns>
        public bool remove(int qty)
        {
            if (this.numStocks - qty < 0)
            {
                return false;
            }
            this.numStocks -= qty;
            return true;
        }
        /// <summary>
        /// Adds stocks
        /// </summary>
        /// <param name="qty">Number of stocks to add</param>
        public void add(int qty)
        {
            this.numStocks += qty;
            
        }
        /// <summary>
        /// Gets the corresponding corporations stock price
        /// </summary>
        /// <returns>The current stock value</returns>
        public int getStockPrice()
        {
            return this.corp.StockPrice;
        }
    }
}
