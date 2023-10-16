using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Base : MonoBehaviourPunCallbacks
{
    public ColorChange colorChange;
    public List<Color> colors;
    public int id;
    private void Start()
    {
        photonView.RPC("AssignBase", RpcTarget.All);
    }
    [PunRPC]
    void AssignBase()
    {
        colorChange.color = colors[photonView.OwnerActorNr-1];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
        id = photonView.OwnerActorNr-1;
    }
}
