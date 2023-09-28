using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class Networkmanager : MonoBehaviourPunCallbacks
{
    public static Networkmanager instance;
    void Awake()
    {
        // if an instance already exists and it's not this one - destroy us
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            // set the instance
            instance = this;
        }
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName);
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master server");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created room: " + PhotonNetwork.CurrentRoom.Name);
    }
    // instance
    

}
