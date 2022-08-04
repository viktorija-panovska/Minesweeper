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

		private Label remainingMines;  // (# total mines - # flagged squares)

		private Timer timer;
		private Label timePassed;


		public GameWindow()
		{
			// Form properties
			Name = "Minesweeper";
			BackColor = Color.Gray;
			ClientSize = new Size((GameState.Difficulty.BoardWidth * 35) + 100, (GameState.Difficulty.BoardHeight * 35) + 100);
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.None;


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


			SetBoard();

			SetMinesDisplay();

			SetTimerDisplay();
		}


		private void Exit_OnClick(object sender, MouseEventArgs e)
		{
			Application.Exit();
		}



		// -- SETUP --

		// Initializes the game board, displays the tile grid representing the game board and
		// sets up the click events for every tile
		private void SetBoard()
		{
			// intialize and fill grid
			grid = new Tile[GameState.Difficulty.BoardHeight, GameState.Difficulty.BoardWidth];

			for (int y = 0; y < GameState.Difficulty.BoardHeight; ++y)
			{
				for (int x = 0; x < GameState.Difficulty.BoardWidth; ++x)
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

			// populate board
			board = new Board(GameWon, GameLost, RefreshTile);

			// initialize click events for every tile
			foreach (Tile tile in grid)
				tile.Button.MouseClick += new MouseEventHandler((sender, e) => OnClick(sender, e, tile));
		}

		// Displays the number of mines remaining to be marked
		// (# mines - # flagged squares)
		private void SetMinesDisplay()
		{
			Label minesLabel = new Label()
			{
				Location = new Point(0, 10),
				Size = new Size(200, 30),
				Text = "Mines Remaining:",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(minesLabel);

			remainingMines = new Label()
			{
				Location = new Point(70, 30),
				Size = new Size(40, 50),
				Text = board.Mines.ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(remainingMines);
		}

		// Displays the timer in minutes:seconds format
		private void SetTimerDisplay()
		{
			Label timerLabel = new Label()
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
			timer.Tick += new EventHandler(OnTick);

			timePassed = new Label()
			{
				Size = new Size(190, 50),
				Location = new Point(ClientSize.Width - 200, 30),
				Text = $"{board.PlayTime / 60:D2}:{board.PlayTime % 60:D2}",
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(timePassed);
		}

		// Increments the board timer every second and updates the timer display
		private void OnTick(object sender, EventArgs e)
		{
			board.IncrementTime();
			timePassed.Text = $"{board.PlayTime / 60:D2}:{board.PlayTime % 60:D2}";
		}



		// -- GAMEPLAY --
	
		// Handles left or right mouse click events on a tile and checks if a game is won
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

			if (board.IsGameWon())
				board.GameWon();
		}

		// Sets the state of the cell in the board to Revealed
		private void RevealTile(Tile tile)
		{
			board.Reveal(tile.X, tile.Y);
		}

		// Sets the state of the cell in the board to Flagged and modifies the amount of mines remaining
		private void FlagTile(Tile tile)
		{
			board.Flag(tile.X, tile.Y);
			remainingMines.Text = board.RemainingMines.ToString();
		}

		// Sets a new image for the tile at the given coordinates
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
    }
}