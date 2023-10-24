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
    public bool hasLost;
    public TextMeshProUGUI resouceText;
    public List<AntBehavior> soldiers;
    public GameObject startPosition;
    Base Base;
    public float FoodScaleFactor =1;
    public float foodUseFactor = 1;
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
        resource = 10;
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
    public void Update()
    {
        resouceText.text = "Food Left: " + resource;
    }
}