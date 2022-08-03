using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public class PlayerStats : Form
    {
        private ListView statsList;
        private List<Score>[] scores;
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

            SetStatsList();
        }

        private void Exit_OnClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void SetStatsList()
        {
            statsList = new ListView()
            {
                Location = new Point(20, 100),
                Size = new Size(460, 475),
                View = View.Details,
                GridLines = true,
                Scrollable = true,
                BackColor = Color.Gray,
                Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point)
            };

            statsList.Columns.Add("Date and Time", 152);
            statsList.Columns.Add("Play Time", 133);
            statsList.Columns.Add("Correctly Marked", 170);

            statsList.ColumnClick += new ColumnClickEventHandler(ListSort);

            Controls.Add(statsList);

            OpenList(GameState.Difficulty.Name);
        }

        private void UpdateScoresDisplay()
        {
            statsList.Items.Clear();

            foreach (var score in scores[scoresIndex])
                statsList.Items.Add(new ListViewItem(new string[] {
                    score.PlayerName,
                    $"{score.DateTime:dd}:{score.DateTime:MM}:{score.DateTime:yy}  {score.DateTime:HH}:{score.DateTime:mm}",
                    $"{score.PlayTime / 60}:{score.PlayTime % 60}"
                }));
        }

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
                    scores[scoresIndex].Sort((a, b) => a.PlayTime.CompareTo(b.PlayTime));
                    break;
            }
        }
    }
}


