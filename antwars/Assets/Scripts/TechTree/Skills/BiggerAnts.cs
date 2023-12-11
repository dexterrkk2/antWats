using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerAnts : Skill
{
    public override void Upgrade()
    {
        player.antstats.combatMod += 1;
        player.antstats.scale *= 2;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.FoodScaleFactor++;
            player.soldiers[i].AssignStats();
        }
    }
}
