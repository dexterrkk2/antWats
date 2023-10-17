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
        id = photonView.OwnerActorNr;
        colorChange.color = colors[id-1];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
    }
}
