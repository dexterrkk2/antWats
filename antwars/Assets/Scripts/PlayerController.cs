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
<<<<<<< Updated upstream
    public Camera playerCam;
=======
    public Camera antCam;
    public List<AntBehavior> soldiers;
>>>>>>> Stashed changes
    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id - 1] = this;
        if (!photonView.IsMine)
        {
<<<<<<< Updated upstream
            UI.gameObject.SetActive(false);
            playerCam.gameObject.SetActive(false);
=======
            gameObject.SetActive(false);
            antCam.gameObject.SetActive(false);
>>>>>>> Stashed changes
        }
        mouse.playerClan = id - 1;
    }
}