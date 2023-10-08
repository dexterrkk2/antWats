using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Stats")]
    public bool gameEnded = false; // has the game ended?
    [Header("Players")]
    public string playerPrefabLocation; // path in Resources folder to the Player prefab
    public Transform[] spawnPoints; // array of all available spawn points
    public List<PlayerController> players; // array of all the players
    private int playersInGame; // number of players in the game
    public int count;
    public string antPrefab;
    // instance
    public static GameManager instance;
    public List<AntBehavior> ants;
    public int loseSceneNumber;
    public int winSceneNumber;
    public int alivePlayers;
    void Awake()
    {
        // instance
        instance = this;
    }
    private void OnEnable()
    {
        AntSpawner.onSpawn += antSpawner;
        AntSpawner.onSpawn += UpdateAnts;
    }
    private void OnDisable ()
    {
        AntSpawner.onSpawn -= antSpawner;
        AntSpawner.onSpawn -= UpdateAnts;
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerController[] temp = new PlayerController[PhotonNetwork.PlayerList.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            players.Add(temp[i]);
            alivePlayers++;
        }
        if (photonView != null)
        {
            photonView.RPC("ImInGame", RpcTarget.All);
        }
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
    public void AntKillScript()
    {
        for (int i=0; i< ants.Count; i++)
        {
            for (int j = 0; j < ants.Count; j++)
            {
                KillAnts(i,j);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateAnts()
    {
        ants.Clear();
        AntBehavior[] temp = (AntBehavior[]) FindObjectsOfType(typeof(AntBehavior));
        for (int i = 0; i < temp.Length - 1; i++)
        {
            ants.Add(temp[i]);
        }
    }
    public void KillAnts(int var1, int var2)
    {
        float distance;
        AntBehavior ant1 = ants[var1];
        AntBehavior ant2 = ants[var2];
        if (ant1._playerClan != ant2._playerClan)
        {
            distance = (ant1.transform.position - ant2.transform.position).sqrMagnitude;
            if (ant1.damageRadius >= distance)
            {
                ant2.TakeDamage(ant1.damage);
                if (ant2.hp <=0)
                {
                    ants.Remove(ant2);
                    ant2.photonView.RPC("Die", RpcTarget.All);
                }
            }
            if (ant2.damageRadius >= distance)
            {
                ant1.TakeDamage(ant2.damage);
                if (ant1.hp <= 0)
                {
                    ants.Remove(ant1);
                    ant1.photonView.RPC("Die", RpcTarget.All);
                }
            }
        }
    }
    [PunRPC]
    public void LoseGame(PlayerController playerController)
    {
        playerController.hasLost = true;
        photonView.RPC("SubtractPlayers", RpcTarget.All);
        photonView.RPC("checkWinCondition", RpcTarget.All);
        PhotonNetwork.LeaveRoom();
        LoadScene(loseSceneNumber);
    }
    public void BackToMenu()
    {
        LoadScene(0);
    }
    [PunRPC]
    public void checkWinCondition()
    {
        Debug.Log(alivePlayers);
        if(alivePlayers == 1)
        {
            for(int i =0; i< players.Count; i++)
            {
                if (players[i].hasLost == false) 
                {
                    photonView.RPC("WinGame", players[i].photonPlayer);
                }
            }
        }
    }
    public void LoadScene(int sceneNumber)
    {
        PhotonNetwork.LoadLevel(sceneNumber);
    }
    [PunRPC]
    public void SubtractPlayers()
    {
        alivePlayers--;
    }
    [PunRPC]
    public void WinGame()
    {
        LoadScene(winSceneNumber);
    }
}