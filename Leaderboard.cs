using System;
using System.Drawing;
using System.Windows.Forms;


namespace Minesweeper
{
    public class Leaderboard : Form
    {
        private ListBox scores;

        public Leaderboard()
        {
            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            Button exit = new Button()
            {
                Location = new Point(ClientSize.Width - 40, 10),
                Size = new Size(30, 30),
                Text = "X",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point),
                BackColor = Color.Red
            };
            Controls.Add(exit);
            exit.MouseClick += new MouseEventHandler(Exit_OnClick);

            SetDifficultySelect();

            SetScoreOrder();

            SetScores();
        }


        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Close();
        }


        private void SetDifficultySelect()
        {
            Button beginner = new Button()
            {
                Location = new Point(20, 45),
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
                Location = new Point(165, 45),
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
                Location = new Point(350, 45),
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
                Location = new Point(50, 100),
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
                Location = new Point(250, 100),
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
                Location = new Point(20, 160),
                Size = new Size(460, 430),
                BackColor = Color.Gray
            };
            Controls.Add(scores);

            switch (GameState.Difficulty.Name)
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


        private void OpenBeginnerList()
        {
            System.Diagnostics.Debug.WriteLine("Beginner");
        }
        private void OpenBeginnerList(object sender, MouseEventArgs e)
        {
            OpenBeginnerList();
        }

        private void OpenIntermediateList()
        {
            System.Diagnostics.Debug.WriteLine("Intermediate");
        }
        private void OpenIntermediateList(object sender, MouseEventArgs e)
        {
            OpenIntermediateList();
        }

        private void OpenExpertList()
        {
            System.Diagnostics.Debug.WriteLine("Expert");
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
