using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : AntClass
{
    public void Start()
    {
        AssignType(Player.instance.playerClan);
    }
}
