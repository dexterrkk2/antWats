using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tech : Skill
{
    public override void Upgrade()
    {
        player.FoodScaleFactor*=1.25f;
    }
}
