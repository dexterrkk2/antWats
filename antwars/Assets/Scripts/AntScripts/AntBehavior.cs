using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class AntBehavior : AntClass
{
    public Camera antCam;
    public List<Color> colors;
    public PlayerController player;
    public float maxDistance;
    public float moveSpeed;
    public ColorChange colorChange;
    public TextMeshProUGUI playerNameText;
    public void Start()
    {
        antCam.gameObject.SetActive(false);
        AssignType(photonView.OwnerActorNr - 1); 
        player = GameManager.instance.players[_playerClan];
        AssignStats();
        colorChange.color = colors[photonView.OwnerActorNr - 1];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
        mesh = colorChange.meshRenderer;
        playerNameText.text = player.nickName + "'s ant";
        player.soldiers.Add(this);
    }
    public void AssignStats()
    {
        hp = player.antstats.hp;
        combatMod = player.antstats.hp;
        moveSpeed = player.antstats.moveSpeed;
        attackTimes = player.antstats.attackTimes;
        transform.localScale = new Vector3(player.antstats.scale, player.antstats.scale, player.antstats.scale);
        AssignType(_playerClan);
    }
    public void Update()
    {
        if (selected)
        {
            Move(MousePosition.mousePosition, player.mouse.playerClan, maxDistance, moveSpeed);
        }
        if (hp <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
        if (Input.GetKeyDown("d"))
        {
            Debug.Log("Pressed d");
            selected = false;
            secondClick = false;
        }
    }
    [PunRPC]
    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.instance.ants.Remove(this);
        player.soldiers.Remove(this);
    }
}
