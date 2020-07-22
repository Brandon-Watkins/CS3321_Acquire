using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;

namespace UnitTestAcquire
{
    [TestClass]
    public class TileTest
    {
        /*******************************************************
         *  Test's Tile Constructor and isValidPosition method
         *      Expects an exception from invalid position.
         *******************************************************/
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),
    "321 was allowed as a Tile position")]
        public void TestTile()
        {
            string position = "1A";
            Tile t = new Tile(position);
            Assert.AreEqual(position, t.getPosition());
            position = "A12"; // invalid position
            t = new Tile(position);
        }

        public void TestGetColumn()
        {
            string position = "12G";
            Tile t = new Tile(position);
            Assert.AreEqual(t.getColumn(), 12);
        }

        public void TestGetRow()
        {
            string position = "10B";
            Tile t = new Tile(position);
            Assert.AreEqual(t.getRow(), 'B');
        }
    }
}
