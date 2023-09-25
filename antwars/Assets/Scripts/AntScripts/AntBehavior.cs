using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntBehavior : AntClass
{
    public Camera antCam;
    public List<Color> colors;
    public int initialPlayerClan;
    public int playerClan;
    private PlayerController player;
    public float maxDistance;
    public float moveSpeed;
    public ColorChange colorChange;
    public void Start()
    {
        player = FindObjectOfType<PlayerController>();
        antCam.gameObject.SetActive(false);
        AssignType(initialPlayerClan);
        colorChange.color = colors[initialPlayerClan];
        colorChange.photonView.RPC("SetColor", RpcTarget.AllBuffered);
    }
    public void Update()
    {
        if (selected)
        {
            Move(MousePosition.mousePosition, player.mouse.playerClan, maxDistance, moveSpeed);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ant")
        {
            AntBehavior ant=collision.collider.GetComponent<AntBehavior>();
            if (playerClan != ant.playerClan)
            {
                ant.TakeDamage(damage);
            }
        }
    }

}
