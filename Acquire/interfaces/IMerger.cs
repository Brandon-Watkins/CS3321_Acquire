using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Interface for calling merge on a corporation
    /// </summary>
    public interface IMerger
    {
        /// <summary>
        /// Advances merge process
        /// </summary>
        /// <returns>true if there's more to do, false if done</returns>
        bool mergeNext();

        /// <summary>
        /// Sets the mergemaker for the current merger
        /// </summary>
        /// <param name="player">mergemaker player</param>
        void setMergeMaker(Player player);

        /// <summary>
        /// Gets the mergemaker for the current merger
        /// </summary>
        /// <returns>The mergemaker</returns>
        Player getMergeMaker();

        /// <summary>
        /// Gets defunct Corporation
        /// </summary>
        /// <returns>corporation name</returns>
        ECorporation getDefunct();

        /// <summary>
        /// Gets overtaking corporation
        /// </summary>
        /// <returns>corporation name</returns>
        ECorporation getOvertaker();
    }
}
