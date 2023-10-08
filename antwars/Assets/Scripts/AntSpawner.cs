using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntSpawner : MonoBehaviourPunCallbacks
{
    public PlayerController player;
    public delegate void SpawnEvent();
    public static event SpawnEvent onSpawn;
    public GameManager gameManager;
    private void Start()
    {
        gameManager = (GameManager) FindObjectOfType(typeof(GameManager));
    }
    public void AntEvent()
    {
        onSpawn();
    }
}
