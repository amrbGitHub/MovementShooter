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


    [Header("Gravity")]

    public float gravityAmount;
    public float gravityMin;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;

    [Header("Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public float cameraStandingHeight;
    public float cameraCrouchHeight;
    public float cameraProneHeight;

    private float cameraHeight;
    private float cameraHeightVelocity; // for smooth damping

    private void Awake()
    {
        // Create an instance of input 
        defaultInput = new DefaultInput();

        // check if any WASD input has been performed, create reference to event into a Vector2 and read it as a Vector2
        defaultInput.Character.Movement.performed += e => inputMovement = e.ReadValue<Vector2>();
        // check if any Mouse movement has been performed, create reference to event into a Vector2 and read it as a Vector2
        defaultInput.Character.View.performed += e => inputView = e.ReadValue<Vector2>();
        // Check if jump button has been performed, call Jump method
        defaultInput.Character.Jump.performed += e => Jump();

        // Need this to make Input system work
        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Confined;

        characterController = GetComponent<CharacterController>();

        cameraHeight = cameraHolder.localPosition.y;
    }

    private void Update()
    {
        CalculateView();
        CalculateMovement();
        CalculateJump();
        CalculateCameraHeight();
       
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


        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }


        if (playerGravity < -0.1 && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }


        newMovementSpeed.y += playerGravity;

        newMovementSpeed += jumpingForce * Time.deltaTime;

        // Moves player
        characterController.Move(newMovementSpeed);

    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFallOff);
    }

    private void CalculateCameraHeight()
    {
        var stanceHeight = cameraStandingHeight;

        if (playerStance == PlayerStance.Crouch)
        {
            stanceHeight = cameraCrouchHeight;
        }
        else if (playerStance == PlayerStance.Prone) 
        {
            stanceHeight = cameraProneHeight;
        }

        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, stanceHeight, ref cameraHeightVelocity, playerStanceSmoothing);

        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);
    }

    private void Jump()
    {
       if (!characterController.isGrounded)
        {
            return;
        }
        // Jump 
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;

        playerGravity = 0;

    }
}
