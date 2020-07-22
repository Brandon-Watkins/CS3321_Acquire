using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Acquire.interfaces;

namespace Acquire.Views
{
    /// <summary>
    /// Interaction logic for BuyStocks.xaml
    /// </summary>
    public partial class BuyStocks : Window
    {
        /// <summary>
        /// Represents the current game model.
        /// </summary>
        private GameModel model;

        /// <summary>
        /// Associates corporation sliders with the members of <see cref="ECorporation"/>.
        /// </summary>
        private Dictionary<Slider, ECorporation> corporationSliders;

        /// <summary>
        /// Associates corporation sliders with their quantity labels.
        /// </summary>
        private Dictionary<Slider, Label> sliderQtys;

        /// <summary>
        /// Associates corporation sliders with their price labels.
        /// </summary>
        private Dictionary<Slider, Label> sliderPrices;

        /// <summary>
        /// Assosiates stock quantity labels with members of <see cref="ECorporation"/>.
        /// </summary>
        private Dictionary<ECorporation, Label> playerStocks;

        /// <summary>
        /// Represents the number of purchased stocks.
        /// </summary>
        private int numStocksPurchased;

        /// <summary>
        /// Buy Stocks Window Constructor
        /// </summary>
        public BuyStocks(GameModel gameModel)
        {
            InitializeComponent();
            numStocksPurchased = 0;
            model = gameModel;

            InitializeCorporationSliders();
            InitializeSliderPrices();
            InitializePlayerStocks();
            InitializeSliderQuantities();
            InitializePlayerInfo();
            InitializeSliders();
        }

        /// <summary>
        /// Displays the info for the current <see cref="Player"/>.
        /// </summary>
        private void InitializePlayerInfo()
        {
            grp_PlayerInfo.Header = model.getCurrentPlayerName();
            lbl_PlayerMoney.Content = "$" + model.getCurrentPlayerMoney().ToString();
        }

        /// <summary>
        /// Defines the associations between corporation sliders and the
        /// stock quantity labels.
        /// </summary>
        private void InitializeSliderQuantities()
        {
            sliderQtys = new Dictionary<Slider, Label>{
                { slider_Corp_American, qty_Corp_American },
                { slider_Corp_Imperial, qty_Corp_Imperial },
                { slider_Corp_Continental, qty_Corp_Continental },
                { slider_Corp_Festival, qty_Corp_Festival },
                { slider_Corp_Worldwide, qty_Corp_Worldwide },
                { slider_Corp_Sackson, qty_Corp_Sackson },
                { slider_Corp_Tower, qty_Corp_Tower }
            };
        }

        /// <summary>
        /// Defines the associations between quantity labels and the members 
        /// of <see cref="ECorporation"/>.
        /// </summary>
        private void InitializePlayerStocks()
        {
            playerStocks = new Dictionary<ECorporation, Label>{
                { ECorporation.American, Content_Qty_American },
                { ECorporation.Imperial, Content_Qty_Imperial },
                { ECorporation.Continental, Content_Qty_Continental },
                { ECorporation.Festival, Content_Qty_Festival },
                { ECorporation.WorldWide, Content_Qty_Worldwide },
                { ECorporation.Sackson, Content_Qty_Sackson },
                { ECorporation.Tower, Content_Qty_Tower }
            };
        }

        /// <summary>
        /// Defines the associations between the corporation sliders
        /// and the price labels.
        /// </summary>
        private void InitializeSliderPrices()
        {
            sliderPrices = new Dictionary<Slider, Label>{
                { slider_Corp_American, Content_Price_American },
                { slider_Corp_Imperial, Content_Price_Imperial },
                { slider_Corp_Continental, Content_Price_Continental },
                { slider_Corp_Festival, Content_Price_Festival },
                { slider_Corp_Worldwide, Content_Price_Worldwide },
                { slider_Corp_Sackson, Content_Price_Sackson },
                { slider_Corp_Tower, Content_Price_Tower }
            };
        }

        /// <summary>
        /// Defines the associations between the corporation sliders and
        /// the members of <see cref="ECorporation"/>.
        /// </summary>
        private void InitializeCorporationSliders()
        {
            corporationSliders = new Dictionary<Slider, ECorporation>{
                { slider_Corp_American, ECorporation.American },
                { slider_Corp_Imperial, ECorporation.Imperial },
                { slider_Corp_Continental, ECorporation.Continental },
                { slider_Corp_Festival, ECorporation.Festival },
                { slider_Corp_Worldwide, ECorporation.WorldWide },
                { slider_Corp_Sackson, ECorporation.Sackson },
                { slider_Corp_Tower, ECorporation.Tower }
            };
        }

        /// <summary>
        /// Initializes sliders to represent the stock buying options for the <see cref="Player"/>.
        /// </summary>
        private void InitializeSliders()
        {
            // Set up sliders to match bank and corporations
            foreach (KeyValuePair<Slider, ECorporation> corpSlider in corporationSliders)
            {
                playerStocks[corpSlider.Value].Content = model.getCurrentPlayerStocks(corpSlider.Value);

                // Only initialize the sliders of corporations that are active.
                if (model.isActiveCorporation(corpSlider.Value))
                {
                    // Are there stocks in the bank?
                    int stockQty = model.getBankStocks(corpSlider.Value);

                    // Set the slider maximum to the number of stocks
                    // of that type in the bank.
                    if (stockQty < 3)
                    {
                        corpSlider.Key.Maximum = stockQty;
                    }

                    // If there are no stocks of a type in the bank,
                    // disable the slider.
                    if (stockQty == 0)
                    {
                        corpSlider.Key.IsEnabled = false;
                    } 
                    else
                    {
                        corpSlider.Key.Background = Brushes.Aqua;
                    }

                    // Fill in current prices
                    sliderPrices[corpSlider.Key].Content = "$" + model.getStockValue(corpSlider.Value).ToString();
                }
                else  // If the corporation is inactive, disable the slider and it's price.
                {
                    corpSlider.Key.IsEnabled = false;
                    sliderPrices[corpSlider.Key].Content = "N/A";
                }
            }

            lbl_PurchaseTotal.Content = "$0";
        }

        /// <summary>
        /// Calculates the total purchase price of buying stocks.
        /// </summary>
        /// <returns>A string representing the price as money.</returns>
        private int CalculatePurchaseTotal()
        {
            int purchaseTotal = 0;

            foreach (KeyValuePair<Slider, ECorporation> corpSlider in corporationSliders)
            {
                Slider slider = corpSlider.Key;
                ECorporation corporation = corpSlider.Value;

                if (model.isActiveCorporation(corporation))
                {
                    int purchaseQty = (int)slider.Value;
                    int purchasePrice = model.getStockValue(corporation);

                    purchaseTotal += purchaseQty * purchasePrice;
                }
            }

            return purchaseTotal;
        }

        /// <summary>
        /// Allows the labels representing the slider quantities to change with the slider.
        /// </summary>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update associated Label to match slider value
            int qty = (int)e.NewValue;
            Slider slider = ((Slider)sender);
            sliderQtys[slider].Content = qty.ToString(); 

            // Update purchase label.
            int purchasePrice = CalculatePurchaseTotal();
            lbl_PurchaseTotal.Content = $"${ purchasePrice }";

            // Update player money label.
            lbl_PlayerMoney.Content = $"${ model.getCurrentPlayerMoney() - purchasePrice }";
        }

        /// <summary>
        /// Attempts to purchase
        /// </summary>
        private void btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            // Check the total stocks and price
            int playerMoney = model.getCurrentPlayerMoney();
            int totalStocks = 0;
            int totalPrice = 0;

            // Update the number of stocks and their prices.
            foreach(KeyValuePair<Slider, ECorporation> valuePair in corporationSliders)
            {
                int qty = (int)valuePair.Key.Value;
                totalStocks += qty;
                totalPrice += model.getStockValue(valuePair.Value) * qty;
            }

            // Don't allow the player to buy more than three stocks.
            if(totalStocks + numStocksPurchased > 3)
            {
                // Unable to purchase more than 3 per turn
                MessageBox.Show("Unable to purchase more than 3 stocks per turn!");
                return;
            }

            // Don't allow the player to buy more stocks than they can afford.
            if(playerMoney < totalPrice)
            {
                // Not enough money
                MessageBox.Show("Insufficient Funds!\nStocks cost: " + totalPrice + "\nYou only have: " + playerMoney);
                return;
            }

            // Purchase the stocks
            foreach (KeyValuePair<Slider, ECorporation> valuePair in corporationSliders)
            {
                int qty = (int)valuePair.Key.Value;
                if (qty > 0)
                {
                    try
                    {
                        model.purchaseStocks(valuePair.Value, qty);
                        numStocksPurchased += qty;
                    } 
                    catch
                    {
                        MessageBox.Show("Unable to purchase stocks");
                    }
                }
            }

            this.Close();
        }

        /// <summary>
        /// Allows the user to cancel buying stocks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
