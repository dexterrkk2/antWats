using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ColorChange : MonoBehaviourPunCallbacks
{
    public static ColorChange instance;
    public MeshRenderer meshRenderer;
    public Image ImageColor;
    public Color color;
    void start()
    {
        instance = this;
    }
    [PunRPC]
    public void SetColor()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = color;
        }
        else
        {
            ImageColor.color = color;
        }
    }
}
