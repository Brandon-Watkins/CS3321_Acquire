using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// An event that is fired when needing to choose a corporation.
    /// </summary>
    public interface ChooseCorpEvent
    {
        /// <summary>
        /// Chooses the corporation.
        /// </summary>
        /// <param name="corporation">One corporation from the valid corporations</param>
        void chooseCorporation(ECorporation corporation);
        /// <summary>
        /// Get valid corporation choices.
        /// </summary>
        /// <returns>list of ECorporations</returns>
        List<ECorporation> getValidCorporationChoices();
        /// <summary>
        /// Sets the handler that will be notified to choose a corporation.
        /// </summary>
        /// <param name="handler">The handler</param>
        void setHandler(ChooseCorpHandler handler);
    }
}
