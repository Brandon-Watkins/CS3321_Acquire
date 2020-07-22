using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Forms a corporation from a tile intersection if there is a corporation available.
    /// </summary>
    public class CorpFoundation : TileIntersection
    {
        private List<ECorporation> validCorporationChoices;
        private List<Corporation> validCorpReferences;
        /// <summary>
        /// Constructor, sets the connecting tile, and its adjacent tiles.
        /// </summary>
        /// <param name="adjTiles">(List of Tile objects) List of the adjacent tiles</param>
        /// <param name="connector">(Tile) The tile being placed, center to adjacent tiles</param>
        public CorpFoundation(Tile connector, List<Tile> adjTiles) : base(connector, adjTiles)
        {
            validCorporationChoices = new List<ECorporation>();
            validCorpReferences = new List<Corporation>();
        }

        /// <summary>
        /// Chooses the corporation to form
        /// </summary>
        /// <param name="corporation">A corporation enumeration</param>
        /// <exception cref="InvalidProgramException">Thrown when attempting to make an illegal choice</exception>
        public override void chooseCorporation(ECorporation corporation)
        {
            // Find the corporation in the valid references
            foreach (Corporation c in validCorpReferences)
            {
                if(corporation == c.Name)
                {
                    adjacentTiles.Add(connector);
                    c.formCorporation(adjacentTiles);
                    return;
                }
            }
            throw new InvalidProgramException("Unable to choose corporation. It is not inactive.");
        }

        /// <summary>
        /// Finds inactive corporations, if any.
        /// </summary>
        /// <param name="corporations">List of all possible corporations</param>
        /// <returns>(Corporation) The first inactive corporation that it finds, or (null) if none found.</returns>
        public List<Corporation> getInactiveCorporation(Corporation[] corporations)
        {
            // making an array to store all inactive corporations, for iteration 2.
            List<Corporation> inactiveCorps = new List<Corporation>();
            // indexer, to keep track of how full the array is.
            int inactiveIndex = 0;

            // for each corporation...
            for (int index = 0; index < corporations.Length; index++)
            {
                // if the corporation is not active, add the corporation to the array of inactive corps.
                if (corporations[index].Active == false)
                {
                    inactiveCorps.Add(corporations[index]);
                    inactiveIndex++;
                }
            }

            /************** modify below for iteration 2 - return array of inactive corps. **************/
           return inactiveCorps;
        }

        /// <summary>
        /// Gets a list of valid corporation choices to form. (Enumeration)
        /// </summary>
        /// <returns>list of Corporation Enumerations</returns>
        public override List<ECorporation> getValidCorporationChoices()
        {
            return validCorporationChoices;
        }

        /// <summary>
        /// Checks for adjacent tiles, to see if any tiles can be used to form a corporation. 
        /// If so, and an inactive corporation exists, it forms a corporation.
        /// </summary>
        /// <param name="corporations">(Array) of all corporations</param>
        public override bool resolveIntersection(Corporation[] corporations)
        {
            // are there any inactive corporations?
            List<Corporation> inactiveCorps = getInactiveCorporation(corporations);
            if (inactiveCorps.Count == 0)
                throw new TemporaryUnplayableTileException(connector.getPosition());
            // if only 1, form corporation
            else if (inactiveCorps.Count == 1)
             {
                adjacentTiles.Add(connector);
                inactiveCorps[0].formCorporation(adjacentTiles);
                return true;
            } else
            {
                foreach (Corporation c in inactiveCorps)
                {
                    validCorporationChoices.Add(c.Name);
                    validCorpReferences.Add(c);
                }
                handler.handleChooseCorp(validCorporationChoices);
            }
            return false;
        }
    }
}

