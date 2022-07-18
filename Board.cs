using System;

public class Board
{
	private Cell[,] board;

	public int Width { get; }

	public int Height { get; }

	public int Mines { get; }


	public Board(int width, int height, int mines)
	{
		board = new Cell[height, width];

		Width = width;
		Height = height;
		Mines = mines;

		FillBoard();
	}

	public Cell GetCell(int x, int y) => board[y, x];


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
}
