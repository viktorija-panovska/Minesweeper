using System.Windows.Forms;


namespace Minesweeper
{
	public class GameWindow : Form
	{
		readonly int width;
		readonly int height;
		readonly int mines;

		public GameWindow(int width, int height, int mines)
		{


			// Event to shut down the entire program when the window is closed
			FormClosing += new FormClosingEventHandler(GameWindow_FormClosing);
		}

		private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			Application.Exit();
		}
	}
}


