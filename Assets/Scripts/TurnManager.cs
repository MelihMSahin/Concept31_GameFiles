using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UIElements;

public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, SELECTION, ACTION, WON, LOST, WAIT };
    TurnState turnState;

    public GameObject combatantParent;
    public Combatant[] combatantsArray;
	public GameObject turnOrderIndicator;

	private Combatant nextAttacker = null;

    private void Awake()
	{
	    turnState = TurnState.START;
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
        turnState = TurnState.SELECTION;
    }

	void FixedUpdate()
	{
		//Select next attacker
		if (turnState == TurnState.SELECTION)
		{
			nextAttacker = NextAttacker();
			turnState = TurnState.ACTION;
		}

		//Attack
		if (turnState == TurnState.ACTION)
		{
			turnState = TurnState.WAIT;
			if (!(nextAttacker is PlayerCombatant))
			{
				nextAttacker.Attack();
			}

			//Attack, at the end remove the indicator and set hasAttacked to true
			if (nextAttacker.HasAttacked)
			{
				turnState = TurnState.SELECTION;
			}
		}
	}

	public void OnBasicAttackButton()
	{
		if (!(turnState == TurnState.ACTION && nextAttacker is PlayerCombatant)) { Debug.Log("not player"); return; }
		Debug.Log(nextAttacker.transform.position);
	}

	IEnumerator wait(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		turnState = TurnState.SELECTION;
	}

	#region NextAttacker
	public Combatant NextAttacker()
	{
		nextAttacker = findNextAttacker();
		if (nextAttacker == null)
		{
			ResetAllAttackers();
			nextAttacker = findNextAttacker();
		}
		AttackerIndicator();
		return nextAttacker;
	}

	private Combatant findNextAttacker()
	{
		nextAttacker = null;
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
		return nextAttacker;
	}

	private void ResetAllAttackers()
	{
		foreach (Combatant combatant in combatantsArray)
		{
			combatant.HasAttacked = false;
		}
	}

	private void AttackerIndicator()
	{
		Instantiate(turnOrderIndicator, nextAttacker.transform);
	}
	#endregion
}
