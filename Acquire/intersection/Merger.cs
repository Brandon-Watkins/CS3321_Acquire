using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acquire.interfaces;

namespace Acquire
{
    /// <summary>
    /// Handles merging corporations when a tile is placed
    /// </summary>s
    public class Merger : TileIntersection, IMerger
    {
        private List<ECorporation> validCorporationChoices;
        private List<Corporation> mergerCorporations; // sorted list from biggest to smallest
        private Corporation overtaker;
        private Corporation defunct;
        private Player mergeMaker;

        /// <summary>
        /// Constructor, sets the connecting tile, and its adjacent tiles.
        /// </summary>
        /// <param name="adjTiles">(List of Tile objects) List of the adjacent tiles</param>
        /// <param name="connector">(Tile) The tile being placed, center to adjacent tiles</param>
        public Merger(Tile connector, List<Tile> adjTiles) : base(connector, adjTiles)
        {
            validCorporationChoices = new List<ECorporation>();
            initializeMergingCorporations();
        }

        /// <summary>
        /// Chooses the corporation that will survive in a tie between largest size merger
        /// </summary>
        /// <param name="corporation">The survivor or overtaker</param>
        /// <exception cref="InvalidProgramException">Thrown when making an illegal corporation choice</exception>
        public override void chooseCorporation(ECorporation corporation)
        {
            if(!validCorporationChoices.Contains(corporation))     // Illegal choice
            {
                throw new InvalidProgramException("Corporation is not a valid choice.");
            }
            foreach (Corporation corporation1 in mergerCorporations)
            {
                if (corporation1.Name == corporation)
                {
                    overtaker = corporation1;
                    notifyDefunctStock();
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the valid corporation choices when choosing a survivor of a merger.
        /// </summary>
        /// <returns>list of corporations enumeration</returns>
        public override List<ECorporation> getValidCorporationChoices()
        {
            return validCorporationChoices;
        }

        /// <summary>
        /// Resolves the intersection by attempting to merge the corporations in the adjacent tiles.
        /// </summary>
        /// <param name="corporations">List of all the corporations</param>
        /// <exception cref="PermanentUnplayableTileException">Thrown when trying to merge multiple safe corporations.</exception>
        public override bool resolveIntersection(Corporation[] corporations)
        {
             // Check for multiple safe corporations
            if(mergerCorporations[0].IsSafe && mergerCorporations[1].IsSafe)
                throw new PermanentUnplayableTileException(connector.getPosition());

            // Largest size 
            int largestSize = mergerCorporations[0].Size;
            // Choose the corporation if the largest are the same size
            if (largestSize == mergerCorporations[1].Size)
            {
                // Gather corporation choices (could be more than 2)
                foreach (Corporation c in mergerCorporations)
                {
                    if (c.Size == largestSize)
                        validCorporationChoices.Add(c.Name);
                }
                handler.handleChooseCorp(validCorporationChoices);
            } 
            else // different sizes
            {
                overtaker = mergerCorporations[0];
                notifyDefunctStock();
        
            }
            return false;
        }

        /// <summary>
        /// Performs the merging of corporations.
        /// </summary>
        private bool notifyDefunctStock()
        {
            // Find the next defunct corporation, and notify that its defunct
            bool foundDefunct = (defunct == null);
            foreach(Corporation c in mergerCorporations)
            {
                if (c == defunct)
                    foundDefunct = true;
                else if(c != overtaker && foundDefunct)
                {
                    defunct = c;
                    // Notify it's defunct, to handle stocks. Pass in this to be able to return execution here.
                    c.notifyDefunct((IMerger)this);
                    return true;
                }
            }
            // No corporations left to handle stocks
            return false;
        }


        /// <summary>
        /// Gets the unincorporated tiles surrounding the intersection including the connector.
        /// </summary>
        /// <returns>List of Tiles</returns>
        private List<Tile> getUnincorporatedTiles()
        {
            List<Tile> unincorporated = new List<Tile>();
            foreach (Tile t in adjacentTiles)
            {
                if (t.Corp == null)
                    unincorporated.Add(t);
            }
            unincorporated.Add(connector);
            return unincorporated;
        }

        /// <summary>
        /// Determine the merging corporations
        /// </summary>
        private void initializeMergingCorporations()
        {
            mergerCorporations = new List<Corporation>();
            foreach (Tile t in adjacentTiles)
            {
                Corporation c = t.Corp;
                if (c != null && !mergerCorporations.Contains(c))    // not null and not already in list
                {
                    int corpSize = c.Size;
                    // insert into sorted list
                    int i = 0;  // determine index to insert at
                    while (i < mergerCorporations.Count)
                    {
                        if (mergerCorporations[i].Size < corpSize)
                            break;
                        i++;
                    }
                    mergerCorporations.Insert(i, c);
                }
            }
        }

        /// <summary>
        /// Tries to handle merging stocks of the next corporation, if there are none then the merging of tiles is done, and merge is ended.
        /// </summary>
        /// <returns>True if there is more to do (iterator)</returns>
        public bool mergeNext()
        {
            if(notifyDefunctStock())
            {
                return true;
            }
            // No more stocks to do
            foreach(Corporation c in mergerCorporations)
            {
                if(c != overtaker)
                    c.mergeInto(overtaker);
            }
            // Gather unincorporated tiles and connector and add them as well
            overtaker.expand(getUnincorporatedTiles());
            return false;
        }

        /// <summary>
        /// Gets the current defunct corp name in a merger.
        /// </summary>
        /// <returns>name of corporation</returns>
        public ECorporation getDefunct()
        {
            return defunct.Name;
        }

        /// <summary>
        /// Gets the overtaking corporation in a merger.
        /// </summary>
        /// <returns>The name</returns>
        public ECorporation getOvertaker()
        {
            return overtaker.Name;
        }

        /// <summary>
        /// Sets the mergemaker for the current merger
        /// </summary>
        /// <param name="player">mergemaker player</param>
        public void setMergeMaker(Player player)
        {
            mergeMaker = player;
        }

        /// <summary>
        /// Gets the mergemaker for the current merger
        /// </summary>
        /// <returns>The mergemaker</returns>
        public Player getMergeMaker()
        {
            return mergeMaker;
        }
    }
}
