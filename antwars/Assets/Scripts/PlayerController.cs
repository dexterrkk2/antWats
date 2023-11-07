using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public Player photonPlayer;
    public string nickName;
    public int id;
    public MousePosition mouse;
    public Canvas UI;
    public Camera playerCam;
    public float resource;
    public bool hasLost = false;
    public TextMeshProUGUI resouceText;
    public List<AntBehavior> soldiers;
    public GameObject startPosition;
    Base Base;
    public float FoodScaleFactor =1;
    public float foodUseFactor = 1;
    public TextMeshProUGUI playerText;
    public List<Color> colors;
    public List<string> colorNames;
    public void OnEnable()
    {
        AntSpawner.onSpawn += ResourceUse;
    }
    public void OnDisable()
    {
        AntSpawner.onSpawn -= ResourceUse;
    }
    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        GameManager.instance.players[id-1] = this;
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
            playerCam.gameObject.SetActive(false);
        }
        else
        {
            startPosition = GameManager.instance.BasePoints[id - 1].gameObject;
            transform.position = startPosition.transform.position;
        }
        mouse.playerClan = id - 1;
        playerText.color = colors[id-1];
        playerText.text = "You are " + colorNames[id - 1];
    }
    public void ResourceUse(int id)
    {
        resource -= (1/foodUseFactor);
        Starvation();
    }
    public void Starvation()
    {
        if (resource == 0)
        {
            for(int i =0; i< soldiers.Count; i++)
            {
                if(i%2 == 0)
                {
                    soldiers.RemoveAt(i);
                }
            }
        }  
    }
    [PunRPC]
    public void LoseGame()
    {
        hasLost = true;
        Debug.Log("Lose");
        GameManager.instance.players.Remove(this);
        GameManager.instance.photonView.RPC("SubtractPlayers", RpcTarget.All);
        GameManager.instance.photonView.RPC("checkWinCondition", RpcTarget.All);
        PhotonNetwork.LeaveRoom();
        GameManager.instance.LoadLoseScreen();
    }
    public void Update()
    {
        resouceText.text = "Food Left: " + resource;
        
    }
}