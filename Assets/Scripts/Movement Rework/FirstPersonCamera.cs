using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float mouseSens = 2f;
    private float cameraVerticalRotation = 0f;

    bool lockedCursor = true;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Fetch Mouse Inputs
        float inputX = Input.GetAxis("Mouse X") * mouseSens;
        float inputY = Input.GetAxis("Mouse Y") * mouseSens;
        
        // Rotate the camera around local X axis 
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotate the camera around local Y axis 
        player.Rotate(Vector3.up * inputX);
       
    }
}
