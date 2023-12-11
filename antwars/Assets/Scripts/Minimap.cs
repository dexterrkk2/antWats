using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Minimap : MonoBehaviourPunCallbacks
{
    public Color allyColor;
    public Color enemyColor;
    public Color resourceColor;
    public Color defaultColor;
    public GameObject tilePrefab;
    public Image[,] mapSquares;
    public PlayerController player;
    public Vector3 minimapSpot;
    int positionx;
    int positionz;
    public int scale;
    public int updateRange;
    private void Start()
    {
        SpawnMiniMap(GameManager.instance.grid.width, GameManager.instance.grid.height);
    }
    public void SpawnMiniMap(int width, int height)
    {
        Debug.Log("Width " +width); 
        Debug.Log("Height " +height);
        mapSquares = new Image[width, height];
        for(int x =0; x< width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject square =Instantiate(tilePrefab, transform);
                square.transform.localPosition = new Vector3((x*scale), y*scale, 0) ;
                square.name += x + " " + y;
                mapSquares[x, y] = square.GetComponent<Image>();
                if (photonView.IsMine)
                {
                    positionx = x;
                    positionz = y;
                    //TileColorPicker();
                }
            }
        }
    }
    public void UpdateTile(int x, int z)
    {
        for (int i = 0; i < updateRange; i++) 
        {
            for (int j = 0; j < updateRange; j++)
            {
                positionx = x + i;
                positionz = z + j;
                bool upperBound = positionx < GameManager.instance.grid.width && positionz < GameManager.instance.grid.height;
                bool lowerBound = positionx >= 0 && positionz >= 0;
                bool inBounds = upperBound & lowerBound;
                if (inBounds && photonView.IsMine)
                {
                    TileColorPicker();
                }
                positionx = x - i;
                positionz = z - j;
                upperBound = positionx < GameManager.instance.grid.width && positionz < GameManager.instance.grid.height;
                lowerBound = positionx >= 0 && positionz >= 0;
                inBounds = upperBound & lowerBound;
                if (inBounds && photonView.IsMine)
                {
                    TileColorPicker();
                }
            }
        }
    }
    [PunRPC]
    void TileColorPicker()
    {
        
        //Debug.Log("Position X: " + positionx);
        //Debug.Log("Position Z: " + positionz);
        bool antonTile = GameManager.instance.tiles[positionx, positionz].ant != null;
        //Debug.Log("ant on tile: " +antonTile + "Tile: " + positionx +" " +positionz);
        bool baseOnTile = GameManager.instance.tiles[positionx, positionz].Base != null;
        //Debug.Log("x" + x);
        // Debug.Log("z" + z);
        //Debug.Log(mapSquares[x,z]);
        //Debug.Log(mapSquares[x, z].color);
        if (antonTile)
        {
            bool isEnemy = (GameManager.instance.tiles[positionx, positionz].ant._playerClan != player.id - 1);
            //Debug.Log(isEnemy+ " enemy here " + x + " " + z);
            //Debug.Log(isAlly + " ally here " + x + " " + z);
            if (isEnemy)
            {
                mapSquares[positionx, positionz].color = enemyColor;
            }
            else
            {
                mapSquares[positionx, positionz].color = allyColor;
            }
        }
        else if (baseOnTile)
        {
            bool isEnemyBase = (GameManager.instance.tiles[positionx, positionz].Base.id != player.id - 1);
            if (isEnemyBase)
            {
                mapSquares[positionx, positionz].color = enemyColor;
            }
            else
            {
                mapSquares[positionx, positionz].color = allyColor;
            }
        }
        else
        {
            if (GameManager.instance.tiles[positionx, positionz].resource != null)
            {
                mapSquares[positionx, positionz].color = resourceColor;
            }
            else
            {
                mapSquares[positionx, positionz].color = defaultColor;
            }
        }
    }
}
