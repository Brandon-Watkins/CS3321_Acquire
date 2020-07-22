using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;
using System.Collections.Generic;

namespace UnitTestAcquire
{
    [TestClass]
    public class CorporationTest
    {
        [TestMethod]
        public void TestFormCorporation()
        {
            Corporation corporation = new Corporation(ECorporation.Continental);
            // Set-up some tiles
            var tiles = new List<Tile>();
            Tile a4 = new Tile("4A");
            Tile a5 = new Tile("5A");
            Tile a6 = new Tile("6A");
            Tile a7 = new Tile("7A");
            tiles.Add(a4);
            tiles.Add(a5);
            tiles.Add(a6);
            tiles.Add(a7);

            // Test method
            corporation.formCorporation(tiles);
            Assert.AreEqual(4, corporation.Size);
        }
    }
}
