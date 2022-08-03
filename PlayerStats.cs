using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public class PlayerStats : Form
    {
        private ListView statsList;
        private List<Score>[] scores = new List<Score>[3];
        private int scoresIndex;

        public PlayerStats()
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

            Label playerName = new Label()
            {
                Location = new Point(0, 10),
                Size = new Size(500, 40),
                Text = GameState.PlayerName,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 30F, FontStyle.Bold, GraphicsUnit.Point),
                ForeColor = Color.Black
            };
            Controls.Add(playerName);

            SetDifficultySelect();
            SetStatsList();
        }

        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Close();
        }



        // Displays the buttons for selecting the difficulty of scores to show on the leaderboard and sets up the 
        // mouse click events
        private void SetDifficultySelect()
        {
            Button beginner = new Button()
            {
                Location = new Point(20, 60),
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
                Location = new Point(165, 60),
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
                Location = new Point(350, 60),
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
        private void SetStatsList()
        {
            statsList = new ListView()
            {
                Location = new Point(20, 110),
                Size = new Size(460, 475),
                View = View.Details,
                GridLines = true,
                Scrollable = true,
                BackColor = Color.Gray,
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point)
            };

            statsList.Columns.Add("Date and Time", 152);
            statsList.Columns.Add("Play Time", 117);
            statsList.Columns.Add("Correctly Marked", 170);

            statsList.ColumnClick += new ColumnClickEventHandler(ListSort);

            Controls.Add(statsList);

            OpenList(GameState.Difficulty.Name);
        }


        // Populates the score list with scores of the currently selected difficulty
        private void UpdateScoresDisplay()
        {
            statsList.Items.Clear();

            foreach (var score in scores[scoresIndex])
            {
                statsList.Items.Add(new ListViewItem(new string[] {
                    $"{score.DateTime:dd}:{score.DateTime:MM}:{score.DateTime:yy}  {score.DateTime:HH}:{score.DateTime:mm}",
                    $"{score.PlayTime / 60}:{score.PlayTime % 60:D2}",
                    $"{score.CorrectlyMarkedMines}/{score.TotalMines}"
                }));

                if (score.CorrectlyMarkedMines == score.TotalMines)
                    statsList.Items[^1].BackColor = Color.Green;
            }
        }


        // If the scores haven't been read yet, reads scores of the given difficulty by the current player
        private void OpenList(DifficultyName difficulty)
        {
            scoresIndex = (int)difficulty;

            if (scores[scoresIndex] == null)
                scores[scoresIndex] = SaveFileManager.LoadScores(difficulty, GameState.PlayerName);

            UpdateScoresDisplay();
        }

        private void OpenList(object sender, MouseEventArgs e, DifficultyName difficulty)
        {
            OpenList(difficulty);
        }


        private void ListSort(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    scores[scoresIndex].Sort((a, b) => a.DateTime.CompareTo(b.DateTime));
                    break;
                case 1:
                    scores[scoresIndex].Sort((a, b) => a.PlayTime.CompareTo(b.PlayTime));
                    break;
                case 2:
                    scores[scoresIndex].Sort((a, b) => b.CorrectlyMarkedMines.CompareTo(a.CorrectlyMarkedMines));
                    break;
            }

            UpdateScoresDisplay();
        }
    }
}


