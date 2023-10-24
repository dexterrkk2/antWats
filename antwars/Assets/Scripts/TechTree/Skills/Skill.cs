using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill: MonoBehaviour
{
    public Skill left;
    public Skill right;
    public bool previousPaid;
    public bool upgraded;
    public int cost;
    public string name;
    public string description;
    public PlayerController player;
    public Vector3 position;
    public Vector3 parentPosition;
    public abstract void Upgrade();
    public void ButtonClick()
    {
        if(previousPaid && player.resource >= cost)
        {
            if (left != null)
            {
                left.previousPaid = true;
            }
            if (right != null)
            {
                right.previousPaid = true;
            }
            Upgrade();
            player.resource -= cost;
        }
    }
}
