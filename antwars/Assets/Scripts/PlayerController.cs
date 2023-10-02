using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPunCallbacks
{
    public Player photonPlayer;
    public string nickName;
    public int id;
    public MousePosition mouse;
    public Canvas UI;
    public Camera playerCam;
    public List<AntBehavior> soldiers;
    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id - 1] = this;
        if (!photonView.IsMine)
        {
            UI.gameObject.SetActive(false);
            playerCam.gameObject.SetActive(false);
        }
        mouse.playerClan = id - 1;
    }
}