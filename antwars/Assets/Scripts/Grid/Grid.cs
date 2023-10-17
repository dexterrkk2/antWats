using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
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
        GenerateGrid();
    }
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
                Tile spawnedTile = Instantiate(tilePrefab, gridParent);
                spawnedTile.transform.position = new Vector3(x, 1 - scale * 2, z);
                spawnedTile.TIleColor(colors[colorCounter]);
                colorCounter=0;
            }
        }
        Corners[0].position = new Vector3(-offset.x, -3, +offset.z);
        Corners[1].position = new Vector3(width * scale - offset.x, -3, +offset.z);
        Corners[2].position = new Vector3(+offset.x, -3, height * scale - offset.z);
        Corners[3].position = new Vector3(width*scale-offset.x, -3, height * scale - offset.z);
    }
}
