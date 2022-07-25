using System.Collections.Generic;
using System.Windows.Forms;

namespace Minesweeper
{
    public static class GameState
    {
        public static Stack<Form> OpenForms { get; set; }

        public static Form CurrentForm { get => OpenForms.Peek(); }

        public static Difficulty Difficulty { get; set; }
    }
}

