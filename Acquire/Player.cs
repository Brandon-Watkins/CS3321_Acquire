using Acquire.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acquire
{
    /// <summary>
    /// Represents a single player in the Acquire game.
    /// </summary>
    /// <remarks>
    /// Has a hand with up to 6 tiles, and a name.
    /// </remarks>
    public class Player : StockContainer
    {
        #region Member variables
        private List<Tile> hand;
        private string name;
        private int money;
        private int stocksPurchasedPerTurn;

        #endregion
        #region Constructor
        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="name">string: name of player</param>
        /// <param name="corporations">A list of the games corporations</param>
        public Player(string name, Corporation[] corporations) : base(corporations)
        {
            this.name = name;
            hand = new List<Tile>();
            money = 6000;
        }
        #endregion
        #region Member methods
        /// <summary>
        /// Get Name
        /// </summary>
        /// <returns>string</returns>
        public string getName()
        {
            return name;
        }
        
        /// <summary>
        /// Get hand
        /// </summary>
        /// <returns>List of tile objects, representing player's hand.</returns>
        public List<Tile> getHand()
        {
            return hand;
        }
        /// <summary>
        /// Get money
        /// </summary>
        /// <returns></returns>
        public int getMoney()
        {
            return money;
        }
        /// <summary>
        /// Allows game to remove or add money for player
        /// </summary>
        /// <param name="amount"></param>
        public void adjustMoney(int amount)
        {
            money += amount;
        }

        /// <summary>
        /// Gives tile to the player if they have room in their hand.
        /// </summary>
        /// <param name="tile">Tile object</param>
        /// <returns>bool: true if the tile was added to hand</returns>
        public bool giveTile(Tile tile)
        {
            if(hand.Count() < 6)
            {
                hand.Add(tile);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Plays a tile, removing it from the player's hand.
        /// </summary>
        /// <param name="tile">string: the tile position in form "1A"</param>
        /// <returns>Tile object if in hand, otherwise null</returns>
        public Tile playTile(string tile)
        {
            // Reset stocks purchased this turn
            stocksPurchasedPerTurn = 0;
            // Determine which tile is being played
            foreach (Tile t in this.hand) {
                if(t.getPosition() == tile)
                {
                    hand.Remove(t);     // remove the tile from hand
                    return t;
                }
            }
            return null;    // The tile isn't in the hand
        }

        /// <summary>
        /// Sells stock at the current price to the bank
        /// </summary>
        /// <param name="bank">The bank</param>
        /// <param name="corporation">Corporation stock is associated with</param>
        /// <param name="qty">The number of stocks</param>
        /// <returns></returns>
        public bool sellStock(Bank bank, ECorporation corporation, int qty)
        {
            // Check if they have stocks
            if(this.countStocks(corporation) < qty)
            {
                return false;
            }
            int sellPrice = this.stocks[corporation].getStockPrice() * qty;
            adjustMoney(sellPrice);
            // Perform stock transaction
            this.removeStock(corporation, qty);
            bank.addStock(corporation, qty);
            return true;
        }

        /// <summary>
        /// Purchases stock from the bank at the current price.
        /// </summary>
        /// <param name="bank">The bank</param>
        /// <param name="corporation">The corporation</param>
        /// <param name="qty">The amount of stocks</param>
        /// <returns>True if successful</returns>
        public bool purchaseStock(Bank bank, ECorporation corporation, int qty)
        {
            if(stocksPurchasedPerTurn + qty > 3)
            {
                throw new InvalidOperationException("Nope, you can't purchase more than 3 stocks per turn!");
            }
            // Check if we have enough money
            int purchasePrice = this.stocks[corporation].getStockPrice() * qty;
            if(money < purchasePrice)
            {
                return false;
            }
            // Does bank have stock
            if(!bank.removeStock(corporation, qty)) 
            {
                return false;
            }
            adjustMoney(-1 * purchasePrice);
            addStock(corporation, qty);
            stocksPurchasedPerTurn += qty;
            return true;
        }
        #endregion
    }
}
