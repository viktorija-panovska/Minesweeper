using System;
using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
	struct Tile
	{
		public int X { get; }
		public int Y { get; }
		public PictureBox Button { get; }

		public Tile(int x, int y, PictureBox button)
		{
			X = x;
			Y = y;
			Button = button;
		}
	}


	public class GameWindow : Form
	{
		private Board board;        // logical representation of the game board
		private Tile[,] grid;       // visual representation of the game board

		private Label minesLabel;
		private Label minesDisplay;

		private Timer timer;
		private Label timerLabel;
		private Label timerDisplay;


		public GameWindow(Difficulty difficulty)
		{
			// Form properties
			Name = "Minesweeper";
			BackColor = Color.Gray;
			ClientSize = new Size((difficulty.BoardWidth * 35) + 100, (difficulty.BoardHeight * 35) + 100);
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.None;
			MaximizeBox = false;


			Button exit = new Button()
			{
				Location = new Point(ClientSize.Width - 30, 5),
				Size = new Size(20, 20),
				Text = "X",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point),
				BackColor = Color.Red
			};
			Controls.Add(exit);
			exit.MouseClick += new MouseEventHandler(Exit_OnClick);


			SetBoard(difficulty);

			SetMinesDisplay();

			SetTimerDisplay();


			// Event to shut down the entire program when the window is closed
			FormClosing += new FormClosingEventHandler(GameWindow_FormClosing);
		}


		private void Exit_OnClick(object sender, MouseEventArgs e)
		{
			Application.Exit();
		}



		// -- SETUP --

		// Initializes the game board, the tile grid representing the game board and the click events for every tile
		private void SetBoard(Difficulty difficulty)
		{
			// initialize visual representation of the board - the grid
			grid = new Tile[difficulty.BoardHeight, difficulty.BoardWidth];

			for (int y = 0; y < difficulty.BoardHeight; ++y)
			{
				for (int x = 0; x < difficulty.BoardWidth; ++x)
				{
					PictureBox button = new PictureBox()
					{
						Location = new Point(x * 35 + 50, y * 35 + 80),
						Size = new Size(35, 35),
						SizeMode = PictureBoxSizeMode.StretchImage,
					};
					grid[y, x] = new Tile(x, y, button);
					Controls.Add(button);
				}
			}

			// populate the board
			board = new Board(difficulty, GameWon, GameLost, RefreshTile);

			// initialize click events
			foreach (Tile tile in grid)
				tile.Button.MouseClick += new MouseEventHandler((sender, e) => OnClick(sender, e, tile));
		}

		// Initializes minesLabel and minesDisplay
		private void SetMinesDisplay()
		{
			minesLabel = new Label()
			{
				Location = new Point(0, 10),
				Size = new Size(200, 30),
				Text = "Mines Remaining:",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(minesLabel);

			minesDisplay = new Label()
			{
				Location = new Point(70, 30),
				Size = new Size(40, 50),
				Text = board.Mines.ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(minesDisplay);
		}

		// Initializes timerLabel, timerDisplay and the timer counting every second
		private void SetTimerDisplay()
		{
			timerLabel = new Label()
			{
				Size = new Size(200, 30),
				Location = new Point(ClientSize.Width - 200, 10),
				Text = "Time Elapsed:",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(timerLabel);

			timer = new Timer()
			{
				Enabled = true,
				Interval = 1000
			};
			timer.Start();
			timer.Tick += new EventHandler(OnTick);

			timerDisplay = new Label()
			{
				Size = new Size(190, 50),
				Location = new Point(ClientSize.Width - 200, 30),
				Text = board.PlayTime.ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(timerDisplay);
		}

		// Increments the timer every second and updates the timerDisplay
		private void OnTick(object sender, EventArgs e)
		{
			board.IncrementTime();
			timerDisplay.Text = board.PlayTime.ToString();
		}



		// -- GAMEPLAY --
	
		private void OnClick(object sender, MouseEventArgs e, Tile tile)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					RevealTile(tile);
					break;

				case MouseButtons.Right:
					FlagTile(tile);
					break;
			}
		}

		private void RevealTile(Tile tile)
		{
			board.Reveal(tile.X, tile.Y);
		}

		private void FlagTile(Tile tile)
		{
			board.Flag(tile.X, tile.Y);
			minesDisplay.Text = board.RemainingMines.ToString();
		}

		private void RefreshTile(int x, int y, Image image)
        {
			grid[y, x].Button.Image = image;
		}



		// -- GAME OVER --

		private void GameLost()
		{
			timer.Stop();
			FormSwitcher.ShowGameOver();
        }

		private void GameWon()
		{
			timer.Stop();
			FormSwitcher.ShowGameWon();
		}



        // -- UTILITY --

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (e.CloseReason == CloseReason.WindowsShutDown)
				Application.Exit();
		}
    }
}