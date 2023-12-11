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
    public int creditSceneNumber;
    public int alivePlayers;
    public float damageTime;
    public Grid grid;
    public int resourceNum;
    public Tile[,] tiles;
    public Vector3 antOffset;
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
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            PlayerController[] temp = new PlayerController[PhotonNetwork.PlayerList.Length];
            players = temp.ToList();
            tiles = new Tile[grid.height, grid.width];
            if (photonView.IsMine)
            {
                grid.photonView.RPC("GenerateGrid", RpcTarget.All);
                for (int i = 0; i < resourceNum; i++)
                {
                    ResourceSpawn();
                }
            }
            if (photonView != null)
            {
                photonView.RPC("ImInGame", RpcTarget.All);
            }
            photonView.RPC("UpdateResources", RpcTarget.All);
        }
    }
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            int id = SpawnPlayer();
            Base playerBase = SpawnBase(id-1);
            //playerBase.id = id-1;
            bases.Add(playerBase);
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
        PhotonNetwork.Instantiate(antPrefab, BasePoints[id].position +grid.offset/4, Quaternion.identity);
    }
    [PunRPC]
    public Base SpawnBase(int num)
    {
        GameObject Base = PhotonNetwork.Instantiate(basePrefabLocation, BasePoints[num].position, Quaternion.identity);
        tiles[((int)(BasePoints[num].position.x/grid.scale)), ((int)(BasePoints[num].position.z/grid.scale))].Base = Base.GetComponent<Base>();
        return Base.GetComponent<Base>();
    }
    [PunRPC]
    public void ResourceSpawn()
    {
        int randomX = Random.Range(0, grid.width);
        int randomZ = Random.Range(0, grid.height);
        Vector3 randomPos = new Vector3(randomX * grid.scale, -4, randomZ * grid.scale);
        GameObject  rescource =PhotonNetwork.Instantiate(resourcePrefab, randomPos, Quaternion.identity);
        if (rescource != null)
        {
            tiles[randomX, randomZ].resource = rescource;
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
    public void AntCheck(AntClass ant)
    {
        int pastx = (int)Mathf.Round(ant.pastPosition.x / grid.scale);
        int pastz = (int)Mathf.Round(ant.pastPosition.z / grid.scale);
        int futurex = (int)Mathf.Round(ant.futurePosition.x / grid.scale);
        int futurez = (int)Mathf.Round(ant.futurePosition.z / grid.scale);
        bool upperBoundPast = pastx < grid.width && pastz < grid.height;
        bool lowerBoundPast = pastx >= 0 && pastz >= 0;
        bool inBoundsPast = upperBoundPast & lowerBoundPast;
        bool upperBound = futurex < grid.width && futurez < grid.height;
        bool lowerBound = futurex >= 0 && futurez >= 0;
        bool inBoundsFuture = upperBound & lowerBound;
        if (inBoundsFuture)
        {
            if (tiles[futurex, futurez].resource != null)
            {
                players[ant._playerClan].resource += 1 * players[ant._playerClan].FoodScaleFactor;
                tiles[futurex, futurez].resource.SetActive(false);
                tiles[futurex, futurez].resource = null;
            }
            if (tiles[futurex, futurez].ant != null && tiles[futurex, futurez].ant._playerClan != ant._playerClan)
            {
                Debug.Log(ant == tiles[pastx, pastz].ant);
                Combat(tiles[futurex, futurez].ant, ant);
            }
            tiles[futurex, futurez].ant = ant;
            if (tiles[futurex, futurez].Base != null && tiles[futurex, futurez].Base.id != ant._playerClan)
            {
                tiles[futurex, futurez].Base.photonView.RPC("TakeDamage", RpcTarget.All);
                Debug.Log("Base hp " + tiles[futurex, futurez].Base.baseHp);
                if (tiles[futurex, futurez].Base.baseHp <= 0)
                {
                    Debug.Log("Player Lose:" + players[tiles[futurex, futurez].Base.id].id);
                    players[tiles[futurex, futurez].Base.id].photonView.RPC("LoseGame", players[tiles[futurex, futurez].Base.id].photonPlayer);
                }
            }
            players[ant._playerClan].minimap.UpdateTile(futurex, futurez);
        }
        if (inBoundsPast)
        {
            tiles[pastx, pastz].ant = null;
            players[ant._playerClan].minimap.UpdateTile(pastx, pastz);

        }
    }
    public void Combat(AntClass ant1, AntClass ant2)
    {
        while(ant1.hp> 0 && ant2.hp > 0)
        {
            int damageRoll1 = Random.Range(0,6) + Random.Range(0, 6) + Random.Range(0, 6) + ant1.combatMod;
            int damageRoll2 = Random.Range(0, 6) + Random.Range(0, 6) + Random.Range(0, 6) + ant2.combatMod;
            if(damageRoll1 > damageRoll2)
            {
                for (int i = 0; i < ant1.attackTimes; i++)
                {
                    ant2.photonView.RPC("TakeDamage", RpcTarget.All);
                }
            }
            else
            {
                for (int i = 0; i < ant2.attackTimes; i++)
                {
                    ant1.photonView.RPC("TakeDamage", RpcTarget.All);
                }
            }
            Debug.Log("ant 1 " +ant1.hp);
            Debug.Log("ant 2 " +ant2.hp);
        }
        if (ant1.hp <= 0)
        {
            Debug.Log("ant died: " + ant1);
        }
        if (ant2.hp <= 0)
        {
            Debug.Log("ant died: " + ant2);
        }
    }
    [PunRPC]
    public void UpdateResources()
    {
        Resource[] resource = FindObjectsOfType<Resource>();
        for (int i = 0; i < resource.Length; i++)
        {
            int positionx = Mathf.RoundToInt(resource[i].transform.position.x/grid.scale);
            int positionz = Mathf.RoundToInt(resource[i].transform.position.z/grid.scale);
            tiles[positionx, positionz].resource = resource[i].gameObject;
        }
    }
    /*   public void BaseCheck(AntBehavior ant)
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
       }*/
    public void BackToMenu()
    {
        LoadScene(0);
    }
    public void ToCredits()
    {
        LoadScene(creditSceneNumber);
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
    public void LoadLoseScreen() 
    {
        LoadScene(loseSceneNumber);
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
        PhotonNetwork.LeaveRoom();
    }
}