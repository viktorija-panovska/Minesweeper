using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{

    public partial class MainMenu : Form
    {
        private readonly Label difficulty;
        private readonly Label boardSize;
        private readonly Label numMines;

        readonly Difficulty[] difficulties =
        {
            new Difficulty(DifficultyName.Beginner, 9, 9, 10),
            new Difficulty(DifficultyName.Intermediate, 16, 16, 40),
            new Difficulty(DifficultyName.Expert, 30, 16, 99)
        };

        int difficultyIndex = 0;        // which difficulty the difficulty selector is currently pointing at

        public MainMenu()
        {
            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;


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

            Label chooseDifficulty = new Label()
            {
                Location = new Point(170, 130),
                Size = new Size(150, 50),
                Text = "Difficulty:",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(chooseDifficulty);

            difficulty = new Label()
            {
                Location = new Point(170, 170),
                Size = new Size(150, 40),
                Text = difficulties[difficultyIndex].Name.ToString(),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(difficulty);

            Button decreaseDifficulty = new Button()
            {
                Location = new Point(120, 172),
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
                Location = new Point(330, 172),
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
                Location = new Point(170, 250),
                Size = new Size(150, 40),
                Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(boardSize);

            numMines = new Label()
            {
                Location = new Point(145, 320),
                Size = new Size(200, 40),
                Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(numMines);

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

            // Event to shut down the entire program when the window is closed
            FormClosing += new FormClosingEventHandler(GameWindow_FormClosing);
        }

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                Application.Exit();
        }

        private void DecreaseDifficulty_OnClick(object sender, MouseEventArgs e)
        {
            difficultyIndex--;
            if (difficultyIndex < 0)
                difficultyIndex = difficulties.Length - 1;

            UpdateLabels();
        }

        private void IncreaseDifficulty_OnClick(object sender, MouseEventArgs e)
        {
            difficultyIndex++;
            if (difficultyIndex >= difficulties.Length)
                difficultyIndex = 0;

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            difficulty.Text = difficulties[difficultyIndex].Name.ToString();
            boardSize.Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}";
            numMines.Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}";
        }

        private void StartGame_OnClick(object sender, MouseEventArgs e)
        {
            GameState.Difficulty = difficulties[difficultyIndex];
            FormSwitcher.ShowGameWindow();
        }

        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }
    }
}
