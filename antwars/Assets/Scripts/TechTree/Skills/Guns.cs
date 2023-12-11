using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : Skill
{
    public override void Upgrade()
    {
        player.antstats.combatMod += 3;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].AssignStats();
        }
    }
}
