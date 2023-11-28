using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Grid : MonoBehaviourPunCallbacks
{
    public int width, height, scale;
    public Tile tilePrefab;
    public Vector3 offset;
    public Transform gridParent;
    public List<Color> colors;
    public List<Transform> Corners;
    int colorCounter;
    private void Start()
    {
        //GenerateGrid();
    }
    [PunRPC]
    public void GenerateGrid()
    {
        for (int x = 0; x < width*scale; x+=(1*scale))
        {
            for (int z = 0; z < height*scale; z+=(1*scale))
            {
                if ((x + z) % 2 ==1)
                {
                    colorCounter=1;
                }
                SpawnTile(x,z);
            }
        }
        Corners[0].position = new Vector3(+offset.x, -3, +offset.z);
        Corners[1].position = new Vector3(width * scale - offset.x, -3, +offset.z);
        Corners[2].position = new Vector3(+offset.x, -3, height * scale - offset.z);
        Corners[3].position = new Vector3(width*scale-offset.x, -3, height * scale - offset.z);
    }
    void SpawnTile(int x, int z)
    {
        Tile spawnedTile = Instantiate(tilePrefab, gridParent);
        spawnedTile.transform.position = new Vector3(x, 1 - (scale * 1.5f), z);
        spawnedTile.TIleColor(colors[colorCounter]);
        spawnedTile.transform.localScale = new Vector3(scale, scale, scale);
        colorCounter = 0;
        GameManager.instance.tiles[x / scale, z / scale] = spawnedTile;
        spawnedTile.name += x / scale + " ";
        spawnedTile.name += z / scale;
    }
}
