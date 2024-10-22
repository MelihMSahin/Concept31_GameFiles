using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine.UI;

public class Combatant : SerializedMonoBehaviour
{
    [SerializeField]
    private string combatantName;

    [Space]
    [SerializeField]
    protected int lvl = 1;
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

	// Start is called before the first frame update
    void Start()
    {
        //adjust dmg, agility and health according to level(?)
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
        Debug.Log("Combatant");
        //Play the animation, describe what happened in text
        return attackPower;
	}

    private void LevelUp()
	{
        //Increase stats accordingly
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