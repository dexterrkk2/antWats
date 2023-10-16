using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntBehavior : AntClass
{
    public Camera antCam;
    public List<Color> colors;
    private PlayerController player;
    public float maxDistance;
    public float moveSpeed;
    public ColorChange colorChange;
    public void Start()
    {
        player = FindObjectOfType<PlayerController>();
        antCam.gameObject.SetActive(false);
        AssignType(photonView.OwnerActorNr - 1); ;
        colorChange.color = colors[photonView.OwnerActorNr - 1];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
    }
    public void Update()
    {
        if (selected)
        {
            Move(player.mouse.mousePosition, player.mouse.playerClan, maxDistance, moveSpeed);
        }
        if(hp <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }
    [PunRPC]
    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.instance.ants.Remove(this);
    }
}
