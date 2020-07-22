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
    /// Interaction logic for PlayerInfo.xaml
    /// </summary>
    public partial class PlayerInfo : Window
    {
        /// <summary>
        /// Represents the number of players chosen for the game.
        /// </summary>
        private int numOfPlayers;

        /// <summary>
        /// Represents the names of the players.
        /// </summary>
        private List<string> playerNames;

        /// <summary>
        /// Contains all the name input fields.
        /// </summary>
        List<TextBox> inputFields;

        /// <summary>
        /// PlayerInfo Constructor, initializes the window to get player names.
        /// </summary>
        /// <param name="numOfPlayers">(int) Number of players</param>
        public PlayerInfo(int numOfPlayers)
        {
            InitializeComponent();

            InitializeControls();

            this.numOfPlayers = numOfPlayers;
            InitializeTextBoxes();
        }

        /// <summary>
        /// Initializes all controls for the <see cref="PlayerInfo"/> view.
        /// </summary>
        private void InitializeControls()
        {
            playerNames = new List<string>();

            inputFields = new List<TextBox>
            {
                // Add each text box to a list so they can be operated on in order.
                txt_Player1Name,
                txt_Player2Name,
                txt_Player3Name,
                txt_Player4Name,
                txt_Player5Name,
                txt_Player6Name
            };

            // Deactivate all text boxes.
            foreach (TextBox inputField in inputFields)
            {
                inputField.IsEnabled = false;
            }
        }

        /// <summary>
        /// Activate or deactivate text boxes based on the number of players.
        /// </summary>
        private void InitializeTextBoxes()
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                TextBox currentTextBox = inputFields[i];

                currentTextBox.IsEnabled = true;
            }
        }

        /// <summary>
        /// Allows the user to confirm the names of the players.
        /// </summary>
        private void btn_Confirm_Click(object sender, RoutedEventArgs e)
        {
            CollectPlayerNames();

            GameView gameView = new GameView(playerNames);
            gameView.Show();
            this.Close();
        }

        /// <summary>
        /// Allows the user to return the player number select view.
        /// </summary>
        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {
            PlayerNumber playerNumber = new PlayerNumber();
            playerNumber.Show();
            this.Close();
        }

        /// <summary>
        /// Adds all player names to a list of player names.
        /// </summary>
        private void CollectPlayerNames()
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                string playerName = inputFields[i].Text;

                playerNames.Add(playerName);
            }
        }
    }
}
