using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class MainMenu : Form
    {
        private const string invalidNameChars = "\\/?*|:<>";

        private Label difficulty;
        private Label boardSize;
        private Label numMines;
        private TextBox playerNameEntry;

        private readonly Difficulty[] difficulties =
        {
            new Difficulty(DifficultyName.Beginner, 9, 9, 10),
            new Difficulty(DifficultyName.Intermediate, 16, 16, 40),
            new Difficulty(DifficultyName.Expert, 30, 16, 99)
        };

        private int difficultyIndex = 0;        // which difficulty the difficulty selector is currently pointing at

        public MainMenu()
        {
            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            Button exit = new Button()
            {
                Location = new Point(ClientSize.Width - 40, 10),
                Size = new Size(30, 30),
                Text = "X",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = Color.Red
            };
            Controls.Add(exit);
            exit.MouseClick += new MouseEventHandler(Exit_OnClick);

            Label title = new Label()
            {
                Location = new Point(75, 50),
                Size = new Size(350, 50),
                Text = "MINESWEEPER",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(title);

            ShowPlayerNameEntry();

            ShowDifficultySelector();

            Button startGame = new Button()
            {
                Location = new Point(195, 400),
                Size = new Size(100, 50),
                Text = "START",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            Controls.Add(startGame);
            startGame.MouseClick += new MouseEventHandler(StartGame_OnClick);
        }

        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        // Displays the prompt and the text box for player name entry
        private void ShowPlayerNameEntry()
        {
            Label nameLabel = new Label()
            {
                Location = new Point(75, 100),
                Size = new Size(350, 50),
                Text = "Enter Your Name:",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(nameLabel);

            playerNameEntry = new TextBox()
            {
                Location = new Point(123, 150),
                Size = new Size(250, 50),
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(playerNameEntry);
        }

        // Displays the prompt for difficulty selection, the arrows for cycling through the difficulties
        // and the name, board size and number of mines for each difficulty
        private void ShowDifficultySelector()
        {
            Label chooseDifficulty = new Label()
            {
                Location = new Point(170, 200),
                Size = new Size(150, 50),
                Text = "Difficulty:",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(chooseDifficulty);

            difficulty = new Label()
            {
                Location = new Point(170, 250),
                Size = new Size(150, 40),
                Text = difficulties[difficultyIndex].Name.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(difficulty);

            Button decreaseDifficulty = new Button()
            {
                Location = new Point(120, 252),
                Size = new Size(35, 35),
                Text = "<",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            Controls.Add(decreaseDifficulty);
            decreaseDifficulty.MouseClick += new MouseEventHandler(DecreaseDifficulty_OnClick);

            Button increaseDifficulty = new Button()
            {
                Location = new Point(330, 252),
                Size = new Size(35, 35),
                Text = ">",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            Controls.Add(increaseDifficulty);
            increaseDifficulty.MouseClick += new MouseEventHandler(IncreaseDifficulty_OnClick);

            boardSize = new Label()
            {
                Location = new Point(60, 320),
                Size = new Size(150, 40),
                Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(boardSize);

            numMines = new Label()
            {
                Location = new Point(230, 320),
                Size = new Size(200, 40),
                Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(numMines);
        }

        // Cycles the difficulties towards the lower difficulty, wrapping around when it reaches the lowest
        private void DecreaseDifficulty_OnClick(object sender, MouseEventArgs e)
        {
            difficultyIndex--;
            if (difficultyIndex < 0)
                difficultyIndex = difficulties.Length - 1;

            UpdateLabels();
        }

        // Cycles the difficulties towards the higher difficulty, wrapping around when it reaches the highest
        private void IncreaseDifficulty_OnClick(object sender, MouseEventArgs e)
        {
            difficultyIndex++;
            if (difficultyIndex >= difficulties.Length)
                difficultyIndex = 0;

            UpdateLabels();
        }

        // Updates the difficulty stats, called when cycling through difficulties
        private void UpdateLabels()
        {
            difficulty.Text = difficulties[difficultyIndex].Name.ToString();
            boardSize.Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}";
            numMines.Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}";
        }

        // Sets the player name and difficulty for this session and starts the game
        private void StartGame_OnClick(object sender, MouseEventArgs e)
        {
            if (playerNameEntry.Text == "")
                MessageBox.Show("Please enter your name");
            else if (playerNameEntry.Text.Length > 15)
                MessageBox.Show("Please enter a name with up to 15 characters");
            else if (IsInvalidName())
                MessageBox.Show("Name cannot contain the following characters: " + invalidNameChars);
            else
            {
                GameState.PlayerName = playerNameEntry.Text;
                GameState.Difficulty = difficulties[difficultyIndex];
                FormSwitcher.ShowGameWindow();
            }
        }

        // Checks if the name provided contains any of the invalid characters
        private bool IsInvalidName()
        {
            foreach (char c in invalidNameChars)
                if (playerNameEntry.Text.Contains(c))
                    return true;

            return false;
        }
    }
}
