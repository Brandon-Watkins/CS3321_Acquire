using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire.interfaces
{
    /// <summary>
    /// Interface for getting corporation information
    /// </summary>
    public interface CorporationInfo
    {
        /// <summary>
        /// Is active?
        /// </summary>
        /// <param name="corporation">Corporation</param>
        /// <returns>True if active</returns>
        bool isActiveCorporation(ECorporation corporation);

        /// <summary>
        /// Get corporation Tiles
        /// </summary>
        /// <param name="corporation">corporation</param>
        /// <returns>List of tiles</returns>
        List<Tile> getCorporationTiles(ECorporation corporation);
        /// <summary>
        /// Gets the current stock value of a corporation
        /// </summary>
        /// <param name="corporation">corporation</param>
        /// <returns>the current value</returns>
        int getStockValue(ECorporation corporation);
    }
}
