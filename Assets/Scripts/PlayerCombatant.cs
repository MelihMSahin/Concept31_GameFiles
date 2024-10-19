using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{
	private void Awake()
	{
		isAlly = true;
	}

	public override bool Attack()
	{
		return false;
	}
}
