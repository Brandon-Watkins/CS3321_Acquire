using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Stock corporation has stocks and fires events like giving a free stock and notifying of defunct stock to the StockCorpHandler.
    /// </summary>
    interface StockCorporation
    {
        /// <summary>
        /// Adds the handler to deal with events.
        /// </summary>
        /// <param name="handler"></param>
        void addHandler(StockCorpHandler handler);
        /// <summary>
        /// Notifies the handler of the corporation going defunct.
        /// </summary>
        /// <param name="merger">The Merger we're dealing with</param>
        void notifyDefunct(IMerger merger);
    }
}
