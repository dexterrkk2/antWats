using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PincerManeuver : Skill
{
    public override void Upgrade()
    {
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].damageRadius *= 2;
        }
    }
}
