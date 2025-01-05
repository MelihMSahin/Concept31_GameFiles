using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombatant : Combatant
{
    protected override void RandomiseStats()
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

        int points = 200;

        int attackPoints = Random.Range(0, points / 2);
        points -= attackPoints;
        attackPower = attackPoints / 10 + 10;
        empowermentBacklashDamage = attackPower / 2;

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
}
