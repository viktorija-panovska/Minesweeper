using System;
using System.Drawing;
using System.Threading;

namespace Minesweeper
{
	public class Board
	{
		public delegate void GameOverFunc();
		public delegate void RefreshCellDisplay(int x, int y, Image image);

		private readonly Cell[,] board;
		private readonly object boardLocker;

		public int Width { get => GameState.Difficulty.BoardWidth; }
		public int Height { get => GameState.Difficulty.BoardHeight; }
		public int Mines { get => GameState.Difficulty.Mines; }
		public int RemainingMines { get => GameState.Difficulty.Mines - flagged; }
		public int PlayTime { get; private set; }

		private const int threadCount = 8;

		private int flagged;
		private int correctlyFlagged;

		private readonly GameOverFunc gameWon;
		private readonly GameOverFunc gameLost;
		private readonly RefreshCellDisplay refresh;


		public Board(GameOverFunc gameWon, GameOverFunc gameLost, RefreshCellDisplay refresh)
		{
			board = new Cell[Height, Width];
			boardLocker = new object();

			flagged = 0;
			correctlyFlagged = 0;

			this.gameWon = gameWon;
			this.gameLost = gameLost;
			this.refresh = refresh;

			FillBoard();
		}



		// -- SETUP --

		// Sets the cells that are occupied by mines and sets the other cells as free cells
		private void FillBoard()
		{
			PlaceMines(threadCount, Mines);
			PlaceFreeCells(0, Height - 1, threadCount);
		}

        // Depending on the number of available threads, chooses either a sequential or parallel divide-and-conquer approach to 
		// placing the mines randomly on the board
        private void PlaceMines(int threads, int mines)
        {
            if (threads == 1)
                PlaceRandomMines(mines);
            else
            {
                Thread thread = new Thread(() => PlaceMines(threads / 2, mines / 2));
                thread.Start();

                PlaceMines(threads - (threads / 2), mines - (mines / 2));
                thread.Join();
            }
        }

		// Places mines at randomly generated coordinates on the board
        private void PlaceRandomMines(int mines)
        {
			Random ranGen = new Random();

			while (mines > 0)
			{
				int x = ranGen.Next(0, Width);
				int y = ranGen.Next(0, Height);

				lock (boardLocker)
                {
					if (board[y, x] == null)
					{
						board[y, x] = new MineCell();
						mines--;
					}
				}

				refresh(x, y, board[y, x].Image);
			}
		}

		// Depending on the number of available threads, chooses either a sequential or parallel divide-and-conquer approach to 
		// setting the free cells
		private void PlaceFreeCells(int from, int to, int threads)
        {
			if (threads == 1)
				FillRow(from, to);
			else
            {
				Thread thread = new Thread(() => PlaceFreeCells(from, from + (to - from) / 2, threads / 2));
				thread.Start();

				PlaceFreeCells(from + (to - from) / 2 + 1, to, threads - (threads / 2));
				thread.Join();
            }
		}

		// Fills the cells of the given rows that don't contain mines with empty cells,
		// counting the number of adjacent mines they have
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

		// Counts the number of mines in the 8 cells surrounding each free cell
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

		// Opens a hidden cell, ending the game if the cell is a mine or opening adjacent free squares if not
		public bool Reveal(int x, int y)
        {
			Cell cell = board[y, x];

			if (cell.State != CellState.Hidden)
				return false;

			cell.Reveal();
			refresh(x, y, cell.Image);

			if (cell is MineCell)
				GameLost();

			if (board[y, x] is FreeCell freeCell && freeCell.AdjacentMines == 0)
				for (int dx = -1; dx <= 1; ++dx)
					for (int dy = -1; dy <= 1; ++dy)
						if (x + dx < Width && x + dx >= 0 &&
							y + dy < Height && y + dy >= 0)
							Reveal(x + dx, y + dy);

			return true;
        }

		// Flags or unflags a cell, depending on the current state of the cell
		public void Flag(int x, int y)
        {
			Cell cell = board[y, x];
			if (cell.State == CellState.Hidden)
            {
				cell.Flag();
				flagged++;

				if (cell is MineCell)
					correctlyFlagged++;
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

		// Reveals the locations of all the mines, then goes to the Game Lost screen
		public void GameLost()
        {
			RevealMines(0, Height - 1, threadCount);
			gameLost();
        }

		// Depending on the number of available threads, chooses either a sequential or parallel divide-and-conquer approach to 
		// revealing the mines
		private void RevealMines(int from, int to, int threads)
        {
			if (threads == 1)
				RevealRow(from, to);
			else
			{
				var thread1 = new Thread(() => RevealMines(from, from + (to - from) / 2, threads / 2));
				thread1.Start();

				RevealMines(from + (to - from) / 2 + 1, to, threads - (threads / 2));
				thread1.Join();
			}
		}

		// Sequentially reveals the mine cells and highlights the flagged cells that didn't hold mines
		private void RevealRow(int from, int to)
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

		// Saves the score and goes to the Game Won screen
		public void GameWon()
        {
			SaveFileManager.SaveScore(new Score()
			{
				Difficulty = GameState.Difficulty.Name,
				PlayerName = GameState.PlayerName,
				DateTime = DateTime.Now,
				PlayTime = PlayTime,
				CorrectlyMarkedMines = correctlyFlagged
			});

			gameWon();
        }
	}
}