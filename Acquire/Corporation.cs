using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Acquire.interfaces;

namespace Acquire
{
    /// <summary>
    /// An enumeration of the seven valid corporations
    /// </summary>
    public enum ECorporation
    {
        /// <summary>
        /// Worldwide Corporation
        /// </summary>
        WorldWide,
        /// <summary>
        /// Sackson Corporation
        /// </summary>
        Sackson,
        /// <summary>
        /// Festival Corporation
        /// </summary>
        Festival,
        /// <summary>
        /// Imperial Corporation
        /// </summary>
        Imperial,
        /// <summary>
        /// American Corporation
        /// </summary>
        American,
        /// <summary>
        /// Continental Corporation
        /// </summary>
        Continental,
        /// <summary>
        /// Tower Corporation
        /// </summary>
        Tower
    }

    /// <summary>
    /// Represents a corporation and it's associated tiles.
    /// </summary>
    public class Corporation : StockCorporation
    {
        private ECorporation name;
        private List<Tile> tiles;
        private bool active;
        private int initialValue;
        private StockCorpHandler handler;

        /// <summary>
        /// constructor, sets the name of the corporation, active status to false, and creates an empty List for Tiles.
        /// </summary>
        /// <param name="nameOfCorporation">(string) the name of the corporation</param>
        /// <param name="s"></param>
        public Corporation(ECorporation nameOfCorporation, int s = 0)
        {
            this.name = nameOfCorporation;
            this.active = false;
            this.tiles = new List<Tile>();
           
            initialValue = s;
        }

      
        /// <summary>
        /// Name property, to get the name of corporation.
        /// </summary>
        public ECorporation Name { get => name; }

        /// <summary>
        /// Tiles property, to get the list of tiles associated with corporation.
        /// </summary>
        public List<Tile> Tiles { get => tiles; }
        /// <summary>
        /// Active property, to get whether the corporation is active.
        /// </summary>
        public bool Active { get => active; }

        /// <summary>
        /// returns true if corp tile count is  > 11
        /// </summary>
        public bool IsSafe { get => tiles.Count() >= 11; }
        /// <summary>
        /// Size property, to get size of corporation.
        /// </summary>
        public int Size { get => tiles.Count(); }
        /// <summary>
        /// returns the stock price based off of tile count and initialValue
        /// </summary>
        public int StockPrice
        {
            get
            {
                int activeNumber = tiles.Count();


                if (activeNumber == 2) { return 200 + initialValue; }
                else if (activeNumber == 3) { return 300 + initialValue; }
                else if (activeNumber == 4) { return 400 + initialValue; }
                else if (activeNumber == 5) { return 500 + initialValue; }
                else if (activeNumber >= 6 && activeNumber <= 10) { return 600 + initialValue; }
                else if (activeNumber >= 11 && activeNumber <= 20) { return 700 + initialValue; }
                else if (activeNumber >= 21 && activeNumber <= 30) { return 800 + initialValue; }
                else if (activeNumber >= 31 && activeNumber <= 40) { return 900 + initialValue; }
                else if (activeNumber >= 41) { return 1000 + initialValue; }
                else { return 0; }
            }
        }

        /// <summary>
        /// Forms a corporation, updating the owning corporation for both the tiles and corporations, 
        /// and sets the corporation as active.
        /// </summary>
        /// <param name="tilesToAdd">(List of Tile objects) The list of newly connected tiles that are forming the corporation</param>
        /// <exception cref="InvalidOperationException">Throws exception if the corporation is already formed.</exception>
        public void formCorporation(List<Tile> tilesToAdd)
        {
            if (this.active)
                throw new InvalidOperationException("Unable to form this corporation! Already Formed!");
            for (int i = 0; i < tilesToAdd.Count(); i++)
            {
                // tells the tiles which corporation they belong to.
                tilesToAdd.ElementAt(i).setCorporation(this);// make sure this is working properly
                // tells the corporation which tiles it owns.
                this.tiles.Add(tilesToAdd.ElementAt(i));
            }
            this.active = true;
            if(handler != null)
                handler.handleGiveFreeStock(this.name); // Give free stock
        }

        /// <summary>
        /// Performs a merger where this corporation goes defunct and is overtaken by overtaker
        /// </summary>
        /// <param name="overtaker">The overtaking corporation</param>
        public void mergeInto(Corporation overtaker)
        {
            Assert.IsTrue(overtaker.active);
            overtaker.expand(this.tiles); // Adds corporations tiles to overtaker
            // Reset corporation to be inactive
            this.active = false;
            this.tiles = new List<Tile>();
        }

        /// <summary>
        /// Performs an expansion of the corporation, increasing size.
        /// </summary>
        /// <param name="tiles">list of tiles to add on to the corporation</param>
        public void expand(List<Tile> tiles)
        {
            Assert.IsTrue(active);
            // fix up the tile references
            foreach (Tile t in tiles)
            {
                t.Corp = this;
                this.tiles.Add(t);
            }
        }

        /// <summary>
        /// Adds the handler for dealing with stocks
        /// </summary>
        /// <param name="handler">Stock Corporation handler</param>
        public void addHandler(StockCorpHandler handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// Notifies that the corporation has gone defunct to the stock handler.
        /// </summary>
        /// <param name="merger">Merger being dealt with</param>
        public void notifyDefunct(IMerger merger)
        {
            if(handler != null) 
                handler.handleStocks(merger);
        }
    }
}
