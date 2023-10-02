using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ColorChange : MonoBehaviourPunCallbacks
{
    public static ColorChange instance;
    public MeshRenderer meshRenderer;
    public Color color;
    void start()
    {
        instance = this;
    }
    [PunRPC]
    public void SetColor()
    {
        meshRenderer.material.color = color;
    }
}
