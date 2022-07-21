using System.Drawing;

namespace Minesweeper
{
	public enum CellState { Hidden, Revealed, Flagged }


	public class Cell
	{
		public CellState State { get; protected set; }

		public Image Image { get; protected set; }


		public Cell()
		{
			Hide();
		}


		// Sets the state of the cell to Hidden
		public void Hide()
		{
			State = CellState.Hidden;
			Image = Properties.Resources.Tile;
		}

		// Sets the state of the cell to Flagged
		public void Flag()
		{
			State = CellState.Flagged;
			Image = Properties.Resources.Flag;
		}

		// Sets the state of the cell to Revealed
		public virtual void Reveal()
		{
			State = CellState.Revealed;
		}


		public void WrongFlag()
        {
			Image = Properties.Resources.WrongFlag;
        }
	}


	public class MineCell : Cell
	{
        public override void Reveal()
        {
            base.Reveal();
			Image = Properties.Resources.Mine;
        }
    }


	public class FreeCell : Cell
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

		public int AdjacentMines { get; }

		public FreeCell(int adjacentMines) : base()
		{
			AdjacentMines = adjacentMines;
		}

        public override void Reveal()
        {
            base.Reveal();
			Image = numberTiles[AdjacentMines];
        }
    }
}