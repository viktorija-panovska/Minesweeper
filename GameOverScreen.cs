using System.Drawing;
using System.Windows.Forms;

namespace Minesweeper
{
    public class GameOverScreen : Form
    {
        public GameOverScreen()
        {
            // Form properties
            BackColor = Color.Gray;
            ClientSize = new Size(300, 220);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            Label text = new Label()
            {
                Location = new Point(22, 20),
                Size = new Size(250, 50),
                Text = "GAME OVER",
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(text);

            Button playAgain = new Button()
            {
                Location = new Point(22, 80),
                Size = new Size(250, 50),
                Text = "PLAY AGAIN?",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(playAgain);
            playAgain.MouseClick += new MouseEventHandler(OnClick);

            Button playerStats = new Button()
            {
                Location = new Point(22, 150),
                Size = new Size(250, 50),
                Text = "PLAYER STATS",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(playerStats);
            playerStats.MouseClick += new MouseEventHandler(PlayerStats_OnClick);
        }


        private void OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.Reset();
        }

        private void PlayerStats_OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.ShowPlayerStats();
        }
    }
}

