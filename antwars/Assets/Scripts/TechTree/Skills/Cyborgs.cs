using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyborgs : Skill
{
    public override void Upgrade()
    {
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.FoodScaleFactor++;
            player.soldiers[i].combatMod += 2;
        }
    }
}
