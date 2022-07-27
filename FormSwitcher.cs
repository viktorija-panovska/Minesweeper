using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
	public class FormSwitcher : Form
	{
		public FormSwitcher()
		{
			Shown += new EventHandler(OnShown);

			// Initialize the empty stack of forms
			GameState.OpenForms = new Stack<Form>();

			ShowMainMenu();
		}

		private void OnShown(object sender, EventArgs e)
		{
			Hide();
		}

		private static void ShowMainMenu()
        {
			GameState.OpenForms.Push(new MainMenu());
			GameState.CurrentForm.Show();
		}

		public static void ShowGameWindow()
		{ 
			GameState.CurrentForm.Hide();
			GameState.OpenForms.Push(new GameWindow(GameState.Difficulty));
			GameState.CurrentForm.Show();
		}

		public static void ShowGameOver()
        {
			GameState.OpenForms.Push(new GameOverScreen());
			GameState.CurrentForm.Show();			
		}

		public static void ShowGameWon()
		{
			GameState.OpenForms.Push(new GameWonScreen());
			GameState.CurrentForm.Show();
		}

		public static void ShowLeaderboard()
        {
			GameState.OpenForms.Push(new Leaderboard());
			GameState.CurrentForm.Show();
		}

		// Closes all forms except the Main Menu, which will be reused
		public static void Reset()
        {
			while (GameState.OpenForms.Count > 1)
				GameState.OpenForms.Pop().Close();

			GameState.CurrentForm.Show();
		}
	}
}

