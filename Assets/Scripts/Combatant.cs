using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class Combatant : SerializedMonoBehaviour
{

    public string combatantName = "Dave";
    [SerializeField]
    protected int lvl = 1;
    [SerializeField]
    protected float experiencePoints = 0f;
    [SerializeField]
    protected float healthMax = 100f;
    [SerializeField]
    protected float health = 100f;
    [SerializeField]
    protected float attackPower = 20f;
	[SerializeField]
    private float agility = 10f;
    [SerializeField]
    protected bool isAlly = false;
    [SerializeField]
    private bool hasAttacked = false;

	// Start is called before the first frame update
    void Start()
    {
        //adjust dmg, agility and health according to level(?)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }


    public float Health { get => health; }
    
    public bool TakeDamage(float dmg)
	{
        health -= dmg;    
        if (health <= 0)
		{
            //Update the game to show that the character has died
            health = 0;
            return true;
		}
        //Update the health bar
        return false;
	}


    public virtual bool BasicAttack(Combatant target)
	{

        return false;
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

    public float Agility { get => agility; set => agility = value; }
    public bool HasAttacked { get => hasAttacked; set => hasAttacked = value; }

    public bool getisAlly ()
	{
        return isAlly;
	}
}