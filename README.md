# Echo 31 - Concept31_GameFiles
Multi-Platform Game Development Module Concept 3.1 Unity Files
This is a turn-based combat game built using Unity, featuring player and enemy combatants. Players must strategize their moves and defeat enemies to win the game.

Gameplay Overview

In this game, you control a team of combatants to fight against enemy combatants in turn-based battles. The goal is to defeat all enemies while keeping at least one of your allies alive. The combat system revolves around turn order, health management, and strategic use of abilities.

Key Mechanics
____________________________________________________________________________________________________________________________________________

- Combatants: Each combatant (player or enemy) has a set of attributes including health, attack power, and agility.
- Turn Order: Turns are determined based on combatants' agility. The combatant with the highest agility gets the next turn.
- Health: Each combatant has a health bar. When a combatant's health reaches zero, they are removed from battle.
Victory & Defeat:
- You win if all enemies are defeated.
- You lose if all player-controlled combatants are defeated.
____________________________________________________________________________________________________________________________________________
Installation and Setup: https://pages.github.qmul.ac.uk/ec22884/Concept31_GameBuilds/
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
Upon starting the game, the scene will load to the overworld. Where you can move with the arrow keys or WASD to touch the cube and start the combat. If you are on a touchscreen device, wait for 30 seconds to enter combat.
Combat is with three player-controlled combatants and three enemy combatants. Each with randomly generated stats. Use the buttons to play and follow the instructions on the screen.
Combat loop:
	-Choose basic attack to attack
	-Choose your target
	-Confirm the target by pressing the button again
	-Press next to move on from the enemy turn
Once you win or lose, you can replay the fight by pressing the button. 
____________________________________________________________________________________________________________________________________________
Turn Phases
____________________________________________________________________________________________________________________________________________
- Selection Phase: During this phase, the turn order is determined based on the agility of each combatant. The combatant with the highest agility is chosen as the next attacker.
- Action Phase: If the next attacker is a player-controlled combatant, you will be prompted to choose an action (attack or use abilities) using the UI buttons. Choose and confirm targets with the UI buttons as well.
- Enemy Action Phase: If the next attacker is an enemy, they will automatically attack one of your combatants. Press next to move on to the next action.
- Wait Phase: The game pauses for a brief moment while actions resolve.
- Turn Resolution: The game checks if either side has won or lost after each turn.

____________________________________________________________________________________________________________________________________________
Controls
____________________________________________________________________________________________________________________________________________
- WASD and arrow-keys for movement out of combat.
- Ability Buttons: Select abilities or attacks for your active combatant.
- Target Buttons: Choose which enemy or ally to target.
- Next Button: Confirm your selection and proceed with the turn.
____________________________________________________________________________________________________________________________________________
End of Game
____________________________________________________________________________________________________________________________________________
- Victory: If all enemies are defeated, a message saying "Congratulations! You won!" will appear.
- Defeat: If all player-controlled combatants are defeated, the message "You lost." will appear.
- Restart: To play again, simply restart the scene from the Unity Editor.

____________________________________________________________________________________________________________________________________________
Customization
____________________________________________________________________________________________________________________________________________

- Combatant Stats: Modify the Combatant script to adjust attributes like health, attack power, and agility.
- Prefabs: Use allyPrefab and enemyPrefab for adding new character models to the game.

____________________________________________________________________________________________________________________________________________
Troubleshooting!!!
____________________________________________________________________________________________________________________________________________
- Make sure all UI elements like sliders and buttons are correctly referenced in the TurnManager.
- Combatants Not Spawning: Verify that the positionsParent has all the necessary child positions and that they are properly assigned in the TurnManager.




