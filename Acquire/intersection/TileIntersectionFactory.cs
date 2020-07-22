using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Factory that determines and generates a tile intersection
    /// </summary>
    public class TileIntersectionFactory
    {
        /// <summary>
        /// Creates the tile intersection
        /// </summary>
        /// <remarks>If there are no surrounding corporations, returns a CorpFoundation.
        /// If there is only one, returns a CorpExpansion.
        /// If there is more than one, returns a merger.</remarks>
        /// <param name="connector">The connecting tile</param>
        /// <param name="adjTiles">The adjacent tiles</param>
        /// <returns>TileIntersection object</returns>
        public TileIntersection determineIntersection(Tile connector, List<Tile> adjTiles)
        {
            int corpCount = 0;          // count the corporations surrounding
            List<Corporation> corpsInTiles = new List<Corporation>();
            foreach (Tile t in adjTiles)
            {
                Corporation corp = t.Corp;
                if (corp != null && !corpsInTiles.Contains(corp))   // Not null and not already in list
                {
                    corpCount++;
                    corpsInTiles.Add(corp);
                }
            }
            if(corpCount == 0)
            {
                return new CorpFoundation(connector, adjTiles);
            } 
            else if (corpCount == 1)
            {
                return new CorpExpansion(connector, adjTiles);
            }
            else
            {
                return new Merger(connector, adjTiles);
            }
        }
    }
}
