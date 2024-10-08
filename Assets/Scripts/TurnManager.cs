using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TurnManager : SerializedMonoBehaviour
{   
    enum TurnState { START, PLAYERTURN, ALLY1TURN, ALLY2TURN, ENEMY1TURN, ENEMY2TURN, ENEMY3TURN, WON, LOST};

    public GameObject combatantParent;
    public Combatant[] combatants;


    private void Awake()
	{
        combatantParent = this.gameObject;

        TurnState turnState = TurnState.START;
        //Find and instantiate player characters; below is a temporary solution
        setCombatants();
        //Generate Enemy characters
	}

    private void setCombatants()
	{
        combatants = combatantParent.GetComponentsInChildren<Combatant>();

        //Summon combatants as childeren

        foreach (Combatant combatant in combatants)
		{
            combatant.dealDmg();
		}
    }

	void Start()
    {
        //Introduce Enemiess to the player
        //Decide on whose turn it is
    }

    void FixedUpdate()
    {
        
    }


}
