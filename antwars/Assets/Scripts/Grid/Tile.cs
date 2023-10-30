using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public MeshRenderer mat;
    public AntClass ant;
    public GameObject resource;
    public Base Base;
    public void TIleColor(Color newColor)
    {
        mat.material.color = newColor;
    }
}
