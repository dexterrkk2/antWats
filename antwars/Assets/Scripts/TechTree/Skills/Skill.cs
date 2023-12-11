using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Skill: MonoBehaviour
{
    public Skill left;
    public Skill right;
    public bool previousPaid;
    public bool upgraded;
    public bool activated = false;
    public int cost;
    public string name;
    public string description;
    public PlayerController player;
    public Vector3 position;
    public Vector3 parentPosition;
    public Color disableColor = Color.black;
    public Color enableColor = Color.green;
    public Button button;
    public ColorChange colorChange;
    public abstract void Upgrade();
    public void ButtonClick()
    {
        if(previousPaid && player.resource >= cost && activated == false)
        {
            if (left != null)
            {
                left.previousPaid = true;
                left.enableColor = enableColor;
                left.disableColor = disableColor;
                left.colorChange.color = enableColor;
                left.colorChange.SetColor();
            }
            if (right != null)
            {
                right.previousPaid = true;
                right.enableColor = enableColor;
                right.disableColor = disableColor;
                right.colorChange.color = enableColor;
                right.colorChange.SetColor();
            }
            Upgrade();
            player.resource -= cost;
            activated = true;
            colorChange.color = disableColor;
            colorChange.SetColor();
        }
    }
}
