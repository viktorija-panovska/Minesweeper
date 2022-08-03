using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
    public class GameWonScreen : Form
    {
        public GameWonScreen()
        {
            // Form properties
            BackColor = Color.Gray;
            ClientSize = new Size(300, 280);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;


            Label text = new Label()
            {
                Location = new Point(22, 10),
                Size = new Size(250, 50),
                Text = "VICTORY!",
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(text);


            Button playAgain = new Button()
            {
                Location = new Point(22, 70),
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
                Location = new Point(22, 140),
                Size = new Size(250, 50),
                Text = "LEADERBOARD",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(leaderboard);
            leaderboard.MouseClick += new MouseEventHandler(Leaderboard_OnClick);


            Button playerStats = new Button()
            {
                Location = new Point(22, 210),
                Size = new Size(250, 50),
                Text = "PLAYER STATS",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(playerStats);
            playerStats.MouseClick += new MouseEventHandler(PlayerStats_OnClick);
        }

        private void PlayAgain_OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.Reset();
        }

        private void Leaderboard_OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.ShowLeaderboard();
        }

        private void PlayerStats_OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.ShowPlayerStats();
        }
    }
}
