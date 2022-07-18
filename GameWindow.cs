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

		private Board board;
		private Tile[,] grid;

		private int mines;
		private Label minesLabel;
		private Label minesDisplay;

		private Timer timer;
		private int seconds;
		private Label timerLabel;
		private Label timerDisplay;


		public GameWindow(int boardWidth, int boardHeight, int mines)
		{
			// Form properties
			Name = "Minesweeper";
			BackColor = Color.Gray;
			ClientSize = new Size((boardWidth * 35) + 100, (boardHeight * 35) + 100);
			StartPosition = FormStartPosition.CenterScreen;
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;


			this.mines = mines;
			SetMinesDisplay();

			SetTimerDisplay();

			SetBoard(boardHeight, boardWidth, mines);


			// Event to shut down the entire program when the window is closed
			FormClosing += new FormClosingEventHandler(GameWindow_FormClosing);
		}

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
				Text = mines.ToString(),
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
				ForeColor = Color.Black
			};
			Controls.Add(minesDisplay);
		}

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

		private void SetBoard(int height, int width, int mines)
        {
			board = new Board(width, height, mines);

			grid = new Tile[height, width];
			int i = 0;
			for (int y = 0; y < height; ++y)
			{
				for (int x = 0; x < width; ++x)
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
					i++;
				}
			}

			foreach (Tile tile in grid)
				tile.Button.MouseClick += new MouseEventHandler((sender, e) => OnClick(sender, e, tile));				
		}


		private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.Exit();
		}


		private void OnTick(object sender, EventArgs e)
        {
			seconds++;
			timerDisplay.Text = seconds.ToString();
        }


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
			if (board.GetCell(tile.X, tile.Y) is MineCell)
            {
				tile.Button.Image = Properties.Resources.Mine;
				GameOver();
			}
				
			if (board.GetCell(tile.X, tile.Y) is FreeCell)
			{
				FreeCell cell = (FreeCell)board.GetCell(tile.X, tile.Y);
				tile.Button.Image = numberTiles[cell.AdjacentMines];
			}

			tile.Button.Enabled = false;

			board.GetCell(tile.X, tile.Y).Reveal();
		}

		private void FlagTile(Tile tile)
        {
			tile.Button.Image = Properties.Resources.Flag;

			board.GetCell(tile.X, tile.Y).Flag();

			mines--;
			minesDisplay.Text = mines.ToString();
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
	}
}


