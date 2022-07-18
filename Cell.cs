using System;

public enum State { Hidden, Revealed, Flagged }

public class Cell
{
	private State state;


	public Cell()
	{
		state = State.Hidden;
	}

	public void Flag()
    {
		state = State.Flagged;
    }

	public void Reveal()
	{
		state = State.Revealed;
    }

	public State GetState() => state;
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