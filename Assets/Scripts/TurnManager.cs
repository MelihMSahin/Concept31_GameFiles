using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, ALLY1TURN, ALLY2TURN, ALLY3TURN, ENEMY1TURN, ENEMY2TURN, ENEMY3TURN, WON, LOST };
    TurnState turnState;

    public GameObject combatantParent;
    public Dictionary<string, Combatant> combatantDict = new();


    private void Awake()
	{
	    TurnState turnState = TurnState.START;
        combatantParent = this.gameObject;

        //Find and instantiate player characters; below is a temporary solution
        setCombatants();
        //Generate Enemy characters
	}

    private void setCombatants()
	{
        Combatant[] combatantsArray = combatantParent.GetComponentsInChildren<Combatant>();
        
        
        int noOfAllies = 1;
        int noOfEnemies = 1;
        //Summon combatants as childeren
        foreach (Combatant combatant in combatantsArray)
		{
			if (combatant is PlayerCombatant)
			{
				combatantDict.Add("Ally" + noOfAllies.ToString(), combatant);
                noOfAllies += 1;
            }
			else
			{
                combatantDict.Add("Enemy" + noOfEnemies.ToString(), combatant);
                noOfEnemies += 1;
            }
            
		}
    }

	void Start()
    {
        //Introduce Enemiess to the player
        //Decide on whose turn it is
    }

    void FixedUpdate()
    {
		switch (turnState)
		{
            case TurnState.START:
                break;
			case TurnState.ALLY1TURN:
                
				break;
            case TurnState.ALLY2TURN:
                break;
            case TurnState.ALLY3TURN:
                break;
            case TurnState.ENEMY1TURN:
                break;
            case TurnState.ENEMY2TURN:
                break;
            case TurnState.ENEMY3TURN:
                break;
            case TurnState.LOST:
                break;
            case TurnState.WON:
                break;
        }
	}


}
