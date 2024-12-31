using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{
	private bool isNew = true;

	private void Awake()
	{
		isAlly = true;
		if (isNew)
		{
			RandomiseStats();
		}
		gameObject.GetComponent<PlayerCombatant>().enabled = false;
	}

	public override void OnDeath()
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
