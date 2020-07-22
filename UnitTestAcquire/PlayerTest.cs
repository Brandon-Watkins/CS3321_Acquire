using System;
using System.Collections.Generic;
using Acquire;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAcquire
{
    [TestClass]
    public class PlayerTest
    {
        private Player player;
        private Bank bank;
        private Corporation[] corps;

        /// <summary>
        /// Constructor for a few variables
        /// </summary>
        public PlayerTest()
        {
            corps = new Corporation[1];
            Corporation dis = new Corporation(ECorporation.American);
            corps[0] = dis;
            player = new Player("Joe", corps);
            bank = new Bank(corps, 25);
            // Setup by forming a corporation to give stock price a value
            Tile t1 = new Tile("1A");
            Tile t2 = new Tile("2A");
            var tiles = new List<Tile>();
            tiles.Add(t1);
            tiles.Add(t2);
            corps[0].formCorporation(tiles);
        }

        [TestMethod]
        // Tests PlayTile and GiveTile
        public void TestGiveTileAndPlayTile()
        {
            Tile playersTile = new Tile("2A");
            player.giveTile(playersTile);
            Tile tile = player.playTile("2A");
            Assert.AreEqual(playersTile, tile);
            Tile badTile = player.playTile("2A");
            Assert.AreEqual(badTile, null);
        }

        /// <summary>
        /// Tests purchasing a stock, for a player
        /// </summary>
        [TestMethod]
        public void TestPurchaseStock()
        {

            int formerMoney = player.getMoney();
            // Purchase the stock from the bank
            player.purchaseStock(bank, corps[0].Name, 3);

            // Make sure things have adjusted
            Assert.IsTrue(player.getMoney() < formerMoney);
            Assert.AreEqual(player.countStocks(corps[0].Name), 3);
        }

        /// <summary>
        /// Tests selling stock for a player
        /// </summary>
        [TestMethod]
        public void TestSellStock()
        {
            // Add some stock to sell
            player.addStock(corps[0].Name, 5);
            int formerMoney = player.getMoney();

            // Sell 3 of the stock
            bool result = player.sellStock(bank, corps[0].Name, 3);

            // check if things are adjusted
            Assert.IsTrue(result);
            Assert.AreEqual(2, player.countStocks(corps[0].Name));
            Assert.IsTrue(player.getMoney() > formerMoney);

            formerMoney = player.getMoney();

            // Test that they can't sell stock they don't have
            result = player.sellStock(bank, corps[0].Name, 3);
            Assert.IsFalse(result);
            Assert.AreEqual(2, player.countStocks(corps[0].Name));
            Assert.AreEqual(formerMoney, player.getMoney());
        }
    }
}
