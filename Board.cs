using System;

public class Board
{
	private readonly Cell[,] board;

	public int Width { get; }

	public int Height { get; }

	public int Mines { get; }

	public int FlaggedCells { get; set; }

	public int HiddenCells { get; set; }


	public Board(int width, int height, int mines)
	{
		board = new Cell[height, width];

		Width = width;
		Height = height;
		Mines = mines;
		FlaggedCells = 0;
		HiddenCells = width * height;

		FillBoard();
	}

	public Cell GetCell(int x, int y) => board[y, x];


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
        {
			for (int dy = -1; dy <= 1; ++dy)
            {
				if (x + dx >= Width || x + dx < 0 || y + dy >= Height || y + dy < 0)
					continue;

				if (board[y + dy, x + dx] is MineCell)
					count++;
			}
        }

		return count;
    }


	// Returns true if all mines have been correctly flagged and the rest of the cells have been revealed
	public bool IsGameWon()
    {
		if (HiddenCells > 0)
			return false;

		for (int x = 0; x < Width; ++x)
			for (int y = 0; y < Height; ++y)
				if (board[y, x].GetState() == State.Flagged && !(board[y, x] is MineCell))
					return false;

		return true;
    }
}
