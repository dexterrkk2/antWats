using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryCapstone : Skill
{
    public override void Upgrade()
    {
        player.antstats.attackTimes += 1;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].AssignStats();
        }
    }
}
