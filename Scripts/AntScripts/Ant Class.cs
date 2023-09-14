using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntClass : MonoBehaviour
{
    public string type;
    public List<string> types;
    public int damage;
    public void AssignType(int playerClan)
    {
        type = types[playerClan];
    }

}
