using System;
using System.Collections.Generic;
using Acquire;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestAcquire
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestGamePlayTile()
        {            
            string[] players = { "Player1", "Player2" };
            Game game = new Game(players);
            List<Tile> hand = game.getCurrentPlayer().getHand();
            string position = hand[0].getPosition();
            game.playTile(position);
            Assert.IsNotNull(game.getTileOnBoard(position));
        }

        [TestMethod]
        public void TestGameNextPlayer()
        {
            string[] players = { "Player1", "Player2"};
            Game game = new Game(players);
            Player player1 = game.getCurrentPlayer();
            game.playTile(player1.getHand()[0].getPosition());
            game.nextPlayer();
            Player player2 = game.getCurrentPlayer();
            Assert.AreEqual(player1.getName(), players[0]);
            Assert.AreEqual(player2.getName(), players[1]);
            Assert.AreEqual(game.getCurrentPlayer(), player2);
        }
        [TestMethod]
        public void TestDrawPile()
        {
            string[] players = { "Player1", "Player2" };
            Game game = new Game(players);
            List<Tile> drawPile = game.DrawPile;
            Console.Write("The draw pile contains " + drawPile.Count + " tiles\n\n\n");
            foreach (Tile dTile in drawPile)
            {
                Console.Write(dTile.getPosition() + " ");
            }
            Console.Write("\n");
            int index = 0;  // Only play one tile, since it could get stuck trying to form a corporation
            foreach (string x in players)
            {
                Player playerThis = game.getCurrentPlayer();
                game.drawTile();
                List<Tile> tileToPlay = playerThis.getHand();
                foreach (Tile c in tileToPlay)
                {
                    Assert.IsFalse(drawPile.Contains(c));
                    Console.Write(c.getPosition() + " ");
                }
                Console.Write("\n");
                if (index == 0)
                {
                    game.playTile(tileToPlay[0].getPosition());
                    game.nextPlayer();
                }
                index++;
            }
            foreach (Tile dTile in drawPile)
            {
                Console.Write(dTile.getPosition() + " ");
            }
            Console.Write("\n\n\nThe draw pile now contains " + drawPile.Count + " tiles");
        }
    }
}
