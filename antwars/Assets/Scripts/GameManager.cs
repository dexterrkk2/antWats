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
    public string basePrefabLocation;
    public Transform[] BasePoints; // array of all available spawn points
    public List<PlayerController> players; // array of all the players
    private int playersInGame; // number of players in the game
    public string antPrefab;
    // instance
    public static GameManager instance;
    public List<AntBehavior> ants;
    public List<Base> bases;
    public int loseSceneNumber;
    public int winSceneNumber;
    public int alivePlayers;
    public float damageTime;
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
        SpawnBase(playersInGame);
    }
    void SpawnPlayer()
    {
        // instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, BasePoints[0].transform.position, Quaternion.identity);
        // get the player script
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
    [PunRPC]
    public void antSpawner(int id)
    {
        PhotonNetwork.Instantiate(antPrefab, BasePoints[id].position, Quaternion.identity);
    }
    [PunRPC]
    public void SpawnBase(int num)
    {
        PhotonNetwork.Instantiate(basePrefabLocation, BasePoints[num-1].position, Quaternion.identity);
        UpdateBases();
    }
    public void UpdateBases()
    {
       Base[] Base = FindObjectsOfType<Base>();
        for (int i=0; i< Base.Length; i++)
        {
            bases.Add(Base[i]);
        }
    }
    public PlayerController GetPlayer(int playerId)
    {
        return players.First(x => x.id == playerId);
    }
    public PlayerController GetPlayer(GameObject playerObject)
    {
        return players.First(x => x.gameObject == playerObject);
    }
    public void AntCheck()
    {
        for (int i=0; i< ants.Count; i++)
        {
            if (i> ants.Count-1)
            {
                KillAnts(i, i + 1);
            }
            else
            {
                KillAnts(i, 0);
            }
            BaseCheck(ants[i]);
        }
    }
    public void UpdateAnts(int id)
    {
        ants.Clear();
        AntBehavior[] temp = (AntBehavior[]) FindObjectsOfType(typeof(AntBehavior));
        for (int i = 0; i < temp.Length; i++)
        {
            ants.Add(temp[i]);
        }
    }
    public void BaseCheck(AntBehavior ant)
    {
        for (int i = 0; i < bases.Count; i++) 
        {
            float distance = (ant.futurePosition - bases[i].transform.position).sqrMagnitude;
            Debug.Log(ant.damageRadius >= distance);
            if (ant.damageRadius >= distance)
            {
                Debug.Log(ant._playerClan);
                Debug.Log(bases[i].id);
                if (ant._playerClan != bases[i].id)
                {
                    LoseGame(players[bases[i].id - 1]);
                }
            }
        }
    }
    public void KillAnts(int var1, int var2)
    {
        AntBehavior ant1 = ants[var1];
        AntBehavior ant2 = ants[var2];
        if (ant1._playerClan != ant2._playerClan)
        {
            float distance1 = (ant1.futurePosition - ant2.transform.position).sqrMagnitude;
            float distance2 = (ant1.transform.position - ant2.futurePosition).sqrMagnitude;
            if (ant1.damageRadius >= distance1)
            {
                ant2.AssignDamage(ant1.damage);
                ant2.Invoke("TakeDamage", damageTime);
            }
            if (ant2.damageRadius >= distance2)
            {
                ant1.AssignDamage(ant2.damage);
                ant1.Invoke("TakeDamage", damageTime);
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