using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    private string combatantName;

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
    enum EmpowermentType { HOLY, CURSE };
    [SerializeField]
    private EmpowermentType empowermentType;
    private float empowermentValue = 0f;
    private float empowermentMax = 100f; //An issue where empowermentMax is set to 0 at the start of the game exist. Temp solution is to set it to 100f at random. Future solution is to set it at instantiation.
    private float empowermentStateEntryValue = 80f;
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
	#endregion

	public Combatant (int lvl, float healthMax, float attackPower, float agility)
	{
        this.lvl = lvl;
        this.healthMax = healthMax;
        this.attackPower = attackPower;
        this.agility = agility;
	}

	void Awake()
    {
        RandomiseStats();
    }

	protected void Start()
	{
        health = healthMax;
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
        empowermentValue = Random.Range(0f, 100f);
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
        empowermentMax = 100f;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if (!isAlive)
		{
            GameObject.Destroy(healthBar.gameObject, 0.2f);
            GameObject.Destroy(empowermentBar.gameObject, 0.2f);
            GameObject.Destroy(gameObject, 0.2f);
        }

        healthBar.value = health;
        empowermentBar.value = empowermentValue;
    }

    
    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        healthBar.maxValue = healthMax;
    }

    public void SetEmpowermentBar(Slider slider)
	{
        empowermentBar = slider;
        empowermentBar.maxValue = empowermentMax;
        print(empowermentBar.maxValue);
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

    public float Health { get => health; }
    
    public bool TakeDamage(float dmg)
	{
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


    public bool BasicAttack(Combatant target)
	{
        return target.TakeDamage(DealDmg());
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

    public bool getisAlly ()
	{
        return isAlly;
	}
}