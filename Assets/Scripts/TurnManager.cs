using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, COMBAT, WON, LOST };
    TurnState turnState;

    public GameObject combatantParent;
    public Combatant[] combatantsArray;

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
        combatantsArray = combatantParent.GetComponentsInChildren<Combatant>();
    }

	

	void Start()
    {
        //Introduce Enemiess to the player
        turnState = TurnState.COMBAT;
    }

    void FixedUpdate()
    { 
        if (Input.GetKeyDown("space"))
		{
            Combatant temp;
            combatantDict.TryGetValue("Ally1", out temp);
            temp.takeDamage(10);
		}

		switch (turnState)
    {
		while (turnState == TurnState.COMBAT)
		{
            Combatant nextAttacker = NextAttacker();
			      //attack()
		}
	}
    public Combatant NextAttacker()
	{
        Combatant nextAttacker = null;
		foreach (Combatant combatant in combatantsArray)
		{
			if (!combatant.HasAttacked)
			{
				if (nextAttacker == null || combatant.Agility > nextAttacker.Agility)
				{
					nextAttacker = combatant;
				}
			}
		}
		if (nextAttacker == null)
		{
			foreach (Combatant combatant in combatantsArray)
			{
				combatant.HasAttacked = false;
			}
		}
		nextAttacker.HasAttacked = true;
		return nextAttacker;
	}
}
