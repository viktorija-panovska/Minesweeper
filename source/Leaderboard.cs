using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Minesweeper
{
    public class Leaderboard : Form
    {
        private ListView scoreList;
        private List<Score>[] scores = new List<Score>[3];   // 0 - beginner scores, 1 - intermediate scores, 2 - expert scores
        private int scoresIndex;


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

            SetScoreList();
        }


        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Close();
        }


        // Displays the buttons for selecting the difficulty of scores to show on the leaderboard
        // and sets up the mouse click events
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

        // Displays the list of scores and sets the click events for sorting the list by different paramenters
        private void SetScoreList()
        {
            scoreList = new ListView()
            {
                Location = new Point(20, 100),
                Size = new Size(460, 472),
                View = View.Details,
                GridLines = true,
                Scrollable = true,
                BackColor = Color.Gray,
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point)
            };

            scoreList.Columns.Add("Player Name", 152);
            scoreList.Columns.Add("Date and Time", 152);
            scoreList.Columns.Add("Play Time", 135);

            scoreList.ColumnClick += new ColumnClickEventHandler(ListSort);

            Controls.Add(scoreList);

            OpenList(GameState.Difficulty.Name);
        }


        // Populates the score list with scores of the currently selected difficulty
        private void UpdateScoresDisplay()
        {
            scoreList.Items.Clear();

            foreach (var score in scores[scoresIndex])
                scoreList.Items.Add(new ListViewItem(new string[] { 
                    score.PlayerName, 
                    $"{score.DateTime:dd}:{score.DateTime:MM}:{score.DateTime:yy}  {score.DateTime:HH}:{score.DateTime:mm}", 
                    $"{score.PlayTime / 60}:{score.PlayTime % 60:D2}"
                }));
        }

        // If the scores haven't been read yet, reads scores of the given difficulty from the save file
        private void OpenList(DifficultyName difficulty)
        {
            scoresIndex = (int)difficulty;

            if (scores[scoresIndex] == null)
                scores[scoresIndex] = SaveFileManager.LoadScores(difficulty);

            UpdateScoresDisplay();
        }

        private void OpenList(object sender, MouseEventArgs e, DifficultyName difficulty)
        {
            OpenList(difficulty);
        }


        // Sorts the scores depending on the clicked column
        private void ListSort(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    scores[scoresIndex].Sort((a, b) => a.PlayerName.CompareTo(b.PlayerName));
                    break;
                case 1:
                    scores[scoresIndex].Sort((a, b) => b.DateTime.CompareTo(a.DateTime));
                    break;
                case 2:
                    scores[scoresIndex].Sort((a, b) => a.PlayTime.CompareTo(b.PlayTime));
                    break;
            }

            UpdateScoresDisplay();
        }
    }
}
