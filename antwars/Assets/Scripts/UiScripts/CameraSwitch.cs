using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public PlayerController player;
    public void Swap()
    {
        if (player.soldiers.Count != 0 && player.playerCam.isActiveAndEnabled)
        {
            player.playerCam.gameObject.SetActive(false);
            player.soldiers[0].antCam.gameObject.SetActive(true);
        }
        else if(!player.playerCam.isActiveAndEnabled)
        {
            player.soldiers[0].antCam.gameObject.SetActive(false);
            player.playerCam.gameObject.SetActive(true);
        }
    }
}
