using System;
using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    struct Difficulty
    {
        public string Name { get; }
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public int Mines { get; }

        public Difficulty(string name, int boardWidth, int boardHeight, int mines)
        {
            Name = name;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Mines = mines;
        }
    }


    public partial class MainMenu : Form
    {
        readonly Label title;
        readonly Label chooseDifficulty;
        readonly Label difficulty;
        readonly Button decreaseDifficulty;
        readonly Button increaseDifficulty;
        readonly Label boardSize;
        readonly Label numMines;
        readonly Button startGame;

        readonly Difficulty[] difficulties =
        {
            new Difficulty("Beginner", 9, 9, 10),
            new Difficulty("Intermediate", 16, 16, 40),
            new Difficulty("Expert", 30, 16, 99)
        };

        int difficultyIndex = 0;        // which difficulty the difficulty selector is currently pointing at

        public MainMenu()
        {
            title = new Label()
            {
                Location = new Point(75, 30),
                Size = new Size(350, 50),
                Text = "MINESWEEPER",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };

            chooseDifficulty = new Label()
            {
                Location = new Point(170, 110),
                Size = new Size(150, 50),
                Text = "Difficulty:",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };

            difficulty = new Label()
            {
                Location = new Point(170, 150),
                Size = new Size(150, 40),
                Text = difficulties[difficultyIndex].Name,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };

            decreaseDifficulty = new Button()
            {
                Location = new Point(120, 152),
                Size = new Size(35, 35),
                Text = "<",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            decreaseDifficulty.Click += new EventHandler(DecreaseDifficulty_OnClick);

            increaseDifficulty = new Button()
            {
                Location = new Point(330, 152),
                Size = new Size(35, 35),
                Text = ">",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            increaseDifficulty.Click += new EventHandler(IncreaseDifficulty_OnClick);

            boardSize = new Label()
            {
                Location = new Point(170, 230),
                Size = new Size(150, 40),
                Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };

            numMines = new Label()
            {
                Location = new Point(145, 300),
                Size = new Size(200, 40),
                Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };

            startGame = new Button()
            {
                Location = new Point(195, 380),
                Size = new Size(100, 50),
                Text = "START",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 20F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.White,
            };
            startGame.Click += new EventHandler(StartGame_OnClick);


            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Controls.Add(title);
            Controls.Add(chooseDifficulty);
            Controls.Add(difficulty);
            Controls.Add(decreaseDifficulty);
            Controls.Add(increaseDifficulty);
            Controls.Add(boardSize);
            Controls.Add(numMines);
            Controls.Add(startGame);

            // Event to shut down the entire program when the window is closed
            FormClosing += new FormClosingEventHandler(MainMenu_FormClosing);
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void DecreaseDifficulty_OnClick(object sender, EventArgs e)
        {
            difficultyIndex--;
            if (difficultyIndex < 0)
                difficultyIndex = difficulties.Length - 1;

            UpdateLabels();
        }

        private void IncreaseDifficulty_OnClick(object sender, EventArgs e)
        {
            difficultyIndex++;
            if (difficultyIndex >= difficulties.Length)
                difficultyIndex = 0;

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            difficulty.Text = difficulties[difficultyIndex].Name;
            boardSize.Text = $"Board size:\n{difficulties[difficultyIndex].BoardWidth}x{difficulties[difficultyIndex].BoardHeight}";
            numMines.Text = $"Number of mines:\n{difficulties[difficultyIndex].Mines}";
        }

        private void StartGame_OnClick(object sender, EventArgs e)
        {
            GameWindow game = new GameWindow(difficulties[difficultyIndex].BoardWidth, 
                                             difficulties[difficultyIndex].BoardHeight, 
                                             difficulties[difficultyIndex].Mines);
            Hide();
            game.Show();
        }
    }
}
