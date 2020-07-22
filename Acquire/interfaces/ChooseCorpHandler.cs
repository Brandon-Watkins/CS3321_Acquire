using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Handles choosing a corporation event from ChooseCorpEvent
    /// </summary>
    public interface ChooseCorpHandler
    {
        /// <summary>
        /// Handles choosing a corporation
        /// </summary>
        /// <param name="validCorps">Valid corporation choices</param>
        void handleChooseCorp(List<ECorporation> validCorps);
    }
}
