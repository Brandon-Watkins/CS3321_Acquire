using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;
using System.Collections.Generic;

namespace UnitTestAcquire
{
    [TestClass]
    public class TileIntersectionFactoryTest
    {
        [TestMethod]
        public void TestDetermineIntersection()
        {
            // Set up intersection for corp foundation
            Tile connector = new Tile("1B");
            List<Tile> adjacentTiles = new List<Tile>();
            Tile adjTile1 = new Tile("1A");
            Tile adjTile2 = new Tile("2B");
            adjacentTiles.Add(adjTile1);
            adjacentTiles.Add(adjTile2);
            TileIntersectionFactory tileIntersectionFactory = new TileIntersectionFactory();
            Acquire.CorpFoundation corpFoundation = new Acquire.CorpFoundation(connector, adjacentTiles);
            // Call method - form corporation
            TileIntersection intersection = tileIntersectionFactory.determineIntersection(connector, adjacentTiles);
            Type t = intersection.GetType();
            // Is it a corporation foundation?
            Assert.AreEqual(t, corpFoundation.GetType());

            // Set up for expansion (single corporation)
            Corporation american = new Corporation(ECorporation.American);
            adjTile2.Corp = american;
            CorpExpansion corpExpansion = new CorpExpansion(connector, adjacentTiles);
            // Call method - expand corporation
            intersection = tileIntersectionFactory.determineIntersection(connector, adjacentTiles);
            // Is it an expansion
            Assert.AreEqual(intersection.GetType(), corpExpansion.GetType());

            // Set up for merger (multi-corporation)
            Corporation sackson = new Corporation(ECorporation.Sackson);
            adjTile1.Corp = sackson;
            Merger merger = new Merger(connector, adjacentTiles);
            // call method - merger
            intersection = tileIntersectionFactory.determineIntersection(connector, adjacentTiles);
            // is it a merger?
            Assert.AreEqual(intersection.GetType(), merger.GetType());
        }
    }
}
