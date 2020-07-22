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

namespace Acquire
{
    /// <summary>
    /// Interaction logic for PlayerNumber.xaml
    /// </summary>
    public partial class PlayerNumber : Window
    {
        /// <summary>
        /// Defines the default number of players in the game.
        /// </summary>
        const int DEFAULT_NUM_OF_PLAYERS = 2;

        /// <summary>
        /// The number of players to play the game.
        /// </summary>
        private int numOfPlayers = DEFAULT_NUM_OF_PLAYERS;

        /// <summary>
        /// Creates a new instance of the <see cref="PlayerNumber"/> window class.
        /// </summary>
        public PlayerNumber()
        {
            InitializeComponent();
            InititalizeControls();
        }

        /// <summary>
        /// Initializes all controls of the <see cref="PlayerNumber"/> view.
        /// </summary>
        private void InititalizeControls()
        {
            lbl_NumOfPlayers.Content = numOfPlayers.ToString();
        }

        /// <summary>
        /// Allows the value for the number of players to change with the slider.
        /// </summary>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            numOfPlayers = (int)e.NewValue;
            lbl_NumOfPlayers.Content = numOfPlayers.ToString();
        }

        /// <summary>
        /// Allows the user to confirm the number of players.
        /// </summary>
        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            PlayerInfo playerInfo = new PlayerInfo(numOfPlayers);
            playerInfo.Show();
            this.Close();
        }

        /// <summary>
        /// Allows the player to go back to the main menu.
        /// </summary>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            IntroScreen introScreen = new IntroScreen();
            introScreen.Show();
            this.Close();
        }
    }
}
