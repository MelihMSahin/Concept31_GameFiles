using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    private string combatantName;

    public TextMeshProUGUI nameTextBox;

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
    private float empowermentValue = 0f;
    private float empowermentMax = 100f;
    private float empowermentStateEntryValue = 80f;


    private float empowermentGainOnTakingDamage = 5f;

    private float empowermentIncreaseOnAttack = 20f;
    private float damageMultiplierOnEmpoweringAttack = 2f;

    private float empowermentBacklashDamage = 5f;
    [SerializeField]
    private bool isEmpowered = false;
    #endregion

    #region Other Stats
    [Space]
    [Header("Other Stat Variables")]
    [SerializeField]
    protected float attackPower = 20f;
	[SerializeField]
    private float agility = 10f;
	#endregion

	#region Combat Management Variables
	[Space]
    [Header("Combat Management Variables")]
    [SerializeField]
    protected bool isAlly = false;
    [SerializeField]
    private bool hasAttacked = false;
    [SerializeField]
    private bool isAlive = true;
    public Transform normalPos;
    public Transform empoweredPos;
	#endregion

	void Awake()
    {
        RandomiseStats();
    }

	protected void Start()
	{
        health = healthMax;
        SetPositionVars();

        //Display the name in combat
        //Display the name in combat
        nameTextBox.text = combatantName;
	}

    private void SetPositionVars()
	{
        normalPos = new GameObject().transform;
        empoweredPos = new GameObject().transform;
        normalPos.position = gameObject.transform.position;
        empoweredPos.position = gameObject.transform.position + new Vector3(0, 1, 0);
    }

	/* A temporary function to imporve testing.
     * The final product will have this overwritten in the player combatant class to carry over data
     * Random encounter's will use a version that scales with player strength (lvls)
     * Won't be called on Awake so boss encounter's won't be random, instead called by TurnManager of random encounter scenes.
     */
	protected void RandomiseStats()
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
        healthMax = Random.Range(75, 125);
        agility = Random.Range(1, 100);
        attackPower = Random.Range(10, 30);
        empowermentValue = Random.Range(0f, 79f);
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
        if (!isAlive)
		{
            DestroyOnDeath();
        }

        healthBar.value = health;
        empowermentBar.value = empowermentValue;
    }

    private void DestroyOnDeath()
	{
        GameObject.Destroy(healthBar.gameObject, 0.2f);
        GameObject.Destroy(empowermentBar.gameObject, 0.2f);
        GameObject.Destroy(nameTextBox.gameObject, 0.2f);
        GameObject.Destroy(gameObject, 0.2f);
    }

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
    
    public bool TakeDamage(float dmg, float empowermentMultiplier, EmpowermentType attackerType)
	{
        AdjustEmpowermentOnDamageTaken(empowermentMultiplier, attackerType);

        health -= dmg;    
        if (health <= 0)
		{
            //Update the game to show that the character has died
            health = 0;
            isAlive = false;
            return true;
		}
        //Update the health bar
        return false;
	}
	#endregion

	#region Empowerment Adjustments
	private void AdjustEmpowermentOnDamageTaken(float empowermentMultiplier, EmpowermentType attackerType)
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

    private void AddToEmpowermentValue(float add)
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
            gameObject.transform.position = empoweredPos.position;
		}
		else
		{
            gameObject.transform.position = normalPos.position;
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
            AddToEmpowermentValue(- damageMultiplierOnEmpoweringAttack * empowermentIncreaseOnAttack);
        }
        else
		{
            AddToEmpowermentValue(damageMultiplierOnEmpoweringAttack * empowermentIncreaseOnAttack);

        }

        return target.TakeDamage((1/damageMultiplierOnEmpoweringAttack) * DealDmg(), empowermentMultiplier: 1f, empowermentType);
	}

    public bool MultiAttack(Combatant target)
	{
        BacklashDamage();

        AddToEmpowermentValue(-empowermentIncreaseOnAttack);

        return target.TakeDamage(DealDmg(), empowermentMultiplier: 2f, empowermentType);
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
    public bool getisAlly ()
	{
        return isAlly;
	}
}