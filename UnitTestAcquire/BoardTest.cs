using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;

namespace UnitTestAcquire
{

    [TestClass]
    public class BoardTest
    {
        [TestMethod]
        public void TestBoardPlayTile()
        {
            Board board = new Board();
            Tile tile = new Tile("10I");
            board.playTile(tile);
            Tile tileRef = board.getTileAt("10I");
            Assert.AreEqual(tile, tileRef);
        }
    }
}
