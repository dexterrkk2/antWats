using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PincerManeuver : Skill
{
    public override void Upgrade()
    {
        player.antstats.combatMod += 2;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].AssignStats();
        }
    }
}
