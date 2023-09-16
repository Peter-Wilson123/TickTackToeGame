using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TickTackToeGame
{
    public partial class Form1 : Form
    {
        private char currentPlayer = 'X'; // 'X' starts the game
        private bool gameEnded = false;
        private Button[,] buttons;
        static string Difficulty = "Hard";

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[3, 3] {
                { button1, button2, button3 },
                { button4, button5, button6 },
                { button7, button8, button9 }
            };

            foreach (Button button in buttons)
            {
                button.Text = "";
                button.Enabled = true;

                // Remove the event handler if it's already attached
                button.Click -= Button_Click;

                // Add the event handler back
                button.Click += Button_Click;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (button.Text == "" && !gameEnded)
            {
                button.Text = currentPlayer.ToString();
                button.Enabled = false;
                CheckForWinner();
                currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';

                if (currentPlayer == 'O' && !gameEnded)
                {
                    // Computer's turn to make a random move
                    ComputerMove();
                }
            }
        }

        private void ComputerMove()
        {
            List<Button> availableButtons = new List<Button>();
            char opponent = (currentPlayer == 'X') ? 'O' : 'X'; // Determine the opponent's symbol

            // Check if the computer can win on the next move
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.Text = "O";
                    if (CheckForWin("O") && Difficulty == "Hard")
                    {
                        button.Enabled = false;
                        currentPlayer = 'X'; // Switch back to the player
                        return;
                    }
                    button.Text = ""; // Reset the button
                }
            }

            // Check if the player can win on the next move and block them
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.Text = "X";
                    if (CheckForWin("X") && Difficulty == "Hard")
                    {
                        button.Text = "O";
                        button.Enabled = false;
                        currentPlayer = 'X'; // Switch back to the player
                        return;
                    }
                    button.Text = ""; // Reset the button
                }
            }

            // Check if the player is about to win and block them
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    button.Text = "X"; // Simulate player's move
                    if (IsPlayerAboutToWin('X') && Difficulty == "Hard")
                    {
                        button.Text = "O";
                        button.Enabled = false;
                        currentPlayer = 'X'; // Switch back to the player
                        return;
                    }
                    button.Text = ""; // Reset the button
                }
            }

            // If no winning or blocking moves, make a random move
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    availableButtons.Add(button);
                }
            }

            if (availableButtons.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(availableButtons.Count);
                Button computerChoice = availableButtons[index];
                computerChoice.Text = "O";
                computerChoice.Enabled = false;
                currentPlayer = 'X'; // Switch back to the player
            }
        }

        private void CheckForWinner()
        {
            // Check for a win condition here
            // You need to implement the logic for checking rows, columns, and diagonals.
            // If a win condition is met, set gameEnded to true and show a message box.
            for (int row = 0; row < 3; row++)
            {
                if (buttons[row, 0].Text != "" && buttons[row, 0].Text == buttons[row, 1].Text && buttons[row, 1].Text == buttons[row, 2].Text)
                {
                    EndGame(buttons[row, 0].Text);
                    return;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (buttons[0, col].Text != "" && buttons[0, col].Text == buttons[1, col].Text && buttons[1, col].Text == buttons[2, col].Text)
                {
                    EndGame(buttons[0, col].Text);
                    return;
                }
            }

            // Check diagonals
            if (buttons[0, 0].Text != "" && buttons[0, 0].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 2].Text)
            {
                EndGame(buttons[0, 0].Text);
                return;
            }

            if (buttons[0, 2].Text != "" && buttons[0, 2].Text == buttons[1, 1].Text && buttons[1, 1].Text == buttons[2, 0].Text)
            {
                EndGame(buttons[0, 2].Text);
                return;
            }

            // Check for a tie (all buttons filled)
            bool isTie = true;
            foreach (Button button in buttons)
            {
                if (button.Text == "")
                {
                    isTie = false;
                    break;
                }
            }

            if (isTie && !gameEnded) // Add !gameEnded to ensure the game hasn't already ended in a win
            {
                EndGame("Tie");
            }
        }
        private void EndGame(string winner)
        {
            gameEnded = true;

            if (winner == "Tie")
            {
                MessageBox.Show("It's a tie!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(winner + " wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Disable buttons after the game ends
            foreach (Button button in buttons)
            {
                button.Enabled = false;
            }

            // Call the ResetGame method to start a new game
        }

        private bool IsPlayerAboutToWin(char playerSymbol)
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                int count = 0; // Count of player's symbols in the row
                int emptyCount = 0; // Count of empty cells in the row
                for (int col = 0; col < 3; col++)
                {
                    if (buttons[row, col].Text == playerSymbol.ToString())
                    {
                        count++;
                    }
                    else if (buttons[row, col].Text == "")
                    {
                        emptyCount++;
                    }
                }

                if (count == 2 && emptyCount == 1)
                {
                    return true;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                int count = 0; // Count of player's symbols in the column
                int emptyCount = 0; // Count of empty cells in the column
                for (int row = 0; row < 3; row++)
                {
                    if (buttons[row, col].Text == playerSymbol.ToString())
                    {
                        count++;
                    }
                    else if (buttons[row, col].Text == "")
                    {
                        emptyCount++;
                    }
                }

                if (count == 2 && emptyCount == 1)
                {
                    return true;
                }
            }

            // Check diagonals
            int diagonal1Count = 0;
            int diagonal2Count = 0;
            int diagonal1EmptyCount = 0;
            int diagonal2EmptyCount = 0;

            for (int i = 0; i < 3; i++)
            {
                if (buttons[i, i].Text == playerSymbol.ToString())
                {
                    diagonal1Count++;
                }
                else if (buttons[i, i].Text == "")
                {
                    diagonal1EmptyCount++;
                }

                if (buttons[i, 2 - i].Text == playerSymbol.ToString())
                {
                    diagonal2Count++;
                }
                else if (buttons[i, 2 - i].Text == "")
                {
                    diagonal2EmptyCount++;
                }
            }

            if (diagonal1Count == 2 && diagonal1EmptyCount == 1)
            {
                return true;
            }

            if (diagonal2Count == 2 && diagonal2EmptyCount == 1)
            {
                return true;
            }

            return false;
        }
        private bool CheckForWin(string playerSymbol)
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (buttons[row, 0].Text == playerSymbol && buttons[row, 1].Text == playerSymbol && buttons[row, 2].Text == playerSymbol)
                {
                    return true;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (buttons[0, col].Text == playerSymbol && buttons[1, col].Text == playerSymbol && buttons[2, col].Text == playerSymbol)
                {
                    return true;
                }
            }

            // Check diagonals
            if (buttons[0, 0].Text == playerSymbol && buttons[1, 1].Text == playerSymbol && buttons[2, 2].Text == playerSymbol)
            {
                return true;
            }

            if (buttons[0, 2].Text == playerSymbol && buttons[1, 1].Text == playerSymbol && buttons[2, 0].Text == playerSymbol)
            {
                return true;
            }

            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void SelectDifficulty_Click(object sender, EventArgs e)
        {
            if (Difficulty == "Hard")
            {
                Difficulty = "Easy";
                SelectDifficulty.Text = "Change Difficulty To Hard?";
            }
            else
            {
                Difficulty = "Hard";
                SelectDifficulty.Text = "Change Difficulty To Easy?";
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            gameEnded = false; // Reset the game state
            currentPlayer = 'X'; // Reset the current player

            InitializeGame(); // Initialize the game board

            // Clear the winner message if it's currently displayed
            
        }
    }
}
