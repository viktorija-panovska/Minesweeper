using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Minesweeper
{
    public class Leaderboard : Form
    {
        private ListBox scoresDisplay;
        private List<HighScore>[] scores;
        private int scoresIndex;

        public Leaderboard()
        {
            // Form properties
            Name = "Minesweeper";
            BackColor = Color.Gray;
            ClientSize = new Size(500, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;

            scores = new List<HighScore>[3];

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
            beginner.MouseClick += new MouseEventHandler((sender, e) => OpenList(sender, e, DifficultyName.Beginner));

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
            intermediate.MouseClick += new MouseEventHandler((sender, e) => OpenList(sender, e, DifficultyName.Intermediate));

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
            expert.MouseClick += new MouseEventHandler((sender, e) => OpenList(sender, e, DifficultyName.Expert));
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
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point)
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
            scoresDisplay = new ListBox()
            {
                Location = new Point(20, 160),
                Size = new Size(460, 430),
                BackColor = Color.Gray,
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point),
                SelectionMode = SelectionMode.None
            };
            Controls.Add(scoresDisplay);

            OpenList(GameState.Difficulty.Name);
        }


        private void UpdateScoresDisplay()
        {
            scoresDisplay.Items.Clear();

            scoresDisplay.Items.Add($"{"DATE",18} {"PLAY TIME",42}");

            foreach (var score in scores[scoresIndex])
                scoresDisplay.Items.Add($"{score.DateTime.Day:D2}:{score.DateTime.Month:D2}:{score.DateTime.Year:D4}  " +
                    $"{score.DateTime.Hour:D2}:{score.DateTime.Minute:D2}:{score.DateTime.Second:D2}" +
                    $"{score.PlayTime + "s",30}");
        }

        private void OpenList(DifficultyName difficulty)
        {
            scoresIndex = (int)difficulty;

            if (scores[scoresIndex] == null)
                scores[scoresIndex] = ScoreManager.LoadScores(difficulty);

            UpdateScoresDisplay();
        }

        private void OpenList(object sender, MouseEventArgs e, DifficultyName difficulty)
        {
            OpenList(difficulty);
        }


        private void ChronologicalSort(object sender, MouseEventArgs e)
        {
            scores[scoresIndex].Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
            UpdateScoresDisplay();
        }

        private void TimeSort(object sender, MouseEventArgs e)
        {
            scores[scoresIndex].Sort((a, b) => a.PlayTime.CompareTo(b.PlayTime));
            UpdateScoresDisplay();
        }
    }
}
