using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryCapstone : Skill
{
    public override void Upgrade()
    {
        for (int i = 0; i < player.soldiers.Count; i++)
        {
            player.soldiers[i].damage *= 2;
        }
    }
}
