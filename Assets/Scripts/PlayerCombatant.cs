using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatant : Combatant
{
	public PlayerCombatant(int lvl, float experiencePoints, float healthMax, float health, float attackPower, float agility) : base(lvl, healthMax, attackPower, agility)
	{
		this.health = health;
		this.experiencePoints = experiencePoints;
	}

	private void Awake()
	{
		isAlly = true;
		RandomiseStats();
	}
}
