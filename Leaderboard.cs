using System.Drawing;
using System.Windows.Forms;


public class Leaderboard : Form
{
    readonly Label text;

    public Leaderboard(int timeElapsed)
    {
        text = new Label()
        {
            Location = new Point(75, 10),
            Size = new Size(350, 50),
            Text = "VICTORY!",
            TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.Black
        };




        // Form properties
        Name = "Minesweeper";
        BackColor = Color.Gray;
        ClientSize = new Size(500, 500);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Controls.Add(text);

        // Event to shut down the entire program when the window is closed
        FormClosing += new FormClosingEventHandler(GameOver_FormClosing);
    }

    
    private void GameOver_FormClosing(object sender, FormClosingEventArgs e)
    {
        Application.Exit();
    }
}

