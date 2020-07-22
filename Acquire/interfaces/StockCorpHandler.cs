using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Handles dealing with stock after a corporation merger and giving free stock
    /// </summary>
    public interface StockCorpHandler
    {
        /// <summary>
        /// Handle stocks is called by the Stock corporation that goes defunct. It's job is to handle the stocks.
        /// </summary>
        /// <param name="merger">The merger being dealt with</param>
        void handleStocks(IMerger merger);

        /// <summary>
        /// Handles giving a free stock to a corporation founder.
        /// </summary>
        /// <param name="corporation">corporation</param>
        void handleGiveFreeStock(ECorporation corporation);

        /// <summary>
        /// Sell stocks at the current price.
        /// </summary>
        /// <param name="quantity">The number of stocks</param>
        void sellStocks(int quantity);

        /// <summary>
        /// Trade stocks 2:1 for the overtaking company stock.
        /// </summary>
        /// <param name="quantity">Number of overtaking stocks</param>
        void tradeStocks(int quantity);

        /// <summary>
        /// Holds all remaining stocks in the defunct company.
        /// </summary>
        void holdStocks();

        /// <summary>
        /// Gets the defunct corporation
        /// </summary>
        /// <returns>corporation</returns>
        ECorporation getDefunctCorporation();

        /// <summary>
        /// Gets the overtaking corporation.
        /// </summary>
        /// <returns>corporation</returns>
        ECorporation getOvertaker();
    }
}
