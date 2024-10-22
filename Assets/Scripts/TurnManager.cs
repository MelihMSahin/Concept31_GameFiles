using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class TurnManager : SerializedMonoBehaviour
{
    enum TurnState { START, SELECTION, ENEMYACTION, ACTION, WON, LOST, WAIT };
	[SerializeField]
    TurnState turnState;

	[Space]
	[Header("UI")]
	public Slider[] healthBars;
	public Button[] abilityButtons;
	public Button[] targetButtons;
	public Button nextButton;
	public TextMeshProUGUI turnExplainer;

	[Space]
	[Header("Combatants")]
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
		turnState = TurnState.START;

	}

   

	void Start()
    {
		SetTargetConfirmArray(noOfEnemies);

		DeactivateAbilityButtons();
		DeactivateTargetButtons();
		nextButton.gameObject.SetActive(false);

		setCombatants();

		turnState = TurnState.SELECTION;
		SetButtonNames();
		SetHealthBars();
	}
	private void setCombatants()
	{
		combatantsArray = gameObject.GetComponentsInChildren<Combatant>();
	}

	void FixedUpdate()
	{
		//Select next attacker
		if (turnState == TurnState.SELECTION)
		{
			//Debug.Log("SelectionState");
			DeactivateTargetButtons();
			nextAttacker = NextAttacker();
			turnExplainer.text = "It is " + nextAttacker + "'s turn!";
			turnState = TurnState.ACTION;
		}

		//Attack
		if (turnState == TurnState.ACTION)
		{
			turnState = TurnState.WAIT;
			if (!(nextAttacker is PlayerCombatant)) { StartCoroutine(EnemyAttack()); }
			else 
			{
				turnExplainer.text = "It is your fighter: " + nextAttacker.CombatantName + "'s turn to attack!";
				ActivateAbilityButtons(); 
			}
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

	private void SetHealthBars()
	{
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			combatantsArray[i].SetSlider(healthBars[i]);
		}
	}

	#region Enemy AI
	IEnumerator EnemyAttack()
	{
		//Enemy chooses an attack
		yield return new WaitForSecondsRealtime(2);
		turnState = TurnState.ENEMYACTION;
		Combatant target = FindTargetPlayer();
		turnExplainer.text = nextAttacker.CombatantName + " chooses to attack " + target.CombatantName;
		yield return new WaitForSecondsRealtime(3);

		if (nextAttacker.BasicAttack(target))
		{
			combatantsArray = RemoveCombatantFromArray(target);
		}
		yield return new WaitForSecondsRealtime(2);

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
	private void SetButtonNames()
	{
		int j = 0;
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			if (!(combatantsArray[i] is PlayerCombatant))
			{

				TextMeshProUGUI tmp = targetButtons[j].GetComponentInChildren<TextMeshProUGUI>();
				tmp.text = "Attack: " + combatantsArray[i].CombatantName;
				j += 1;
			}	
		}
	}

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
		turnExplainer.text = nextAttacker.CombatantName + "chooses Basic Attack!";
		DeactivateAbilityButtons();
		ActivateTargetButtons();
	}

	public void OnTarget1ButtonPress()
	{
		StartCoroutine(PlayerTargetAndAttack(1));
	}

	public void OnTarget2ButtonPress()
	{
		StartCoroutine(PlayerTargetAndAttack(2));
	}

	public void OnTarget3ButtonPress()
	{
		StartCoroutine(PlayerTargetAndAttack(3));
	}
	#endregion

	#region Player Target and Attack
	IEnumerator PlayerTargetAndAttack(int targetNo)
	{
		targetNo -= 1;
		Combatant target = FindTargetEnemy(targetNo);
		DeconfirmOtherTargets(targetNo);

		if (targetConfirms[targetNo])
		{
			if (nextAttacker.BasicAttack(target))
			{
				turnExplainer.text = "You killed " + target.CombatantName;
				combatantsArray = RemoveCombatantFromArray(target);
			}
			else { turnExplainer.text = "You attacked " + target.CombatantName; }
			DeactivateTargetButtons();
			yield return new WaitForSecondsRealtime(1);
			nextAttacker.HasAttacked = true;
			DeconfirmOtherTargets(-1);
			RemoveTargetIndicators();
			
		}
		else
		{
			turnExplainer.text = "Confirm " + target.CombatantName + " as target?";
			RemoveTargetIndicators();
			InstantiateIndicator(targetIndicator, target.transform);
			targetConfirms[targetNo] = true;
		}	
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
