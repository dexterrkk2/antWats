using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class AntBehavior : AntClass
{
    public Camera antCam;
    public List<Color> colors;
    public PlayerController player;
    public float maxDistance;
    public float moveSpeed;
    public ColorChange colorChange;
    public TextMeshProUGUI playerNameText;
    public void Start()
    {
        antCam.gameObject.SetActive(false);
        AssignType(photonView.OwnerActorNr - 1); ;
        player = GameManager.instance.players[_playerClan];
        colorChange.color = colors[photonView.OwnerActorNr - 1];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
        mesh = colorChange.meshRenderer;
        playerNameText.text = player.nickName + "'s ant";
        player.soldiers.Add(this);
    }
    public void Update()
    {
        if (selected)
        {
            Move(MousePosition.mousePosition, player.mouse.playerClan, maxDistance, moveSpeed);
        }
        if (hp <= 0)
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
