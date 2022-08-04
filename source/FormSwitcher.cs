using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
	public class FormSwitcher : Form
	{
		public FormSwitcher()
		{
			// Hide the form the moment it is supposed to be drawn on the screen
			Shown += new EventHandler(OnShown);

			// Initialize the stack for the forms that are to be opened
			GameState.OpenForms = new Stack<Form>();

			// The game starts with the main menu opened
			ShowMainMenu();
		}

		private void OnShown(object sender, EventArgs e)
		{
			Hide();
		}

		private static void ShowMainMenu()
        {
			if (GameState.OpenForms.Count != 0)
				GameState.CurrentForm.Hide();

			GameState.OpenForms.Push(new MainMenu());
			GameState.CurrentForm.Show();
		}

		public static void ShowGameWindow()
		{
			if (GameState.OpenForms.Count != 0)
				GameState.CurrentForm.Hide();

			GameState.OpenForms.Push(new GameWindow());
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

		public static void ShowPlayerStats()
		{
			GameState.OpenForms.Push(new PlayerStats());
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

