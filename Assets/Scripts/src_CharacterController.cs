using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class src_CharacterController : MonoBehaviour
{
    private DefaultInput defaultInput;
    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector2 newCameraRotation;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70f;
    public float viewClampYMax = 80f;
    private void Awake()
    {
        // Create an instance of input 
        defaultInput = new DefaultInput();

        // check if any WASD input has been performed, create reference to event into a Vector2 and read it as a Vector2
        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        // check if any Mouse movement has been performed, create reference to event into a Vector2 and read it as a Vector2
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();

        // Need this to make Input system work
        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
    }

    private void CalculateView()
    {

        newCameraRotation.x += playerSettings.ViewYSens * inputView.y * Time.deltaTime;

        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        // We move the camera around the y and x axis 
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {

    }
}
