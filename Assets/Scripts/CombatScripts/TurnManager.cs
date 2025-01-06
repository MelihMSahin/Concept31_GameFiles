using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
	#region enum START, SELECTION, ENEMYACTION, ACTION, WON, LOST, WAIT
	enum TurnState { START, SELECTION, ENEMYACTION, ACTION, WON, LOST, WAIT };
	[SerializeField]
    TurnState turnState;
	#endregion

	#region UI Variables
	[Space]
	[Header("UI")]
	public TextMeshProUGUI[] combatantNameTextBoxes;
	public Slider[] healthBars;
	public Slider[] empowermentBars;
	public Button[] abilityButtons;
	public Button[] targetButtons;
	public Button nextButton;
	public TextMeshProUGUI turnExplainer;
	public Canvas canvas;
	public Canvas endCanvas;
	public TextMeshProUGUI endText;
	#endregion

	#region Combatant Variables
	[Space]
	[Header("Combatants")]
	public GameObject positionsParent;
	public Transform[] positionsArray;
    public Combatant[] combatantsArray;
	public GameObject allyPrefab;
	public GameObject enemyPrefab;
	public float empoweredAttackChance = 5f;
	public PlayerCombatant[] playerCombatantDataArray;
	#endregion

	#region Indicators
	[Space]
	[Header("Indicators")]
	public GameObject turnOrderIndicator;
	public GameObject targetIndicator;
	#endregion

	#region Additional Combat Management Variables
	private Combatant nextAttacker = null;
	private int noOfEnemies = 3;
	private bool[] targetConfirms;
	enum AttackType { BASIC, EMPOWERING, MULTI }
	AttackType attackType;
	#endregion

	#region Setting Up the Combat
	private void Awake()
	{
		endCanvas.gameObject.SetActive(false);
		turnState = TurnState.START;

		playerCombatantDataArray = (GameObject.FindGameObjectWithTag("CombatantData")).GetComponentsInChildren<PlayerCombatant>();

		InstantiateCombatants();
		nextAttacker = null;
	}



	private void InstantiateCombatants()
	{
		positionsArray = positionsParent.GetComponentsInChildren<Transform>();

		

		int j = 0;
		foreach (PlayerCombatant combatant in playerCombatantDataArray)
		{
			combatant.transform.parent = gameObject.transform;
			combatant.transform.position = positionsArray[j + 1].position;
			j += 1;
			combatant.enabled = true;
		}
		
		for (int i = 3; i < 6; i++)
		{
			GameObject temp = Instantiate(enemyPrefab, gameObject.transform);
			temp.transform.position = positionsArray[i+1].position;	
		}
	}

	void Start()
    {
		SetTargetConfirmArray(noOfEnemies);

		DeactivateAbilityButtons();
		DeactivateTargetButtons();
		nextButton.gameObject.SetActive(false);

		setCombatants();

		SetButtonNames();
		SetSliders();
		StartCoroutine(Introduction());

		SetCombatantNameText();
	}


	public void SetCombatantNameText()
	{
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			combatantsArray[i].nameTextBox = combatantNameTextBoxes[i];
			combatantsArray[i].StartCombatant();
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

	private void setCombatants()
	{
		combatantsArray = gameObject.GetComponentsInChildren<Combatant>();
	}

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

	private void SetSliders()
	{
		SetHealthBars();
		SetEmpowermentBars();
	}
	private void SetHealthBars()
	{
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			combatantsArray[i].SetHealthBar(healthBars[i]);
		}
	}

	private void SetEmpowermentBars()
	{
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			combatantsArray[i].SetEmpowermentBar(empowermentBars[i]);
		}
	}

	IEnumerator Introduction()
	{
		string temp = "Your allies are; " + combatantsArray[0].CombatantName + ", " + combatantsArray[1].CombatantName + " and " + combatantsArray[2].CombatantName + ". Good luck!";
		turnExplainer.text = temp;
		yield return new WaitForSecondsRealtime(5);
		turnState = TurnState.SELECTION;
	}
	#endregion

	void FixedUpdate()
	{
		#region Combat Loop
		//Select next attacker
		if (turnState == TurnState.SELECTION) //Find the next attacker
		{
			//Debug.Log("SelectionState");
			DeactivateTargetButtons();
			nextAttacker = NextAttacker();
			turnExplainer.text = "It is " + nextAttacker.CombatantName + "'s turn!";
			turnState = TurnState.ACTION;
		}

		//Attack
		if (turnState == TurnState.ACTION)
		{
			turnState = TurnState.WAIT;
			if (!(nextAttacker is PlayerCombatant)) { StartCoroutine(EnemyAttack()); } //If player is not the next attacker, start ai
			else 
			{
				turnExplainer.text = "It is your fighter: " + nextAttacker.CombatantName + "'s turn to attack!";
				ActivateAbilityButtons(); //Otherwise activate buttons for player to attack
			}
		}

		if (turnState == TurnState.ENEMYACTION || turnState == TurnState.ACTION || turnState == TurnState.WAIT)  //Wait for the attack to finish and end turn (reset to selection)
		{
			if (nextAttacker.HasAttacked)
			{
				RemoveTurnIndicators();
				turnState = TurnState.SELECTION;
			}
		}
		#endregion

		#region Game Won/Lost
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

		if (turnState == TurnState.WON)
		{
			canvas.gameObject.SetActive(false);
			endCanvas.gameObject.SetActive(true);
			SaveCombatants();
			endText.text = "Congratulations! You won!";
			StartCoroutine("ExitCombat");
		}

		if (turnState == TurnState.LOST)
		{
			canvas.gameObject.SetActive(false);
			endCanvas.gameObject.SetActive(true);
			endText.text = "You lost.";
			StartCoroutine("ExitCombat");
		}
		#endregion
	}

	#region Game Won/Lost
	IEnumerator ExitCombat()
	{
		yield return new WaitForSecondsRealtime(2f);
		SceneManager.LoadScene("Out-Of-Combat");
	}
	private void SaveCombatants() //To allow the stats of player combatants to keep existing until the end of the game.
	{
		foreach (Combatant combatant in combatantsArray)
		{
			if (combatant is PlayerCombatant)
			{
				combatant.OnDeath();
			}
		}
	}

	private bool CheckIfPlayerLeft() //Checks if player has lost
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

	private bool CheckIfEnemyLeft() //Checks if enemy has lost
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
	#endregion


	#region Enemy AI
	IEnumerator EnemyAttack() //Starts the enemy's attack
	{
		//Enemy chooses an attack
		yield return new WaitForSecondsRealtime(1);
		turnState = TurnState.ENEMYACTION;

		//If enemy can, they do a multi attack to hit all players
		#region MultiAttack 
		if (nextAttacker.IsEmpowered)
		{
			foreach (Combatant combatant in combatantsArray)
			{
				if (combatant is PlayerCombatant)
				{
					if (nextAttacker.MultiAttack(combatant))
					{
						combatantsArray = RemoveCombatantFromArray(combatant);
					}
				}
			}
		}
		#endregion
		
		else
		{
			Combatant target = FindTargetPlayer();
			turnExplainer.text = nextAttacker.CombatantName + " chooses to attack " + target.CombatantName;
			yield return new WaitForSecondsRealtime(2);


			if (Random.Range(0, 10) > empoweredAttackChance) //Random chance for the enemy to do an empowered attack
			{
				if (nextAttacker.EmpoweringAttack(target))
				{
					combatantsArray = RemoveCombatantFromArray(target);
				}
			}
			else if (nextAttacker.BasicAttack(target))
			{
				combatantsArray = RemoveCombatantFromArray(target);
			}
		}

		yield return new WaitForSecondsRealtime(1);

		//Update text to notify the user of what happened.
		nextButton.gameObject.SetActive(true);
	}

	public void FinishEnemyAttack() //allows the player to end enemies attack
	{
		nextButton.gameObject.SetActive(false);
		nextAttacker.HasAttacked = true;
	}

	public Combatant FindTargetPlayer() //The enemy picks a player as target and returns to player
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

	private int NoOfPlayerCharacters() //A simple function to return the number of player combatants left
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
		//If Empowered, activate multi. If not deactivate multi.
		ToggleMultiButton(abilityButtons[abilityButtons.Length - 1]);
	}

	private void ToggleMultiButton(Button button)
	{
		if (!nextAttacker.IsEmpowered)
		{
			button.gameObject.SetActive(false);
		}
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

	public void RemoveTargetButton(int targetNo)
	{
		Button[] temp = new Button[targetButtons.Length - 1];
		int j = 0;
		for (int i = 0; i < targetButtons.Length; i++)
		{
			if (targetNo != i)
			{
				temp[j] = targetButtons[i];
				j += 1;
			}
		}
		targetButtons = temp;
	}
	#endregion

	#region On Button Press
	public void OnBasicAttackButton()
	{
		attackType = AttackType.BASIC;
		turnExplainer.text = nextAttacker.CombatantName + "chooses Basic Attack!";
		DeactivateAbilityButtons();
		ActivateTargetButtons();
	}

	public void OnEmpowermentAttackButton()
	{
		attackType = AttackType.EMPOWERING;
		turnExplainer.text = nextAttacker.CombatantName + "chooses Empowering Attack!";
		DeactivateAbilityButtons();
		ActivateTargetButtons();
	}

	public void OnMultiAttackButton()
	{
		attackType = AttackType.MULTI;
		turnExplainer.text = nextAttacker.CombatantName + "chooses MULTI Attack!";
		DeactivateAbilityButtons();
		StartCoroutine(PlayerMultiAttack());
	}

	public void OnTarget1ButtonPress()
	{
		StartCoroutine(PlayerTargetAndAttack(0));
	}

	public void OnTarget2ButtonPress()
	{
		int targetNo = 1;
		int noOfEnemyLeft = NoOfEnemyCharacters();
		if (noOfEnemyLeft < 3)
		{
			targetNo = 0;
		}
		StartCoroutine(PlayerTargetAndAttack(targetNo));
	}

	public void OnTarget3ButtonPress()
	{
		int targetNo = 2;
		int noOfEnemyLeft = NoOfEnemyCharacters();
		if (noOfEnemyLeft == 1)
		{
			targetNo = 0;
		}
		else if (noOfEnemyLeft == 2)
		{
			targetNo = 1;
		}
		StartCoroutine(PlayerTargetAndAttack(targetNo));
	}
	#endregion

	#region Player Target and Attack
	IEnumerator PlayerMultiAttack() //Allows player to multi attack by attacking each enemy one by one
	{
		turnExplainer.text = "You used your ultimate move!";
		string deadNames = "";
		bool hasKilled = false;
		yield return new WaitForSecondsRealtime(0.3f);
		foreach (Combatant combatant in combatantsArray)
		{
			if (!(combatant is PlayerCombatant))
			{
				if (Attack(combatant))
				{
					hasKilled = true;
					deadNames += combatant.name;
					combatantsArray = RemoveCombatantFromArray(combatant);
				}
			}
		}
		if (hasKilled)
		{
			turnExplainer.text = "You killed: " + deadNames + ".";
		}
		else
		{
			turnExplainer.text = "You attacked everyone!";
		}
		yield return new WaitForSecondsRealtime(1);
		nextAttacker.HasAttacked = true;
	}

	IEnumerator PlayerTargetAndAttack(int targetNo) //Master function for player attacks
	{
		Combatant target = FindTargetEnemy(targetNo);
		DeconfirmOtherTargets(targetNo);

		if (targetConfirms[targetNo])
		{
			if (Attack(target)) //If enemy dies remove them from the game
			{
				turnExplainer.text = "You killed " + target.CombatantName;
				DeactivateTargetButtons();
				RemoveTargetButton(targetNo);
				combatantsArray = RemoveCombatantFromArray(target);
			}
			else { turnExplainer.text = "You attacked " + target.CombatantName; }

			//Turn of the players attacking ability
			DeactivateTargetButtons();
			yield return new WaitForSecondsRealtime(0.5f);
			nextAttacker.HasAttacked = true;
			DeconfirmOtherTargets(-1);
			RemoveTargetIndicators();
		}
		else //Allow the player to switch targets
		{
			turnExplainer.text = "Confirm " + target.CombatantName + " as target?";
			RemoveTargetIndicators();
			InstantiateIndicator(targetIndicator, target.transform);
			targetConfirms[targetNo] = true;
		}	
	}

	private bool Attack(Combatant target) //Calls the correct attack function according to the enum
	{
		if (attackType == AttackType.BASIC)
		{
			return nextAttacker.BasicAttack(target);
		}
		else if (attackType == AttackType.EMPOWERING)
		{
			return nextAttacker.EmpoweringAttack(target);
		}
		else if (attackType == AttackType.MULTI)
		{
			return nextAttacker.MultiAttack(target);
		}
		else
		{
			return false;
		}
		
	}

	private Combatant[] RemoveCombatantFromArray(Combatant target) //when the combatant dies, it is removed from the game with all it's helpers (like health bar).
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

	public Combatant FindTargetEnemy(int targetNo) //Returns the targeted enemy combatant
	{
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

	private int NoOfEnemyCharacters()
	{
		int total = 0;
		for (int i = 0; i < combatantsArray.Length; i++)
		{
			if (!(combatantsArray[i] is PlayerCombatant))
			{
				total += 1;
			}
		}
		return total;
	}
	
	public void DeconfirmOtherTargets(int currentTarget) //Allows player to retarget
	{
		for (int i = 0; i < targetConfirms.Length; i++)
		{
			if (i != currentTarget)
			{
				targetConfirms[i] = false;
			}
		}
	}
	#endregion

	#region NextAttacker
	public Combatant NextAttacker() //Finds the enxt attacker and indicates it to the player
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

	private Combatant FindNextAttacker() //Finds the next attacker by finding the highest agility combatant that hasn't attacked this turn
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

	private void ResetAllAttackers() //when the turn ends, reset everyones has attacked status
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
