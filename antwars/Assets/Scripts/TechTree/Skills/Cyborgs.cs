using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyborgs : Skill
{
    public override void Upgrade()
    {
        player.antstats.combatMod += 4;
        player.FoodScaleFactor*=1.25f;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].AssignStats();
        }
    }
}
