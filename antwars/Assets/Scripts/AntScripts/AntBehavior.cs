using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntBehavior : AntClass
{
    public Camera antCam;
    public void Start()
    {
        AssignType(Player.instance.playerClan);
        antCam.gameObject.SetActive(false);
    }
    public void Update()
    {
        if (selected)
        {
            Move(MousePosition.mousePosition);
        }
    }
}
