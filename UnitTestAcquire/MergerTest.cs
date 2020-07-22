using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;
using Acquire.interfaces;
using System.Collections.Generic;

namespace UnitTestAcquire
{
    [TestClass]
    public class MergerTest : ChooseCorpHandler, StockCorpHandler
    {
        private ChooseCorpEvent corpEvent;

        /// <summary>
        /// Test's resolving the intersection of a merger between 3 unequal-sized corporations.
        /// </summary>
        [TestMethod]
        public void TestResolveIntersection()
        {
            // Test unequal size merger between 3 corporations
            // Setup tiles and corporations
            Corporation american = new Corporation(ECorporation.American);
            american.addHandler(this);
            Corporation sackson = new Corporation(ECorporation.Sackson);
            sackson.addHandler(this);
            Corporation worldWide = new Corporation(ECorporation.WorldWide);
            worldWide.addHandler(this);
            Corporation [] corporations = { american, sackson, worldWide };
            
            // Form American
            var tiles = new List<Tile>();
            Tile a1 = new Tile("1A");
            Tile a2 = new Tile("2A");
            tiles.Add(a1);
            tiles.Add(a2);
            american.formCorporation(tiles); // dependency on formCorporation

            // Form Sackson
            tiles = new List<Tile>();
            Tile b3 = new Tile("3B");
            Tile c3 = new Tile("3C");
            Tile d3 = new Tile("3D");
            tiles.Add(b3);
            tiles.Add(c3);
            tiles.Add(d3);
            sackson.formCorporation(tiles);

            // Form WorldWide
            tiles = new List<Tile>();
            Tile a4 = new Tile("4A");
            Tile a5 = new Tile("5A");
            Tile a6 = new Tile("6A");
            Tile a7 = new Tile("7A");
            tiles.Add(a4);
            tiles.Add(a5);
            tiles.Add(a6);
            tiles.Add(a7);
            worldWide.formCorporation(tiles);

            // Connector
            Tile connector = new Tile("3A");
            List<Tile> adjacentTiles = new List<Tile>(); // Adjacent Tiles
            adjacentTiles.Add(a4);
            adjacentTiles.Add(a2);
            adjacentTiles.Add(b3);

            Merger merger = new Merger(connector, adjacentTiles);
            // Finally, test merge
            merger.resolveIntersection(corporations); // relies on corporation.mergeInto()
            // Ensure that the merge was correct
            Assert.IsTrue(worldWide.Active);
            Assert.IsFalse(american.Active);
            Assert.IsFalse(sackson.Active);
            Assert.AreEqual(10, worldWide.Size);
        }

        /// <summary>
        /// Tests merging two equal-sized corporations and choosing the overtaker.
        /// </summary>
        [TestMethod]
        public void TestChooseCorporation()
        {
            // Setup tiles and corporations
            Corporation american = new Corporation(ECorporation.American);
            american.addHandler(this);
            Corporation sackson = new Corporation(ECorporation.Sackson);
            sackson.addHandler(this);
            Corporation[] corporations = { american, sackson };

            // Form American
            var tiles = new List<Tile>();
            Tile a1 = new Tile("1A");
            Tile a2 = new Tile("2A");
            tiles.Add(a1);
            tiles.Add(a2);
            american.formCorporation(tiles); // dependency on formCorporation

            // Form Sackson
            tiles = new List<Tile>();
            Tile b3 = new Tile("3B");
            Tile c3 = new Tile("3C");
            tiles.Add(b3);
            tiles.Add(c3);
            sackson.formCorporation(tiles);

            // Connector
            Tile connector = new Tile("3A");
            List<Tile> adjacentTiles = new List<Tile>(); // Adjacent Tiles
            adjacentTiles.Add(a2);
            adjacentTiles.Add(b3);
            // Start Merger
            Merger merger = new Merger(connector, adjacentTiles);
            corpEvent = merger;
            merger.setHandler(this);
            merger.resolveIntersection(corporations);

            // Check that it chose American (see handleChooseCorp)
            Assert.IsTrue(american.Active);
            Assert.IsFalse(sackson.Active);
        }

        /// <summary>
        /// Handles choosing the corporation when 2 equal-sized corporations are merged.
        /// </summary>
        /// <param name="validCorps">The valid list of corporations</param>
        public void handleChooseCorp(List<ECorporation> validCorps)
        {
            corpEvent.chooseCorporation(ECorporation.American);
        }

        public void handleStocks(IMerger merger)
        {
            merger.mergeNext();
        }

        public void handleGiveFreeStock(ECorporation corporation)
        {
            // Do nothing
        }

        public void sellStocks(int quantity)
        {
            throw new NotImplementedException();
        }

        public void tradeStocks(int quantity)
        {
            throw new NotImplementedException();
        }

        public void holdStocks()
        {
            throw new NotImplementedException();
        }

        public ECorporation getDefunctCorporation()
        {
            throw new NotImplementedException();
        }

        public ECorporation getOvertaker()
        {
            throw new NotImplementedException();
        }
    }
}
