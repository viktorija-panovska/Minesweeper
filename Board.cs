using System;
using System.Drawing;

namespace Minesweeper
{
	public class Board
	{
		public delegate void GameOverFunc();


		private readonly Cell[,] board;

		public int Width { get; }

		public int Height { get; }

		public int Mines { get; }

		public int FlaggedCells { get; private set; }

		private int hiddenCells;

		private GameOverFunc gameWon;
		private GameOverFunc gameLost;		


		public Board(int width, int height, int mines, GameOverFunc gameWon, GameOverFunc gameLost)
		{
			board = new Cell[height, width];

			Width = width;
			Height = height;
			Mines = mines;

			FlaggedCells = 0;
			hiddenCells = width * height;

			this.gameWon = gameWon;
			this.gameLost = gameLost;

			FillBoard();
		}

		public Image GetCellImage(int x, int y) => board[y, x].Image;

		public bool IsCellEmpty(int x, int y) => board[y, x] is FreeCell freeCell && freeCell.AdjacentMines == 0;


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
						board[y, x] = new FreeCell(CountAdjacentMines(x, y));
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
			hiddenCells--;

			if (cell is MineCell)
				GameLost();
				
			if (IsGameWon())
				GameWon();

			return true;
        }

		public void Flag(int x, int y)
        {
			Cell cell = board[y, x];
			if (cell.State == CellState.Hidden)
            {
				cell.Flag();
				FlaggedCells++;
			}
			else if (cell.State == CellState.Flagged)
            {
				cell.Hide();
				FlaggedCells--;
			}
        }



		// Returns true if all mines have been correctly flagged and the rest of the cells have been revealed
		public bool IsGameWon()
		{
			if (hiddenCells - FlaggedCells != 0)
				return false;
		
			for (int x = 0; x < Width; ++x)
				for (int y = 0; y < Height; ++y)
					if (board[y, x].State == CellState.Flagged && !(board[y, x] is MineCell))
						return false;				

			return true;
		}


		public void GameLost()
        {
			foreach (Cell cell in board)
            {
				if (cell is FreeCell && cell.State == CellState.Flagged)
					cell.WrongFlag();
				else if (cell is MineCell)
					cell.Reveal();
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