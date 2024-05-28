using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Models;

public class src_CharacterController : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;
    public Vector2 inputMovement;
    public Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

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
        newCharacterRotation = transform.localRotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Confined;

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
    }

    private void CalculateView()
    {
        //Rotate around Y axis
        newCharacterRotation.y += playerSettings.ViewXSens * inputView.x * Time.deltaTime;
        transform.rotation = Quaternion.Euler(newCharacterRotation); // returns rotation
        // Rotate around X axis
        newCameraRotation.x += playerSettings.ViewYSens * (playerSettings.ViewYInverted ? -inputView.y : inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        // We move the camera around the y and x axis 
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation); 
    }

    private void CalculateMovement()
    {
        // Calculate speed 
        var vertialSpeed = playerSettings.walkingForwardSpeed * inputMovement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.walkingStrafeSpeed * inputMovement.x * Time.deltaTime;

        // set speeds in a new Vector3
        var newMovementSpeed = new Vector3(horizontalSpeed, 0, vertialSpeed);
        // Makes sure that this is relative to the player's rotation
        newMovementSpeed = transform.TransformDirection(newMovementSpeed);
        // Moves player
        characterController.Move(newMovementSpeed);

    }
}
