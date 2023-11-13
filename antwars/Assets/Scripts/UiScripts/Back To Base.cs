using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToBase : MonoBehaviour
{
    public PlayerController player;
   public void BaseReturn()
   {
        Debug.Log("Return");
        player.playerCam.transform.position = new Vector3(player.startPosition.transform.position.x, player.playerCam.transform.position.y, player.startPosition.transform.position.z);
   }
}
