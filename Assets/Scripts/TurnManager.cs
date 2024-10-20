using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, SELECTION, ACTION, WON, LOST, WAIT };
	[SerializeField]
    TurnState turnState;
	
	[Space]
	[Header("UI")]
	public Button[] abilityButtons;
	public Button[] targetButtons;

	[Space]
	[Header("Combatants")]
    public GameObject combatantParent;
    public Combatant[] combatantsArray;

	[Space]
	[Header("Indicators")]
	public GameObject turnOrderIndicator;
	public GameObject targetIndicator;

	private Combatant nextAttacker = null;

	private int noOfEnemies = 3;
	private bool[] targetConfirms;


	private void Awake()
	{
		SetTargetConfirmArray(noOfEnemies);

		DeactivateAbilityButtons();
		DeactivateTargetButtons();
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
			DeactivateTargetButtons();
			nextAttacker = NextAttacker();
			turnState = TurnState.ACTION;
		}

		//Attack
		if (turnState == TurnState.ACTION)
		{
			turnState = TurnState.WAIT;
			if (!(nextAttacker is PlayerCombatant)) { }
			else { ActivateAbilityButtons(); }
		}

		if (nextAttacker.HasAttacked) 
		{
			RemoveTurnIndicators();
			turnState = TurnState.SELECTION; 
		}
	}

	#region Button Managers
	private void DeactivateAbilityButtons()
	{
		ToggleSetActiveForButtonArray(false, abilityButtons);
	}

	private void ActivateAbilityButtons()
	{
		ToggleSetActiveForButtonArray(true, abilityButtons);
	}

	private void DeactivateTargetButtons()
	{
		ToggleSetActiveForButtonArray(false, targetButtons);
	}

	private void ActivateTargetButtons()
	{
		ToggleSetActiveForButtonArray(true, targetButtons);
	}

	public void ToggleSetActiveForButtonArray(bool b, Button[] buttonArray)
	{
		foreach (Button button in buttonArray)
		{
			button.gameObject.SetActive(b);
		}
	}
	#endregion

	#region On Button Press
	public void OnBasicAttackButton()
	{
		DeactivateAbilityButtons();
		ActivateTargetButtons();
	}

	public void OnTarget1ButtonPress()
	{
		TargetAndAttack(1);
	}

	public void OnTarget2ButtonPress()
	{
		TargetAndAttack(2);
	}

	public void OnTarget3ButtonPress()
	{
		TargetAndAttack(3);
	}
	#endregion

	#region Target and Attack
	public void TargetAndAttack(int targetNo)
	{
		targetNo -= 1;
		Combatant target = FindTarget(targetNo);
		DeconfirmOtherTargets(targetNo);

		if (targetConfirms[targetNo])
		{
			nextAttacker.BasicAttack(target);
			nextAttacker.HasAttacked = true;
			DeconfirmOtherTargets(-1);
			RemoveTargetIndicators();
			return;
		}

		RemoveTargetIndicators();
		InstantiateIndicator(targetIndicator, target.transform);
		targetConfirms[targetNo] = true;
	}

	public Combatant FindTarget(int targetNo)
	{
		if (targetNo < 0 || targetNo > 2) { Debug.LogError("False targetNo"); }

		int count = 0;
		int enemyNo = 0;
		while (count < combatantsArray.Length)
		{
			if (!(combatantsArray[count] is PlayerCombatant))
			{
				if (enemyNo == targetNo)
				{
					return combatantsArray[count];
				}
				enemyNo += 1;
			}
			count += 1;
		}
		Debug.LogError("Couldn't find target");
		return combatantsArray[combatantsArray.Length - 1];
	}
	
	public void DeconfirmOtherTargets(int currentTarget)
	{
		for (int i = 0; i < targetConfirms.Length; i++)
		{
			if (i != currentTarget)
			{
				targetConfirms[i] = false;
			}
		}
	}

	public void SetTargetConfirmArray(int size)
	{
		targetConfirms = new bool[size];
		for (int i = 0; i < size; i++)
		{
			targetConfirms[i] = false;
		}
	}
	#endregion
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
		InstantiateIndicator(turnOrderIndicator, nextAttacker.transform);
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
	#endregion

	#region Add and remove indicators
	private void InstantiateIndicator(GameObject objectToInstantiate, Transform transform)
	{
		Instantiate(objectToInstantiate, transform);
	}

	private void RemoveTargetIndicators()
	{
		DestroyBasedOnTag("TargetIndicator");
	}

	private void RemoveTurnIndicators()
	{
		DestroyBasedOnTag("TurnIndicator");
	}
	#endregion

	private void DestroyBasedOnTag(string tag)
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		foreach (GameObject o in objects)
		{
			Destroy(o);
		}
	}
}
