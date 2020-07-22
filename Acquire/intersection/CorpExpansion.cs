using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Expands a corporation when intersecting with unincorporated tiles
    /// </summary>
    public class CorpExpansion : TileIntersection
    {
        /// <summary>
        /// Constructor, sets the connecting tile, and its adjacent tiles.
        /// </summary>
        /// <param name="adjTiles">(List of Tile objects) List of the adjacent tiles</param>
        /// <param name="connector">(Tile) The tile being placed, center to adjacent tiles</param>
        public CorpExpansion(Tile connector, List<Tile> adjTiles) : base(connector, adjTiles)
        {
        }
        /// <summary>
        /// Corporations cannot be selected during a standard expansion
        /// </summary>
        /// <param name="corporation">Corporation chosen</param>
        public override void chooseCorporation(ECorporation corporation)
        {
            throw new InvalidOperationException("Unable to choose a corporation during an expansion.");
        }
        /// <summary>
        /// This should not be called in this phase
        /// </summary>
        /// <returns>Nothing</returns>
        public override List<ECorporation> getValidCorporationChoices()
        {
            return null;
        }

        /// <summary>
        /// Expands the corporation to include the unincorporated tiles.
        /// </summary>
        /// <param name="corporations">A list of corporations</param>
        public override bool resolveIntersection(Corporation[] corporations)
        {
            List<Tile> unincorporated = new List<Tile>();
            Corporation corporation = null;
            foreach (Tile t in adjacentTiles)
            {
                if (t.Corp != null)
                    corporation = t.Corp;
                else
                    unincorporated.Add(t);
            }
            unincorporated.Add(connector);
            corporation.expand(unincorporated);
            return true;
        }
    }
}
