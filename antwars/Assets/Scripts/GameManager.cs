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
    public string resourcePrefab;
    // instance
    public static GameManager instance;
    public List<AntBehavior> ants;
    public List<Base> bases;
    public List<Resource> resources;
    public int loseSceneNumber;
    public int winSceneNumber;
    public int alivePlayers;
    public float damageTime;
    public Grid grid;
    public int resourceNum;
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
        if (grid != null)
        {
            grid.GenerateGrid();
        }
        if (photonView != null)
        {
            photonView.RPC("ImInGame", RpcTarget.All);
        }
        if (photonView.IsMine)
        {
            for (int i = 0; i < resourceNum; i++)
            {
                ResourceSpawn();
            }
            for (int i =0; i< players.Count; i++)
            {
                Base playerBase = SpawnBase(i);
                playerBase.id = i;
                bases.Add(playerBase);
            }
        }
        UpdateResources();
    }
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            int id = SpawnPlayer();
        }
    }
    int SpawnPlayer()
    {
        // instantiate the player across the network
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, BasePoints[0].transform.position, Quaternion.identity);
        // get the player script
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        return playerScript.id;
    }
    [PunRPC]
    public void antSpawner(int id)
    {
        GameObject antObject =PhotonNetwork.Instantiate(antPrefab, BasePoints[id].position, Quaternion.identity);
        AntBehavior ant = antObject.GetComponent<AntBehavior>();
        //Debug.Log("playerid" + id);
        //Debug.Log("ant id" + ant._playerClan);
        if (id == ant._playerClan)
        {
            players[id].soldiers.Add(ant);
        }
    }
    [PunRPC]
    public Base SpawnBase(int num)
    {
        GameObject Base = PhotonNetwork.Instantiate(basePrefabLocation, BasePoints[num].position, Quaternion.identity);
        return Base.GetComponent<Base>();
    }
    [PunRPC]
    public void ResourceSpawn()
    {
        int randomX = Random.Range(0, grid.width * grid.scale);
        int randomZ = Random.Range(0, grid.height * grid.scale);
        Vector3 randomPos = new Vector3(randomX, -3, randomZ);
        PhotonNetwork.Instantiate(resourcePrefab, randomPos, Quaternion.identity);
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
            ResourceCheck(ants[i]);
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
    public void UpdateBases()
    {
        Base[] Base = FindObjectsOfType<Base>();
        for (int i = 0; i < Base.Length; i++)
        {
            bases.Add(Base[i]);
        }
    }
    public void UpdateResources()
    {
        Resource[] resource = FindObjectsOfType<Resource>();
        for (int i = 0; i < resource.Length; i++)
        {
            resources.Add(resource[i]);
        }
    }
    public void BaseCheck(AntBehavior ant)
    {
        for (int i = 0; i < bases.Count; i++) 
        {
            float distance = (ant.futurePosition - bases[i].transform.position).sqrMagnitude;
            //Debug.Log("ant radius " + ant.damageRadius);
            //Debug.Log("distance " + distance);
            if (ant.damageRadius >= distance)
            {
                //Debug.Log("Ant id " + ant._playerClan);
                //Debug.Log("Base id " + bases[i].id);
                if (ant._playerClan != bases[i].id)
                {
                    LoseGame(players[bases[i].id]);
                }
            }
        }
    }
    public void ResourceCheck(AntBehavior ant)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            float distance = (ant.futurePosition - resources[i].transform.position).sqrMagnitude;
          
            if (ant.damageRadius >= distance)
            {
                players[ant._playerClan].resource+= 1* players[ant._playerClan].FoodScaleFactor;
                resources[i].photonView.RPC("Die", RpcTarget.All);
                resources.RemoveAt(i);
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