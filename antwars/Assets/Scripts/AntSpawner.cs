using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntSpawner : MonoBehaviourPunCallbacks
{
    public List<Transform> spawnPoints;
    public string antPrefab;
    public int count;
    public int playerClan;
    public void antSpawner()
    {
        if (count >= spawnPoints.Count)
        {
            count = 0;
        }
        GameObject ant =PhotonNetwork.Instantiate(antPrefab, spawnPoints[count].position, Quaternion.identity);
        AntBehavior antBehavior = ant.GetComponent<AntBehavior>();
        antBehavior.initialPlayerClan = playerClan;
        count++;
    }
}
