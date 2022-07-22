using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
    public class GameWonScreen : Form
    {
        private Difficulty difficulty;

        public GameWonScreen(Difficulty difficulty)
        {
            this.difficulty = difficulty;

            // Form properties
            BackColor = Color.Gray;
            ClientSize = new Size(300, 250);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            Label text = new Label()
            {
                Location = new Point(22, 40),
                Size = new Size(250, 50),
                Text = "VICTORY!",
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(text);

            Button playAgain = new Button()
            {
                Location = new Point(22, 100),
                Size = new Size(250, 50),
                Text = "PLAY AGAIN",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(playAgain);
            playAgain.MouseClick += new MouseEventHandler(PlayAgain_OnClick);


            Button leaderboard = new Button()
            {
                Location = new Point(22, 170),
                Size = new Size(250, 50),
                Text = "LEADERBOARD",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(leaderboard);
            leaderboard.MouseClick += new MouseEventHandler(Leaderboard_OnClick);
        }

        private void PlayAgain_OnClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void Leaderboard_OnClick(object sender, MouseEventArgs e)
        {
            Leaderboard leaderboard = new Leaderboard(difficulty);
            leaderboard.Show();
            Hide();
        }
    }
}
