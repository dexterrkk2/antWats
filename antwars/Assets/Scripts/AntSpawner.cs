using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public List<GameObject> ants;
    public List<Transform> spawnPoints;
    public int count;
    public void antSpawner()
    {
        if (count >= spawnPoints.Count)
        {
            count = 0;
        }
        Instantiate(ants[0], spawnPoints[count]);
        count++;
    }
}
