using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAndScratch : Skill
{
    public override void Upgrade()
    {
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].combatMod += 1;
        }
    }
}
