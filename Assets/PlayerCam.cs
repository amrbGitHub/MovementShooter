using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    [SerializeField] private float senX;
    [SerializeField] private float senY;
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // gather the mouse input
        float MouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float MouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += MouseX;

        xRotation -= MouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation 
        transform.rotation = Quaternion.Euler(xRotation, yRotation,0);    
        orientation.rotation = Quaternion.Euler(0, yRotation,0); // rotates player along the y axis

    }
}
