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
    public AntSpawner antSpawner;
    public MousePosition mouse;
    public Canvas UI;
    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id - 1] = this;
        antSpawner.playerClan = id - 1;
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
        mouse.playerClan = id - 1;
    }
}
