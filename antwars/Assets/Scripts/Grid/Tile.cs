using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public MeshRenderer mat;
    public void TIleColor(Color newColor)
    {
        mat.material.color = newColor;
    }
}
