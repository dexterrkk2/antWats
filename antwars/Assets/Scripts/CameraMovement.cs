using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float zoom;
    float minFov = 15f;
    float maxFov= 90f;
    float sensitivity = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float fov = mainCamera.fieldOfView;
        if (Input.GetButtonDown("Fire2"))
        {
            transform.position = new Vector3(MousePosition.mousePosition.x, transform.position.y, MousePosition.mousePosition.z);
        }
        fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        mainCamera.fieldOfView = fov;
    }
}
