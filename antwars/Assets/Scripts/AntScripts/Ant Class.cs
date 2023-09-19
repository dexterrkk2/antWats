using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntClass : MonoBehaviour
{
    public string type;
    public List<string> types;
    public int damage;
    public bool selected = false;
    public bool secondClick = false;
    public void AssignType(int playerClan)
    {
        type = types[playerClan];
    }
    public void OnMouseDown()
    {
        selected = true;
    }
    public void Move(Vector3 targetSquare)
    {
        if (selected == true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (secondClick)
                {
                    transform.position = targetSquare;
                    selected = false;
                    secondClick = false;
                }
                else
                {
                    secondClick = true;
                }

            }
        }
    }
}
