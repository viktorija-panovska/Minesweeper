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
            ClientSize = new Size(300, 200);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            Label text = new Label()
            {
                Location = new Point(22, 40),
                Size = new Size(250, 50),
                Text = "GAME OVER",
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(text);

            Button playAgain = new Button()
            {
                Location = new Point(22, 100),
                Size = new Size(250, 50),
                Text = "PLAY AGAIN?",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 25F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(playAgain);
            playAgain.MouseClick += new MouseEventHandler(OnClick);
        }


        private void OnClick(object sender, MouseEventArgs e)
        {
            FormSwitcher.Reset();
        }
    }
}

