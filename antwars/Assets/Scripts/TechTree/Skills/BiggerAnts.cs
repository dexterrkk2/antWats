using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerAnts : Skill
{
    public override void Upgrade()
    {
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].transform.localScale *= 2;
            player.FoodScaleFactor++;
            player.soldiers[i].combatMod += 1;
        }
    }
}
