# Software structure:

The program utilizes a Model-View architecture.


The Model:

- **FormSwitcher** - controls the opening and closing of Forms, isn't itself displayed on the screen. This is the Form that is launched first when the program is started.

- **GameState** -  maintains a list of open forms, the currently opened form, the player name and the difficulty of the game. The difficulty of the game is represented by a struct holding the difficulty name, board height and width and number of mines on the board.

- **Cell** - manages the state of the cell and holds the image that should be displayed when the cell is shown on the screen. Has subclasses **MineCell**, for the cells that hold mines, and **FreeCell** for the cells that don't (also contains the number of adjancent mines).

- **Board** - fills the board, randomly generating the positions of the mines, manages revealing all neighboring free cells when a cell is clicked (using the recursive FloodFill algorithm) and flagging cells. Also manages the win and lose conditions of the game, and calls to save the game data. Filling the board and revealing the positions of the mines when the game is lost is implemented by multithreaded programming.

- **Score** - serializable class representing the state of a game. Saves the player name, date and time the game was finished, the length of the game, the difficulty of the game, the number of mines correctly flagged and the total number of mines.

- **SaveFileManager** - saves and loads scores to and from XML files.



The View:

- **MainMenu** - player name and game difficulty (Beginner, Intermediate, Expert) selection screen. Sets these properties in the GameState.

- **GameWindow** - display of the game board with properties set in MainMenu. Keeps a game timer and number of mines left to mark. Handles click events for the tiles on the board.

- **GameOverScreen** - shown when player loses a game (clicks on a tile with a mine). Allows the player to restart the game or view their scores from all games they've played.

- **GameWonScreen** - shown when player wins a game (find all the mines). Allows the player to restart the game, view a leaderboard of won games or view their scores from all games they've played.

- **Leaderboard** - shows a list of all won games with player name, date and time when the game was played and the amount of minutes and seconds it took to complete the game. The player can click on a button to show a list of beginner games, intermediate games or expert games. The games can be sorted by player name, by date and time or by length of game.

- **PlayerStats** - shows a list of all of the games the current players has played with date and time, length of game and amount of mines correctly marked. The player can click on a button to show a list of beginner games, intermediate games or expert games. The games can be sorted by date and time, by length of game (leading with the won games) or by the number of mines marked correctly.