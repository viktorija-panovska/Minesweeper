using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public enum DifficultyName { Beginner, Intermediate, Expert };

    public struct Difficulty
    {
        public DifficultyName Name { get; }
        public int BoardWidth { get; }
        public int BoardHeight { get; }
        public int Mines { get; }

        public Difficulty(DifficultyName name, int boardWidth, int boardHeight, int mines)
        {
            Name = name;
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            Mines = mines;
        }
    }

    public static class GameState
    {
        // Forms properties
        public static Stack<Form> OpenForms { get; set; }

        public static Form CurrentForm { get => OpenForms.Peek(); }


        // Player properties
        public static string PlayerName { get; set; }

        public static Difficulty Difficulty { get; set; }
    }
}

