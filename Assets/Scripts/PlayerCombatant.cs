using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{
	public override float DealDmg()
	{
		Debug.Log("Player");
		//Play the animation, describe what happened in text
		return attackPower;
	}
}
