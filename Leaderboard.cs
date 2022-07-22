using System;
using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
    public class Leaderboard : Form
    {
        private ListBox scores;

        private Difficulty difficulty;

        public Leaderboard(Difficulty difficulty)
        {
            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            this.difficulty = difficulty;

            SetDifficultySelect();

            SetScoreOrder();

            SetScores();

            // Event to shut down the entire program when the window is closed
            FormClosing += new FormClosingEventHandler(GameOver_FormClosing);
        }


        private void SetDifficultySelect()
        {
            Button beginner = new Button()
            {
                Location = new Point(20, 20),
                Size = new Size(130, 40),
                Text = "Beginner",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(beginner);
            beginner.MouseClick += new MouseEventHandler(OpenBeginnerList);

            Button intermediate = new Button()
            {
                Location = new Point(165, 20),
                Size = new Size(170, 40),
                Text = "Intermediate",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(intermediate);
            intermediate.MouseClick += new MouseEventHandler(OpenIntermediateList);

            Button expert = new Button()
            {
                Location = new Point(350, 20),
                Size = new Size(130, 40),
                Text = "Expert",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(expert);
            expert.MouseClick += new MouseEventHandler(OpenExpertList);
        }

        private void SetScoreOrder()
        {
            Button chronologicalOrder = new Button()
            {
                Location = new Point(50, 80),
                Size = new Size(180, 40),
                Text = "Chronological",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(chronologicalOrder);
            chronologicalOrder.MouseClick += new MouseEventHandler(ChronologicalSort);

            Button byTimeOrder = new Button()
            {
                Location = new Point(250, 80),
                Size = new Size(180, 40),
                Text = "By Play Time",
                TextAlign = ContentAlignment.TopCenter,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
            };
            Controls.Add(byTimeOrder);
            byTimeOrder.MouseClick += new MouseEventHandler(TimeSort);
        }

        private void SetScores()
        {
            scores = new ListBox()
            {
                Location = new Point(20, 140),
                Size = new Size(460, 450),
                BackColor = Color.Gray
            };
            Controls.Add(scores);

            switch (difficulty.Name)
            {
                case "Beginner":
                    OpenBeginnerList();
                    break;
                case "Intermediate":
                    OpenIntermediateList();
                    break;
                case "Expert":
                    OpenExpertList();
                    break;
            }
        }


        private void GameOver_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }


        private void OpenBeginnerList()
        {

        }
        private void OpenBeginnerList(object sender, MouseEventArgs e)
        {
            OpenBeginnerList();
        }

        private void OpenIntermediateList()
        {

        }
        private void OpenIntermediateList(object sender, MouseEventArgs e)
        {
            OpenIntermediateList();
        }

        private void OpenExpertList()
        {

        }
        private void OpenExpertList(object sender, MouseEventArgs e)
        {
            OpenExpertList();
        }


        private void ChronologicalSort(object sender, MouseEventArgs e)
        {

        }

        private void TimeSort(object sender, MouseEventArgs e)
        {

        }
    }
}
