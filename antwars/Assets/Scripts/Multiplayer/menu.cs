using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class menu : MonoBehaviourPunCallbacks
{
    public string sceneName;
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;
    [Header("Main Screen")]
    public Button createRoomButton;
    public Button joinRoomButton;
    [Header("Lobby Screen")]
    public TextMeshProUGUI playerListText;
    public Button startGameButton;

    // Start is called before the first frame update
    void Start()
    {
        createRoomButton.interactable = false;
        joinRoomButton.interactable = false;
    }
    public override void OnConnectedToMaster()
    {
        createRoomButton.interactable = true;
        joinRoomButton.interactable = true;
    }
    void SetScreen(GameObject screen)
    {
        // deactivate all screens
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);
        // enable the requested screen
        screen.SetActive(true);
    }
    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        Networkmanager.instance.CreateRoom(roomNameInput.text);
    }
    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {
        Networkmanager.instance.JoinRoom(roomNameInput.text);
    }
    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }
    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }
    [PunRPC]
    public void UpdateLobbyUI()
    {
        playerListText.text = "";
        // display all the players currently in the lobby
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";
        }
        // only the host can start the game
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.interactable = true;
        }
        else
        {
            startGameButton.interactable = false;
            // since there's now a player in the lobby, tell everyone to update the lobby UI
            //photonView.RPC("UpdateLobbyUI", RpcTarget.All);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // we don't RPC it like when we join the lobby
        // that's because OnJoinRoom is only called for the client who just joined
        // OnPlayerLeftRoom gets called for all clients in the room, so we don't need to RPC 
        UpdateLobbyUI();
    }
    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }
    public void OnStartGameButton()
    {
        Networkmanager.instance.photonView.RPC("ChangeScene", RpcTarget.All, sceneName);
    }
}
