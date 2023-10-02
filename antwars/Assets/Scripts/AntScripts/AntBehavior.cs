using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AntBehavior : AntClass
{
    public Camera antCam;
    public List<Color> colors;
    public int initialPlayerClan;
<<<<<<< Updated upstream
    public int playerClan;
=======
>>>>>>> Stashed changes
    private PlayerController player;
    public float maxDistance;
    public float moveSpeed;
    public ColorChange colorChange;
    public void Start()
    {
        player = FindObjectOfType<PlayerController>();
        antCam.gameObject.SetActive(false);
<<<<<<< Updated upstream
        AssignType(player.id-1);
        colorChange.color = colors[playerClan];
=======
        AssignType(photonView.OwnerActorNr - 1); ;
        colorChange.color = colors[photonView.OwnerActorNr - 1];
>>>>>>> Stashed changes
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
        Debug.Log("collide");
        if (collision.collider.tag == "Ant")
        {
            AntBehavior ant=collision.collider.GetComponent<AntBehavior>();
<<<<<<< Updated upstream
            if (playerClan != ant.playerClan)
=======
            if (photonView.OwnerActorNr != ant.photonView.OwnerActorNr)
>>>>>>> Stashed changes
            {
                ant.TakeDamage(damage);
            }
        }
    }
<<<<<<< Updated upstream

}
=======
}     
>>>>>>> Stashed changes
