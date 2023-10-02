using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    public bool gameEnded = false; // has the game ended?
    public float timeToWin; // time a player needs to hold the hat to win
    public float invincibleDuration; // how long after a player gets the hat, are they invincible
    [Header("Players")]
    public string playerPrefabLocation; // path in Resources folder to the Player prefab
    public Transform[] spawnPoints; // array of all available spawn points
    public PlayerController[] players; // array of all the players
    private int playersInGame; // number of players in the game
    public int count;
    public string antPrefab;
    // instance
    public static GameManager instance;
    void Awake()
    {
        // instance
        instance = this;
    }
    private void OnEnable()
    {
        AntSpawner.onSpawn += antSpawner;
    }
    private void OnDisable ()
    {
        AntSpawner.onSpawn -= antSpawner;
    }
    // Start is called before the first frame update
    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.All);
    }
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
    }
    [PunRPC]
    void WinGame(int playerId)
    {
        gameEnded = true;
        PlayerController player = GetPlayer(playerId);
        // set the UI to show who's won
        Invoke("GoBackToMenu", 3.0f);
    }
    void GoBackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        Networkmanager.instance.ChangeScene("Menu");
    }
    void SpawnPlayer()
    {
        // instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        // get the player script
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    public void antSpawner()
    {
        if (count >= spawnPoints.Length-1)
        {
            count = 0;
        }
        PhotonNetwork.Instantiate(antPrefab, spawnPoints[count].position, Quaternion.identity);
        count++;
    }
    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId);
    }
    public PlayerController GetPlayer(GameObject playerObject)
    {
        return players.First(x => x.gameObject == playerObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
