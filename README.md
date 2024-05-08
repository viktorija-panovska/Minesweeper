# Running the project

To launch the project, open the command line and navigate to the project directory. 

First, the project must be built. Execute the command:

    >dotnet build

Then, to launch the game, execute the command:
    
    >dotnet run



# Playing the game

The player will first encounter the main menu screen where they will need to enter their name (which can contain up to 15 characters and cannot contain any invalid characters) and to chose a diffculty (Beginner, Intermediate, Expert). Then they can start the game.

Upon starting the game, the player will be shown a grid of blank tiles representing the board that they can interact with by either left-clicking to reveal a tile or right-clicking to flag a tile and make it unclickable (right-click again on the tile to remove the flag). Some of these tiles hide mines, and revealing a tile with a mine will end the game. The goal of the game is to find and flag all tiles that hide mines. The player can also see a timer, keeping track of the time taken to solve the game, and a count of the number of mines remaining to be flagged (does not guarantee that the tiles that have already been flagged are correct). 

Clicking on a tile that doesn't hide a mine will reveal that tile. If any of the tiles surrounding the newly revealed tile hide a mine, the tile will show a number, denoting the number of mines surrounding it. If there are no mines surrounding the tile, then clicking the tile will reveal all tiles around it that also don't neighbor any mines, until it finds tiles that do neighbor mines. Using these numbers, the player can discover where the mines are hidden.

If the game is lost, the player is presented with a screen prompting them to either try again or view their statistics. Their statistics include a list of all the games played under the name entered at the beginning of the program, seperated by difficulty. The list can be sorted by date and time played, length of game and number of correctly flagged tiles, by clicking on the column headers. To return to the game over screen, click the red X button.

If the game is won, the player is presented with a screen prompting them to either play again, view a leaderboard of scores or view their statistics. The statistics screen is described above. The leaderboard is a list of won games by all players, seperated by difficulty. The list can be sorted by player name, date and time played and length of game, by clicking on the column headers. To return to the game over screen, click the red X button.

To exit the game, click on the red X button in the main menu or on the game screen
