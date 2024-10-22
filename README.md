# Echo 31 - Concept31_GameFiles
Multi-Platform Game Development Module Concept 3.1 Unity Files
This is a turn-based combat game built using Unity, featuring player and enemy combatants. Players must strategize their moves and defeat enemies to win the game.

Gameplay Overview

In this game, you control a team of combatants to fight against enemy combatants in turn-based battles. The goal is to defeat all enemies while keeping at least one of your allies alive. The combat system revolves around turn order, health management, and strategic use of abilities.

Key Mechanics
____________________________________________________________________________________________________________________________________________

Combatants: Each combatant (player or enemy) has a set of attributes including health, attack power, and agility.
Turn Order: Turns are determined based on combatants' agility. The combatant with the highest agility gets the next turn.
Health: Each combatant has a health bar. When a combatant's health reaches zero, they are removed from battle.
Victory & Defeat:
You win if all enemies are defeated.
You lose if all player-controlled combatants are defeated.
____________________________________________________________________________________________________________________________________________
Installation and Setup
____________________________________________________________________________________________________________________________________________

To run the game locally:

- Clone the Repository: Clone this repository using:
- bash
- Copy code
- git clone <repository_url>

Open with Unity: Open the project in Unity (version 2023.1.13f1 or later recommended).
Scene Setup: Ensure the scene with the TurnManager is set up properly.
Run the Game: Press the Play button in the Unity Editor to start the game.

____________________________________________________________________________________________________________________________________________
How to Play
____________________________________________________________________________________________________________________________________________
Starting the Game:
Upon starting the game, the scene will load with three player-controlled combatants and three enemy combatants.
The game will introduce the player-controlled combatants with a brief message.

____________________________________________________________________________________________________________________________________________
Turn Phases
____________________________________________________________________________________________________________________________________________
Selection Phase: During this phase, the turn order is determined based on the agility of each combatant. The combatant with the highest agility is chosen as the next attacker.
Action Phase: If the next attacker is a player-controlled combatant, you will be prompted to choose an action (attack or use abilities) using the UI buttons.
Enemy Action Phase: If the next attacker is an enemy, they will automatically attack one of your combatants.
Wait Phase: The game pauses for a brief moment while actions resolve.
Turn Resolution: The game checks if either side has won or lost after each turn.

____________________________________________________________________________________________________________________________________________
Controls
____________________________________________________________________________________________________________________________________________
Ability Buttons: Select abilities or attacks for your active combatant.
Target Buttons: Choose which enemy or ally to target.
Next Button: Confirm your selection and proceed with the turn.
UI Elements: The health bars indicate the remaining health of each combatant.

____________________________________________________________________________________________________________________________________________
End of Game
____________________________________________________________________________________________________________________________________________
Victory: If all enemies are defeated, a message saying "Congratulations! You won!" will appear.
Defeat: If all player-controlled combatants are defeated, the message "You lost." will appear.
Restart: To play again, simply restart the scene from the Unity Editor.

____________________________________________________________________________________________________________________________________________
Customization
____________________________________________________________________________________________________________________________________________

Adjusting Combatants: You can adjust the number of enemies or allies by changing the noOfEnemies variable in the TurnManager.
Combatant Stats: Modify the Combatant script to adjust attributes like health, attack power, and agility.
Prefabs: Use allyPrefab and enemyPrefab for adding new character models to the game.

____________________________________________________________________________________________________________________________________________
Troubleshooting!!!
____________________________________________________________________________________________________________________________________________
UI Not Displaying Properly: Make sure all UI elements like sliders and buttons are correctly referenced in the TurnManager.
Combatants Not Spawning: Verify that the positionsParent has all the necessary child positions and that they are properly assigned in the TurnManager.
Game Not Starting: Ensure that endCanvas is disabled initially and the TurnManager script is attached to an active GameObject.
Contributing




