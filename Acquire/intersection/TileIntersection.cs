using Acquire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Represents a tile intersection on the board when a tile is placed.
    /// </summary>
    public abstract class TileIntersection : ChooseCorpEvent
    {
        /// <summary>
        /// Connector is the tile played, at the center of the tile intersection.
        /// </summary>
        protected Tile connector;
        /// <summary>
        /// adjacentTiles are the list of tiles surrounding the connector tile.
        /// </summary>
        protected List<Tile> adjacentTiles;
        /// <summary>
        /// Handler for choosing a corporation
        /// </summary>
        protected ChooseCorpHandler handler;

        /// <summary>
        /// Constructor, sets the connecting tile, and its adjacent tiles.
        /// </summary>
        /// <param name="adjTiles">(List of Tile objects) List of the adjacent tiles</param>
        /// <param name="connector">(Tile) The tile being placed, center to adjacent tiles</param>
        public TileIntersection(Tile connector, List<Tile> adjTiles)
        {
            this.connector = connector;
            this.adjacentTiles = adjTiles;
        }


        /// <summary>
        /// Chooses the corporation from the valid choices
        /// </summary>
        /// <param name="corporation">One of the valid corporation enumerations</param>
        public abstract void chooseCorporation(ECorporation corporation);
        /// <summary>
        /// Gets the valid corporation choices.
        /// </summary>
        /// <returns>List of corporation enumerations</returns>
        public abstract List<ECorporation> getValidCorporationChoices();

        /// <summary>
        /// Abstract method to resolve the tile intersection.
        /// </summary>
        /// <param name="corporations">The array of all corporations</param>
        abstract public bool resolveIntersection(Corporation[] corporations);

        /// <summary>
        /// Sets the handler for choosing a corporation.
        /// </summary>
        /// <param name="handler">Object that implements the ChooseCorpHandler interface</param>
        public void setHandler(ChooseCorpHandler handler)
        {
            this.handler = handler;
        }

    }
}