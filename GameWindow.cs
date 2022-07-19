using System;
using System.Drawing;
using System.Diagnostics;
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
		private readonly Image[] numberTiles = {
			Properties.Resources.Zero,
			Properties.Resources.One,
			Properties.Resources.Two,
			Properties.Resources.Three,
			Properties.Resources.Four,
			Properties.Resources.Five,
			Properties.Resources.Six,
			Properties.Resources.Seven,
			Properties.Resources.Eight
		};

		private Board board;		// logical representation of the game board
		private Tile[,] grid;		// visual representation of the game board

		private Label minesLabel;
		private Label minesDisplay;

		private Timer timer;
		private int seconds;
		private Label timerLabel;
		private Label timerDisplay;


		public GameWindow(int boardWidth, int boardHeight, int mines)
		{
			board = new Board(boardHeight, boardWidth, mines);

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
						Image = Properties.Resources.Tile,
						SizeMode = PictureBoxSizeMode.StretchImage,
					};
					grid[y, x] = new Tile(x, y, button);
					Controls.Add(button);
				}
			}

			foreach (Tile tile in grid)
				tile.Button.MouseClick += new MouseEventHandler((sender, e) => OnClick(sender, e, tile));				
		}		


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
			Cell cell = board.GetCell(tile.X, tile.Y);

			if (cell.GetState() != State.Revealed && cell is MineCell)
				GameOver();


			if (cell.GetState() != State.Revealed && cell is FreeCell freeCell)
			{
				tile.Button.Image = numberTiles[freeCell.AdjacentMines];
				tile.Button.Enabled = false;
				cell.Reveal();
				board.HiddenCells--;

				if (freeCell.AdjacentMines == 0)
                {
					for (int dx = -1; dx <= 1; ++dx)
					{
						for (int dy = -1; dy <= 1; ++dy)
						{
							if (tile.X + dx >= grid.GetLength(1) || tile.X + dx < 0 || tile.Y + dy >= grid.GetLength(0) || tile.Y + dy < 0)
								continue;
							else
								RevealTile(grid[tile.Y + dy, tile.X + dx]);
						}
					}
				}
			}
		}


		// Flags tile if it is not flagged, unflags it otherwise
		private void FlagTile(Tile tile)
        {
			Cell cell = board.GetCell(tile.X, tile.Y);

			if (cell.GetState() == State.Hidden)
			{
				tile.Button.Image = Properties.Resources.Flag;
				cell.Flag();
				board.FlaggedCells++;
			}
			else if (cell.GetState() == State.Flagged)
            {
				tile.Button.Image = Properties.Resources.Tile;
				cell.Hide();
				board.FlaggedCells--;
			}

			minesDisplay.Text = (board.Mines - board.FlaggedCells).ToString();
		}



		private void GameOver()
		{
			timer.Stop();
			
			foreach (Tile tile in grid)
            {
                if (board.GetCell(tile.X, tile.Y) is MineCell)
					tile.Button.Image = Properties.Resources.Mine;

				tile.Button.Enabled = false;
            }

            // game over mark
        }

		private void GameWon()
        {
			timer.Stop();
			Leaderboard leaderboard = new Leaderboard(seconds);
			Hide();
			leaderboard.Show();
		}



		private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.Exit();
		}

	}
}


