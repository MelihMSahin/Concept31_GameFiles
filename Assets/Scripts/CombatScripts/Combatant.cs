using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    protected string combatantName;

    public TextMeshProUGUI nameTextBox;
    private Animator animator;

	#region lvl variables
	[Space]
    [Header("Level Variables")]
    [SerializeField]
    protected int lvl = 1;
    private int maxLvl = 5;
    [SerializeField]
    protected float experiencePoints = 0f;
	#endregion

	#region Health Variables
	[Space]
    [Header("Health Variables")]
    public Slider healthBar;
    public TextMeshProUGUI healthValueDisplay;
    [SerializeField]
    protected float healthMax = 100f;
    [SerializeField]
    protected float health = 100f;
	#endregion

	#region Empowerment Variables
	[Space]
    [Header("Empowerment Variables")]
    public Slider empowermentBar;
    public enum EmpowermentType { HOLY, CURSE };
    public EmpowermentType empowermentType;
    [SerializeField]
    protected float empowermentValue = 0f;
    private float empowermentMax = 100f;
    private float empowermentStateEntryValue = 80f;
    private float empowermentGainOnTakingDamage = 5f;
    private float empowermentIncreaseOnAttack = 20f;
    private float damageMultiplierOnEmpoweringAttack = 2f;
    protected float empowermentBacklashDamage = 5f;
    [SerializeField]
    private bool isEmpowered = false;
    #endregion

    #region Other Stats
    [Space]
    [Header("Other Stat Variables")]
    [SerializeField]
    protected float attackPower = 20f;
	[SerializeField]
    protected float agility = 10f;
    #endregion

    #region Combat Management Variables
    [Space]
    [Header("Combat Management Variables")]
    protected GameObject combatantData;
    [SerializeField]
    protected bool isAlly = false;
    [SerializeField]
    private bool hasAttacked = false;
    [SerializeField]
    private bool isAlive = true;
    private bool isDead = false; //To stop duplicate deaths
    //public Transform normalPos;
    //public Transform empoweredPos;

    [Space]
    public ParticleSystem particleSystem;
	#endregion

	void Awake()
    {
        RandomiseStats();
    }
    void start(){
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on " + gameObject.name);
        }
    }

	public void StartCombatant()
	{
        health = healthMax;
        
        particleSystem.Stop();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        if (empowermentType == EmpowermentType.HOLY)
        {
            mainModule.startColor = Colour(253, 199, 35); //https://www.color-hex.com/color-palette/95552
        }
        else if (empowermentType == EmpowermentType.CURSE)
        {
            mainModule.startColor = Colour(115, 0, 115); //https://www.color-hex.com/color-palette/83792
        }

        //SetPositionVars();

        //Display the name in combat
        //Display the name in combat
        nameTextBox.text = combatantName;
        healthValueDisplay = healthBar.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void SetPositionVars() //The particleSystem now handles this instead
	{
        //normalPos = new GameObject().transform;
        //empoweredPos = new GameObject().transform;
        //normalPos.position = gameObject.transform.position;
        //empoweredPos.position = gameObject.transform.position + new Vector3(0, 1, 0);
    }

	
	protected virtual void RandomiseStats()
    {
        string[] names = { "Harry", "Ross",
                        "Bruce", "Cook",
                        "Carolyn", "Morgan",
                        "Albert", "Walker",
                        "Randy", "Reed",
                        "Larry", "Barnes",
                        "Lois", "Wilson",
                        "Jesse", "Campbell",
                        "Ernest", "Rogers",
                        "Theresa", "Patterson",
                        "Henry", "Simmons",
                        "Michelle", "Perry",
                        "Frank", "Butler",
                        "Shirley" };
        combatantName = names[Random.Range(0, names.Length)];
        
        int points = 100; //A point system is used to ensure that randomness won't make a combatant that is tanky, high dps and fast.
        /* This is achieved by subtracting from the remaining points everytime we choose a value for a stat. So the greater attack power
         * the combatant has, the less hp they'll have and vice versa
         */

        int attackPoints = Random.Range(10, points/2);
        points -= attackPoints;
        attackPower = attackPoints/5 + 5;
        empowermentBacklashDamage = attackPower;

        int agilityPoints = Random.Range(0, points/2);
        points -= agilityPoints;
        agility = agilityPoints;

        int empowermentPoints = Random.Range(0, Mathf.FloorToInt(points/5));
        points -= empowermentPoints;
        empowermentValue = empowermentPoints * 4;

        healthMax = points;
        #region random empowerment type
        if (Random.Range(0,10) < 5)
		{
            empowermentType = EmpowermentType.CURSE;
		}
        else
		{
            empowermentType = EmpowermentType.HOLY;
		}
        #endregion
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if (combatantData == null)
        {
            combatantData = GameObject.FindGameObjectWithTag("CombatantData");
        }

        if (!isAlive && !isDead)
		{
            isDead = true;
            OnDeath();
        }

		if (healthBar != null)
		{
            healthBar.value = health;
            healthValueDisplay.text = health.ToString();
            empowermentBar.value = empowermentValue;
        }
    }

	#region DeathManagement
    public virtual void OnDeath() //Remove the objext from combat
	{
        DestroyOnDeath();
        GameObject.Destroy(gameObject, 2f);
    }

	protected void DestroyOnDeath() //Remove all associated objects from combat
	{
		if (healthBar != null)
		{
            GameObject.Destroy(healthBar.gameObject, 0.2f);
        }
		if (empowermentBar != null)
		{
            GameObject.Destroy(empowermentBar.gameObject, 0.2f);
        }
		if (nameTextBox != null)
		{
            GameObject.Destroy(nameTextBox.gameObject, 0.2f);
        }
    }
	#endregion

	#region Set Bars
	public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        healthBar.maxValue = healthMax;
    }

    public void SetEmpowermentBar(Slider slider)
	{
        empowermentBar = slider;
        empowermentBar.maxValue = empowermentMax;
        SetEmpowermentBarColour();
	}

    public void SetEmpowermentBarColour()
	{
        Image[] images = empowermentBar.GetComponentsInChildren<Image>();
        Image fill = images[images.Length - 1];
		if (empowermentType == EmpowermentType.HOLY)
		{
            fill.color = Colour(253, 199, 35); //https://www.color-hex.com/color-palette/95552
        }
		else if (empowermentType == EmpowermentType.CURSE)
		{
            fill.color = Colour(115, 0, 115); //https://www.color-hex.com/color-palette/83792
        }
	}

    private Color Colour(float r, float b, float g)
	{
        return new Color(r / 255f, b / 255f, g / 255f);
	}
	#endregion

	#region HealthManagement
	public float Health { get => health; }
    
    public bool TakeDamage(float dmg, float empowermentMultiplier, EmpowermentType attackerType) //Reduces the hp by the incoming damage
	{
        AdjustEmpowermentOnDamageTaken(empowermentMultiplier, attackerType);

        health -= dmg;    
        if (health <= 0)
		{
            //Update the game to show that the character has died
            health = 0;
            isAlive = false;
            return true;
            //ANIMATION
            if (animator != null)
                {
                animator.SetBool("Death", true); // Trigger death animation
                }
		}
        //Update the health bar
        return false;
	}
	#endregion

	#region Empowerment Adjustments
	private void AdjustEmpowermentOnDamageTaken(float empowermentMultiplier, EmpowermentType attackerType) //Adjusts the empowerment value according to the attacker type and combatants empowerment status
	{
        if (isEmpowered)
		{
            AddToEmpowermentValue(-empowermentGainOnTakingDamage * empowermentMultiplier);
		}
		else if (empowermentType == attackerType)
		{
            AddToEmpowermentValue(empowermentGainOnTakingDamage * empowermentMultiplier);
		}
        //The reason that I used else if instaed of else is to make adding more types possible. As not all types will be opposite of each.
		else if (empowermentType == EmpowermentType.HOLY & attackerType == EmpowermentType.CURSE)
		{
            AddToEmpowermentValue(-empowermentGainOnTakingDamage * empowermentMultiplier);
		}
        else if (empowermentType == EmpowermentType.CURSE & attackerType == EmpowermentType.HOLY)
        {
            AddToEmpowermentValue(-empowermentGainOnTakingDamage * empowermentMultiplier);
        }
    }

    private void AddToEmpowermentValue(float add) //Adjusts the empowerment value according to the attacker type and combatants empowerment status
    {
        empowermentValue += add;
		if (empowermentValue > 100f)
		{
            empowermentValue = 100f;
		}
        else if (empowermentValue < 0)
		{
            empowermentValue = 0f;
		}

        isEmpowered = (empowermentValue > empowermentStateEntryValue);
		if (isEmpowered)
		{
            //gameObject.transform.position = empoweredPos.position;
            particleSystem.Play();
		}
		else
		{
            particleSystem.Stop();
            //gameObject.transform.position = normalPos.position;
		}
    }
    #endregion

    public bool BasicAttack(Combatant target)
	{
        BacklashDamage();

        AddToEmpowermentValue(empowermentIncreaseOnAttack);
		
        return target.TakeDamage(DealDmg(), empowermentMultiplier: 1f, empowermentType);
    }

    public bool EmpoweringAttack(Combatant target)
	{
        BacklashDamage();

		if (isEmpowered)
		{
            AddToEmpowermentValue(- damageMultiplierOnEmpoweringAttack * empowermentIncreaseOnAttack); //The empowered attack reduces empowerment if empowered
        }
        else
		{
            AddToEmpowermentValue(damageMultiplierOnEmpoweringAttack * empowermentIncreaseOnAttack);

        }

        return target.TakeDamage((1/damageMultiplierOnEmpoweringAttack) * DealDmg(), empowermentMultiplier: 1f, empowermentType); //Deals reduces damage to the target
	}

    public bool MultiAttack(Combatant target) //A function that attacks a simple combatant, is intended to be called multiple times and deals backlash/empowerment accordingly
	{
        MultiAttackBacklashDamage();

        AddToEmpowermentValue(-empowermentIncreaseOnAttack);

        return target.TakeDamage(DealDmg(), empowermentMultiplier: 2f, empowermentType);
	}

    private void MultiAttackBacklashDamage()
	{
        TakeDamage(empowermentBacklashDamage, empowermentMultiplier: 0, empowermentType);
        if (health < 1)
        {
            health = 1f;
        }
    }

    private void BacklashDamage()
	{
        if (isEmpowered)
        {
            TakeDamage(empowermentBacklashDamage, empowermentMultiplier: 0, empowermentType);
            if (health < 1)
            {
                health = 1f;
            }
        }
    }

    protected float DealDmg()
	{
        //Play the animation, describe what happened in text
        return attackPower;
	}

    public void gainExp()
    {
        //add exp and check if the player leveled up
    }


    public string CombatantName { get => combatantName; set => combatantName = value; }
    public float Agility { get => agility; set => agility = value; }
    public bool HasAttacked { get => hasAttacked; set => hasAttacked = value; }
    public bool IsEmpowered { get => isEmpowered; }
    public bool getisAlly () { return isAlly; }
}