using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition : MonoBehaviour
{
    public Camera mainCam;
    public static Vector3 mousePosition;
    public LayerMask layerMask;
    private void Update()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            mousePosition = raycastHit.point;
        }
    }
}
