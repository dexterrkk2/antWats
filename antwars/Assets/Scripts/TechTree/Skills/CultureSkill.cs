using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultureSkill : Skill
{
    public float resourceTime;
    public override void Upgrade()
    {
        InvokeRepeating("PassiveBuff", 0f, resourceTime);
    }
    public void PassiveBuff()
    {
        player.resource++;
    }
}
