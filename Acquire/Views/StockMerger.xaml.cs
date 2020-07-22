using Acquire.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Acquire.Views
{
    /// <summary>
    /// Interaction logic for HandleStocks.xaml
    /// </summary>
    public partial class StockMerger : Window
    {
        /// <summary>
        /// Represents how many defunct stocks are exchanged for overtaker stocks.
        /// </summary>
        const int EXCHANGE_RATE = 2;

        /// <summary>
        /// Represents the current instance of the game.
        /// </summary>
        private GameModel model;

        /// <summary>
        /// Stores the style class for the game.
        /// </summary>
        private AquireStyle aquireStyle;

        /// <summary>
        /// Constructor for the stock Merger window
        /// </summary>
        public StockMerger(GameModel model)
        {
            InitializeComponent();

            this.model = model;
            aquireStyle = new AquireStyle();
            ECorporation defunct = model.getDefunctCorporation();
            ECorporation overtaker = model.getOvertaker();
            int inHand = model.getCurrentPlayerStocks(defunct);
            int overtakerInBank = model.getBankStocks(overtaker);
            int maximum = (int)inHand / 2;

            // Get player info.
            grp_PlayerInfo.Header = model.getCurrentPlayerName();
            tb_PlayerMoney.Text = "$" + model.getCurrentPlayerMoney();

            // Get defunct corporation and overtaker corporation.
            lbl_merger.Content = "Merging " + defunct.ToString() + " into " + overtaker.ToString();

            // Set defunct label values.
            lbl_defunct.Content = defunct.ToString();
            lbl_defunct_inBank.Content = model.getBankStocks(defunct);
            lbl_defunct_inHand.Content = inHand;
            lbl_defunct_price.Content = "$" + model.getStockValue(defunct).ToString();
            brd_DefunctBorder.BorderBrush = aquireStyle.GetActiveCorpColor(defunct);

            // Set overtaker label values.
            lbl_overtaker.Content = overtaker.ToString();
            lbl_overtaker_inBank.Content = overtakerInBank;
            lbl_overtaker_inHand.Content = model.getCurrentPlayerStocks(overtaker);
            lbl_overtaker_price.Content = "$" + model.getStockValue(overtaker).ToString();
            brd_OvertakerBorder.BorderBrush = aquireStyle.GetActiveCorpColor(overtaker);

            // Set slider maximum values.
            slider_defunct_sell.Maximum = inHand;
            slider_defunct_trade.Maximum = (maximum < overtakerInBank ? maximum : overtakerInBank);
        }

        /// <summary>
        /// Allows the value of the quantity of defunct and overtaker stocks
        /// to trade to vary with the slider.
        /// </summary>
        private void slider_defunct_trade_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int val = (int)e.NewValue;
            lbl_defunct_trade.Content = val * 2;
            lbl_overtaker_trade.Content = val;
        }

        /// <summary>
        /// Allows the value of the quantity of defunct and overtaker stocks
        /// to sell to vary with the slider.
        /// </summary>
        private void slider_defunct_sell_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int val = (int)e.NewValue;
            lbl_defunct_sell.Content = val;

            // Update the sell price
            string stockPrice = ((string) lbl_defunct_price.Content).Trim('$');
            lbl_defunct_sellPrice.Content = "$" + (val * int.Parse(stockPrice)).ToString();
        }

        /// <summary>
        /// Allows the <see cref="Player"/> to cancel trading or selling to hold their stocks.
        /// </summary>
        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to hold your stocks?", "Confirm", MessageBoxButton.YesNo)
                == MessageBoxResult.No)
                return;
            model.holdStocks();
            this.Close();
        }

        /// <summary>
        /// Allows the <see cref="Player"/> to submit the quantities of stocks to trade and sell.
        /// </summary>
        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            // Check if valid
            int defunctStocksToTrade = (int)slider_defunct_trade.Value;
            int defunctStocksToSell = (int)slider_defunct_sell.Value;
            int totalStocks = (defunctStocksToTrade * EXCHANGE_RATE) + defunctStocksToSell;

            if (TotalIsGreaterThanStockMax(totalStocks))
            {
                MessageBox.Show("You don't have that many stocks!");
                return;
            }

            // Perform Trade/Sell
            if (defunctStocksToTrade > 0)
                model.tradeStocks(defunctStocksToTrade);

            if (defunctStocksToSell > 0)
                model.sellStocks(defunctStocksToSell);

            model.holdStocks();
            this.Close();
        }

        /// <summary>
        /// Determines whether the number of stocks to trade or sell is greater
        /// than the number of owned stocks.
        /// </summary>
        /// <param name="totalStocks">The number of stocks to compare.</param>
        /// <returns>Whether the number of stocks is greater than the 
        /// defunct stock slider's maximum.</returns>
        private bool TotalIsGreaterThanStockMax(int totalStocks)
        {
            return totalStocks > slider_defunct_sell.Maximum;
        }
    }
}
