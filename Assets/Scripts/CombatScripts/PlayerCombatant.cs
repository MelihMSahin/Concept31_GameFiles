using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{
	private bool isNew = true;

	private void Awake()
	{
		isAlly = true; //indicates that it is a player combatant
		if (isNew)
		{
			RandomiseStats();
		}
		gameObject.GetComponent<PlayerCombatant>().enabled = false;
	}

    protected override void RandomiseStats() //Same as the Randomise function in combatants but with greater values
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

        int points = 400;

        int attackPoints = Random.Range(0, points / 2);
        points -= attackPoints;
        attackPower = attackPoints / 10 + 20;
        empowermentBacklashDamage = attackPower/2;

        int agilityPoints = Random.Range(0, points / 2);
        points -= agilityPoints;
        agility = agilityPoints;

        int empowermentPoints = Random.Range(0, Mathf.FloorToInt(points / 5));
        points -= empowermentPoints;
        empowermentValue = empowermentPoints * 10;

        healthMax = points;
        #region random empowerment type
        if (Random.Range(0, 10) < 5)
        {
            empowermentType = EmpowermentType.CURSE;
        }
        else
        {
            empowermentType = EmpowermentType.HOLY;
        }
        #endregion
    }

    public override void OnDeath() //If the combatant dies, it stores them and hides them so they can be used again later.
	{
		this.transform.position = new Vector3(0, -100f, 0);
		if (health < 0)
		{
			health = 1;
		}
		this.transform.parent = combatantData.transform;
		DestroyOnDeath();
		if (this.transform.childCount > 0)
		{
			Destroy(this.transform.GetChild(0).gameObject);
		}
	}

	public bool IsNew { get => isNew; set => isNew = value; }
}
