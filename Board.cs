using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Minesweeper
{
	public class Board
	{
		public delegate void GameOverFunc();
		public delegate void RefreshCellDisplay(int x, int y, Image image);

		private readonly Cell[,] board;

		public Difficulty Difficulty { get; }

		public int Width { get => Difficulty.BoardWidth; }
		public int Height { get => Difficulty.BoardHeight; }
		public int Mines { get => Difficulty.Mines; }
		public int RemainingMines { get => Difficulty.Mines - flagged; }
		public int PlayTime { get; private set; }

		private const int threadCount = 8;

		private int flagged;
		private int correctlyFlagged;

		private readonly GameOverFunc gameWon;
		private readonly GameOverFunc gameLost;
		private readonly RefreshCellDisplay refresh;


		public Board(Difficulty difficulty, GameOverFunc gameWon, GameOverFunc gameLost, RefreshCellDisplay refresh)
		{
			Difficulty = difficulty;
			board = new Cell[Height, Width];

			flagged = 0;
			correctlyFlagged = 0;

			this.gameWon = gameWon;
			this.gameLost = gameLost;
			this.refresh = refresh;

			FillBoard();
		}



		// -- SETUP --

		// Sets the cells that are occupied by mines and counts the number of adjacent mines for the other cells
		private void FillBoard()
		{
			PlaceMines();
			PlaceFreeCells(0, Height - 1, threadCount);
		}

		// Sets cells at random coordinates in the board to be mines
		private void PlaceMines()
		{
			Random ranGen = new Random();

			int remainingMines = Mines;
			while (remainingMines > 0)
			{
				int x = ranGen.Next(0, Width);
				int y = ranGen.Next(0, Height);

				if (board[y, x] == null)
				{
					board[y, x] = new MineCell();
					remainingMines--;
					refresh(x, y, board[y, x].Image);
				}
			}
		}

		// Fills out the cells that don't contain mines with empty cells, counting the numebr of adjacent mines they have
		private void PlaceFreeCells(int from, int to, int threads)
        {
			if (threads == 1)
				FillRow(from, to);
			else
            {
				var thread1 = new Thread(() => PlaceFreeCells(from, from + (to - from) / 2, threads / 2));
				thread1.Start();

				PlaceFreeCells(from + (to - from) / 2 + 1, to, threads - (threads / 2));
				thread1.Join();
            }
		}

		private void FillRow(int from, int to)
        {
			for (int y = from; y <= to; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					if (board[y, x] == null)
					{
						board[y, x] = new FreeCell(CountAdjacentMines(x, y));
						refresh(x, y, board[y, x].Image);
					}
				}
			}
		}

		// Counts the number of mines in the 8 cells surrounding each cell that doesn't contain a mine
		private int CountAdjacentMines(int x, int y)
		{
			int count = 0;

			for (int dx = -1; dx <= 1; ++dx)
				for (int dy = -1; dy <= 1; ++dy)
					if (x + dx < Width && x + dx >= 0 &&
						y + dy < Height && y + dy >= 0 &&
						board[y + dy, x + dx] is MineCell)
						count++;

			return count;
		}



		// -- GAMEPLAY --

		public bool Reveal(int x, int y)
        {
			Cell cell = board[y, x];

			if (cell.State != CellState.Hidden)
				return false;

			cell.Reveal();
			refresh(x, y, cell.Image);

			if (cell is MineCell)
				GameLost();
				
			if (IsGameWon())
				GameWon();

			if (board[y, x] is FreeCell freeCell && freeCell.AdjacentMines == 0)
				for (int dx = -1; dx <= 1; ++dx)
					for (int dy = -1; dy <= 1; ++dy)
						if (x + dx < Width && x + dx >= 0 &&
							y + dy < Height && y + dy >= 0)
							Reveal(x + dx, y + dy);

			return true;
        }

		public void Flag(int x, int y)
        {
			Cell cell = board[y, x];
			if (cell.State == CellState.Hidden)
            {
				cell.Flag();
				flagged++;

				if (cell is MineCell)
					correctlyFlagged++;

				if (IsGameWon())
					GameWon();
			}
			else if (cell.State == CellState.Flagged)
            {
				cell.Hide();
				flagged--;

				if (cell is MineCell)
					correctlyFlagged--;
			}

			refresh(x, y, cell.Image);
        }

		public void IncrementTime() => PlayTime++;
		


		// -- ENDGAME --

		public bool IsGameWon() => correctlyFlagged == Mines && correctlyFlagged == flagged;

		public void GameLost()
        {
			MarkMines(0, Height - 1, threadCount);
			gameLost();
        }

		private void MarkMines(int from, int to, int threads)
        {
			if (threads == 1)
				MarkRow(from, to);
			else
			{
				var thread1 = new Thread(() => MarkMines(from, from + (to - from) / 2, threads / 2));
				thread1.Start();

				MarkMines(from + (to - from) / 2 + 1, to, threads - (threads / 2));
				thread1.Join();
			}
		}

		private void MarkRow(int from, int to)
        {
			for (int y = from; y <= to; ++y)
			{
				for (int x = 0; x < Width; ++x)
				{
					Cell cell = board[y, x];

					if (cell.State != CellState.Revealed)
					{
						if (cell is FreeCell && cell.State == CellState.Flagged)
							cell.WrongFlag();
						else if (cell is MineCell)
							cell.Reveal();
					}

					refresh(x, y, cell.Image);
				}
			}
		}

		public void GameWon()
        {
			ScoreManager.SaveScore(new HighScore()
			{
				Difficulty = Difficulty.Name,
				PlayTime = PlayTime,
				DateTime = DateTime.Now
			});

			gameWon();
        }
	}
}