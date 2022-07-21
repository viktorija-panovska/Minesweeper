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
		private int seconds;
		private Label timerLabel;
		private Label timerDisplay;


		public GameWindow(int boardWidth, int boardHeight, int mines)
		{
			board = new Board(boardWidth, boardHeight, mines, GameWon, GameLost);

			// Form properties
			Name = "Minesweeper";
			BackColor = Color.Gray;
			ClientSize = new Size((board.Width * 35) + 100, (board.Height * 35) + 100);
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;


			SetMinesDisplay();

			SetTimerDisplay();

			SetBoardDisplay();


			// Event to shut down the entire program when the window is closed
			FormClosing += new FormClosingEventHandler(GameWindow_FormClosing);
		}


		// -- SETUP --

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
				Text = seconds.ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(timerDisplay);
		}

		// Increments the timer every second and updates the timerDisplay
		private void OnTick(object sender, EventArgs e)
		{
			seconds++;
			timerDisplay.Text = seconds.ToString();
		}

		// Initializes the tile grid representing the game board and the click events for every tile
		private void SetBoardDisplay()
		{
			grid = new Tile[board.Height, board.Width];

			for (int y = 0; y < board.Height; ++y)
			{
				for (int x = 0; x < board.Width; ++x)
				{
					PictureBox button = new PictureBox()
					{
						Location = new Point(x * 35 + 50, y * 35 + 80),
						Size = new Size(35, 35),
						Image = board.GetCellImage(x, y),
						SizeMode = PictureBoxSizeMode.StretchImage,
					};
					grid[y, x] = new Tile(x, y, button);
					Controls.Add(button);
				}
			}

			foreach (Tile tile in grid)
				tile.Button.MouseClick += new MouseEventHandler((sender, e) => OnClick(sender, e, tile));
		}



		// -- GAMEPLAY --
	
		// Handles the left click or right click functionality
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

		 
		// 
		private void RevealTile(Tile tile)
		{
			if (board.Reveal(tile.X, tile.Y))
            {
				tile.Button.Image = board.GetCellImage(tile.X, tile.Y);

				if (board.IsCellEmpty(tile.X, tile.Y))
					for (int dx = -1; dx <= 1; ++dx)
						for (int dy = -1; dy <= 1; ++dy)
							if (tile.X + dx < board.Width && tile.X + dx >= 0 &&
								tile.Y + dy < board.Height && tile.Y + dy >= 0)
								RevealTile(grid[tile.Y + dy, tile.X + dx]);
			}
		}


		private void FlagTile(Tile tile)
		{
			board.Flag(tile.X, tile.Y);
			minesDisplay.Text = (board.Mines - board.FlaggedCells).ToString();
			tile.Button.Image = board.GetCellImage(tile.X, tile.Y);
		}



		// -- GAME OVER --

		private void GameLost()
		{
			timer.Stop();

            foreach (Tile tile in grid)
            {
				tile.Button.Image = board.GetCellImage(tile.X, tile.Y);
				tile.Button.Enabled = false;
            }

            GameOverScreen gameOver = new GameOverScreen();
            gameOver.Show();
        }

		private void GameWon()
		{
			timer.Stop();
			Leaderboard leaderboard = new Leaderboard(seconds);
			leaderboard.Show();
		}



		// -- UTILITY --

		private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.Exit();
		}

	}
}