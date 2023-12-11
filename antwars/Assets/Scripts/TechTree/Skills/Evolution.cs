using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : Skill
{
    public override void Upgrade()
    {
        player.antstats.moveSpeed *= 1.5f;
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].AssignStats();
        }
    }
}
