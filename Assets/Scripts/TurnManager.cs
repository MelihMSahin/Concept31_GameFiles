using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, SELECTION, ENEMYACTION, ACTION, WON, LOST, WAIT };
	[SerializeField]
    TurnState turnState;

	[Space]
	[Header("UI")]
	public Button[] abilityButtons;
	public Button[] targetButtons;
	public Button nextButton;

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
		nextButton.gameObject.SetActive(false);
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
			//Debug.Log("SelectionState");
			DeactivateTargetButtons();
			nextAttacker = NextAttacker();
			turnState = TurnState.ACTION;
		}

		//Attack
		if (turnState == TurnState.ACTION)
		{
			turnState = TurnState.WAIT;
			if (!(nextAttacker is PlayerCombatant)) { EnemyAttack(); }
			else { ActivateAbilityButtons(); }
		}

		if (nextAttacker.HasAttacked) 
		{
			RemoveTurnIndicators();
			turnState = TurnState.SELECTION; 
		}


		

		if (!CheckIfEnemyLeft())
		{
			turnState = TurnState.WON;
			DeactivateAbilityButtons();
			DeactivateTargetButtons();
			nextAttacker = null;
		}

		if (!CheckIfPlayerLeft())
		{
			turnState = TurnState.LOST;
			DeactivateAbilityButtons();
			DeactivateTargetButtons();
			nextAttacker = null;
		}
	}

	private bool CheckIfPlayerLeft()
	{
		bool isPlayerLeft = false;
		foreach (Combatant combatant in combatantsArray)
		{

			if (combatant is PlayerCombatant)
			{
				isPlayerLeft = true;
			}
		}
		return isPlayerLeft;
	}

	private bool CheckIfEnemyLeft()
	{
		bool isEnemyLeft = false;
		foreach (Combatant combatant in combatantsArray)
		{
			
			if (!(combatant is PlayerCombatant))
			{
				isEnemyLeft = true;
			}
		}
		return isEnemyLeft;
	}

	#region Enemy AI
	public void EnemyAttack()
	{
		//Enemy chooses an attack
		turnState = TurnState.ENEMYACTION;

		Combatant target = FindTargetPlayer();
		if (nextAttacker.BasicAttack(target))
		{
			combatantsArray = RemoveCombatantFromArray(target);
		}

		//Update text to notify the user of what happened.
		nextButton.gameObject.SetActive(true);

	}

	public void FinishEnemyAttack()
	{
		nextButton.gameObject.SetActive(false);
		nextAttacker.HasAttacked = true;
	}

	public Combatant FindTargetPlayer()
	{
		int noOfPlayerCharacters = NoOfPlayerCharacters();
		int targetNo = Random.Range(0, noOfPlayerCharacters - 1);
		
		int count = 0;
		int playerNo = 0;
		while (count < combatantsArray.Length)
		{
			if ((combatantsArray[count] is PlayerCombatant))
			{
				if (playerNo == targetNo)
				{
					return combatantsArray[count];
				}
				playerNo += 1;
			}
			count += 1;
		}
		Debug.LogError("Couldn't find target");
		return combatantsArray[combatantsArray.Length - 1]; //This should never happen
	}

	private int NoOfPlayerCharacters()
	{
		int noOfPlayerCharacters = 0;
		foreach (Combatant combatant in combatantsArray)
		{
			if (combatant is PlayerCombatant)
			{
				noOfPlayerCharacters += 1;
			}
		}
		return noOfPlayerCharacters;
	}

	#endregion

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
		PlayerTargetAndAttack(1);
	}

	public void OnTarget2ButtonPress()
	{
		PlayerTargetAndAttack(2);
	}

	public void OnTarget3ButtonPress()
	{
		PlayerTargetAndAttack(3);
	}
	#endregion

	#region Player Target and Attack
	public void PlayerTargetAndAttack(int targetNo)
	{
		targetNo -= 1;
		Combatant target = FindTargetEnemy(targetNo);
		DeconfirmOtherTargets(targetNo);

		if (targetConfirms[targetNo])
		{
			if (nextAttacker.BasicAttack(target))
			{
				combatantsArray = RemoveCombatantFromArray(target);
			}
			
			nextAttacker.HasAttacked = true;
			DeconfirmOtherTargets(-1);
			RemoveTargetIndicators();
			return;
		}

		RemoveTargetIndicators();
		InstantiateIndicator(targetIndicator, target.transform);
		targetConfirms[targetNo] = true;
	}

	private Combatant[] RemoveCombatantFromArray(Combatant target)
	{
		int temp = -1;
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			if (target.Equals(combatantsArray[i]))
			{
				combatantsArray[i] = null;
				temp = i;
			}
		}
		Combatant[] tempArray = new Combatant[combatantsArray.Length - 1];
		for (int i = 0; i < temp; i++)
		{
			tempArray[i] = combatantsArray[i];
		}
		for (int i = temp; i < combatantsArray.Length - 1; i++)
		{
			tempArray[i] = combatantsArray[i + 1];
		}
		return tempArray;
	}

	public Combatant FindTargetEnemy(int targetNo)
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

	#region NextAttacker
	public Combatant NextAttacker()
	{
		nextAttacker = FindNextAttacker();
		if (nextAttacker == null)
		{
			ResetAllAttackers();
			nextAttacker = FindNextAttacker();
		}
		InstantiateIndicator(turnOrderIndicator, nextAttacker.transform);
		return nextAttacker;
	}

	private Combatant FindNextAttacker()
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

	#region Add and Remove Indicators
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
