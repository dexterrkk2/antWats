using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Skill
{
    public override void Upgrade()
    {
        CultureSkill cultureSkill= player.GetComponentInChildren<CultureSkill>();
        //cultureSkill.resourceTime = cultureSkill.resourceTime / 2;
        cultureSkill.Upgrade();
    }
}
