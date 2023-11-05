using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public List<MeshRenderer> mats;
    public AntClass ant;
    public GameObject resource;
    public Base Base;
    public void TIleColor(Color newColor)
    {
        for (int i = 0; i < mats.Count; i++)
        {
            mats[i].material.color = newColor;
        }
    }
}
