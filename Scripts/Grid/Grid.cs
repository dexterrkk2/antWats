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
    int colorCounter;
    private void Start()
    {
        GenerateGrid();
    }
    void GenerateGrid()
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
                spawnedTile.transform.position = new Vector3(x - offset.x, 1 - scale * 2, z - offset.z);
                spawnedTile.TIleColor(colors[colorCounter]);
                colorCounter=0;
            }
        }
    }
}
