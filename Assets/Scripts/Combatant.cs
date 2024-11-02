using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Combatant : MonoBehaviour
{
    [SerializeField]
    private string combatantName;

    [Space]
    [SerializeField]
    protected int lvl = 1;
    private int maxLvl = 5;
    [SerializeField]
    protected float experiencePoints = 0f;

    [Space]
    [Header("Health")]
    public Slider healthBar;
    [SerializeField]
    protected float healthMax = 100f;
    [SerializeField]
    protected float health = 100f;

    [Space]
    [SerializeField]
    protected float attackPower = 20f;
	[SerializeField]
    private float agility = 10f;
    [SerializeField]
    protected bool isAlly = false;
    [SerializeField]
    private bool hasAttacked = false;
    [SerializeField]
    private bool isAlive = true;

    public Combatant (int lvl, float healthMax, float attackPower, float agility)
	{
        this.lvl = lvl;
        this.healthMax = HealthMultiplierFromLevels(healthMax);
        this.attackPower = attackPower;
        this.agility = agility;
	}

	void Awake()
    {

    }

	protected void Start()
	{
        health = healthMax;
	}
	/* A function to imporve the prototype.
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
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAlive)
		{
            GameObject.Destroy(gameObject, 0.2f);
        }

        healthBar.value = health;
    }

    public void SetSlider(Slider slider)
	{
        healthBar = slider;
        healthBar.maxValue = healthMax;
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
        return DamageMultiplierFromLevels();
	}

    private float DamageMultiplierFromLevels()
	{
        /*The goal of this method is the increase the attack power according to the level.
         * This function grows faster on lower levels, and reaches exactly maxLevelDamageMultiplier times the damage on max lvl.
         */

        int maxLevelDamageMultiplier = 3;
        return attackPower * (float) Math.Exp((Math.Log(maxLevelDamageMultiplier)/maxLvl)*(lvl-1));
	}

    private float HealthMultiplierFromLevels(float maxHealth)
	{
        /*The goal of this method is the increase the maxHealth according to the level.
         * This function grows faster on lower levels, and reaches exactly maxLevelHealthMultiplier times the maxHealth on max lvl.
         */

        int maxLevelHealthMultiplier = 2;
        return MathF.Round(healthMax * (float) Math.Exp((Math.Log(maxLevelHealthMultiplier) / maxLvl) * (lvl - 1)));
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