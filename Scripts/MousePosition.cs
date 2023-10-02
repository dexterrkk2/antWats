using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Camera playerCam;
    public static Vector3 mousePosition;
    public LayerMask layerMask;
    public int playerClan;
    private void Update()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            mousePosition = raycastHit.point;
        }
    }
}
