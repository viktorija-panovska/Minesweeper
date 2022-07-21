using System;
using System.Drawing;
using System.Timers;

namespace Minesweeper
{
	public class Board
	{
		public delegate void GameOverFunc();
		public delegate void RefreshCellDisplay(int x, int y, Image image);

		private readonly Cell[,] board;
		public int Width { get; }
		public int Height { get; }
		public int Mines { get; }
		public int RemainingMines { get; private set; }

		private int flagged;
		private int correctlyFlagged;

		private GameOverFunc gameWon;
		private GameOverFunc gameLost;
		private RefreshCellDisplay refresh;


		public Board(int width, int height, int mines, GameOverFunc gameWon, GameOverFunc gameLost, RefreshCellDisplay refresh)
		{
			board = new Cell[height, width];

			Width = width;
			Height = height;
			Mines = mines;
			RemainingMines = mines;

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

			for (int y = 0; y < Height; ++y)
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

		// Sets cells at random coordinates in the board to be mines
		private void PlaceMines()
		{
			Random ranGen = new Random();

			int remainingMines = Mines;
			while (remainingMines > 0)
			{
				int x = ranGen.Next(0, Width - 1);
				int y = ranGen.Next(0, Height - 1);

				if (board[y, x] == null)
				{
					board[y, x] = new MineCell();
					remainingMines--;
					refresh(x, y, board[y, x].Image);
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

				RemainingMines--;

				if (IsGameWon())
					GameWon();
			}
			else if (cell.State == CellState.Flagged)
            {
				cell.Hide();
				flagged--;

				if (cell is MineCell)
					correctlyFlagged--;

				RemainingMines++;
			}

			refresh(x, y, cell.Image);
        }

		

		// -- ENDGAME --

		public bool IsGameWon() => correctlyFlagged == Mines && correctlyFlagged == flagged;

		public void GameLost()
        {
			for (int x = 0; x < Width; ++x)
            {
				for (int y = 0; y < Height; ++y)
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
				
			gameLost();
        }

		public void GameWon()
        {
			// save game state

			gameWon();
        }
	}
}