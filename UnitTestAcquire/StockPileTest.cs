using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Acquire;

namespace UnitTestAcquire
{
    [TestClass]
    public class StockPileTest
    {
        /// <summary>
        /// Tests adding and removing from a stock pile
        /// </summary>
        [TestMethod]
        public void TestAddRemove()
        {
            Corporation corporation = new Corporation(ECorporation.WorldWide);
            StockPile pile = new StockPile(corporation, 3); // Start with 3
            pile.add(3);
            Assert.AreEqual(6, pile.NumStocks);
            Assert.IsFalse(pile.remove(7));
            pile.remove(6);
            Assert.AreEqual(0, pile.NumStocks);
        }
    }
}
