using System;

public enum State { Hidden, Revealed, Flagged }

public class Cell
{
	private State state;


	public Cell()
	{
		Hide();
	}

	public State GetState() => state;

	// Sets the state of the cell to Flagged
	public void Flag()
    {
		state = State.Flagged;
    }

	// Sets the state of the cell to Revealed
	public void Reveal()
	{
		state = State.Revealed;
    }

	// Sets the state of the cell to Hidden
	public void Hide()
    {
		state = State.Hidden;
    }
}


public class MineCell : Cell
{
}


public class FreeCell : Cell
{
	public int AdjacentMines { get; }

	public FreeCell(int adjacentMines) : base()
    {
		AdjacentMines = adjacentMines;
    }
}