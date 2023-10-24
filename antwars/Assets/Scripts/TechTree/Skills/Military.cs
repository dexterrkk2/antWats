using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Military : Skill
{
    public override void Upgrade()
    {
        player.foodUseFactor++;
    }
}
