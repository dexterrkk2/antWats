using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Base : MonoBehaviourPunCallbacks
{
    public ColorChange colorChange;
    public List<Color> colors;
    public int id;
    public int baseHp;
    public int maxHp;
    public Slider healthBarValue;
    private void Start()
    {
        photonView.RPC("AssignBase", RpcTarget.All);
        maxHp = baseHp;
        healthBarValue.maxValue = maxHp;
        healthBarValue.value = maxHp;
    }
    [PunRPC]
    void AssignBase()
    {
        id = photonView.OwnerActorNr-1;
        colorChange.color = colors[id];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
        int x = Mathf.RoundToInt(transform.position.x / GameManager.instance.grid.scale);
        int z = Mathf.RoundToInt(transform.position.z / GameManager.instance.grid.scale);
        GameManager.instance.tiles[x, z].Base = this;
    }
    [PunRPC]
    void TakeDamage()
    {
        baseHp--;
        healthBarValue.value = baseHp;
    }
}
