using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuvers : Skill
{
    public override void Upgrade()
    {
        for (int i =0; i<player.soldiers.Count; i++)
        {
            player.soldiers[i].hp *= 2;
        }
    }
}
