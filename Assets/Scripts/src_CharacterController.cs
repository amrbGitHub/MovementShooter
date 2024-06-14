using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static Models;

public class src_CharacterController : MonoBehaviour
{
    private CharacterController characterController;
    private DefaultInput defaultInput;
    private Vector2 inputMovement;
    [HideInInspector]
    public Vector2 inputView;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform cameraHolder;
    public Transform feetTransform;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70f;
    public float viewClampYMax = 80f;
    public LayerMask playerMask;


    Vector3 velocity;

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
    public CharacterStance playerStandStance;
    public CharacterStance playerCrouchStance;
    public CharacterStance playerProneStance;
    private float stanceCheckErrorMargin = 0.05f ;


    private float cameraHeight;
    private float cameraHeightVelocity; // for smooth damping

    // Player Stance smoothing
    private Vector3 stanceCapsuleCenterVelocity;
    private float stanceCapsuleHeightVelocity;
    
    private bool isSprinting;
    Action OnNextDrawGizmos;


    [SerializeField] private float slideDuration;

    private Vector3 newMovementSpeed;
    private Vector3 newMovementSpeedVelocity;

    [Header("Weapon")]
    public WeaponController currentWeapon;

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
        // Check if Crouch button has been performed, call Crouch method
        defaultInput.Character.Crouch.performed += e => Crouch();
        // Check if Prone button has been performed, call Prone method
        defaultInput.Character.Prone.performed += e => Prone();
        // Check if Sprint button has been performed, call Sprint method
        defaultInput.Character.Sprint.performed += e => ToggleSprint();
        defaultInput.Character.SprintReleased.performed += e => StopSprint();
        // Check if Slide button has been performed, call Slide method
        defaultInput.Character.Slide.performed += e => Slide();



        // Need this to make Input system work
        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        characterController = GetComponent<CharacterController>();

        cameraHeight = cameraHolder.localPosition.y;

        if (currentWeapon)
        {
            currentWeapon.Initialize(this);
        }
    }

    private void Update()
    {
        CalculateView();
        CalculateStance();

        UpdateSlopeSliding();





    }

  

    private void FixedUpdate()
    {
        CalculateMovement();
        CalculateJump();
    }

    private void CalculateView()
    {
        //Rotate around Y axis
        newCharacterRotation.y += playerSettings.ViewXSens * (playerSettings.ViewXInverted ? -inputView.x : inputView.x) * Time.deltaTime;
        transform.rotation = Quaternion.Euler(newCharacterRotation); // returns rotation
        // Rotate around X axis
        newCameraRotation.x += playerSettings.ViewYSens * (playerSettings.ViewYInverted ? -inputView.y : inputView.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        // We move the camera around the y and x axis 
        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation); 
    }

    private void CalculateMovement()
    {

     

        if (inputMovement.y <= 0.1f)
        {
            isSprinting = false;
        }
        float verticalSpeed = playerSettings.walkingForwardSpeed;
        float horizontalSpeed = playerSettings.walkingStrafeSpeed;

        if (isSprinting)
        {
            verticalSpeed = playerSettings.runningForwardSpeed;
            horizontalSpeed = playerSettings.runningStrafeSpeed;
        }



        // Effectors

        switch (playerStance)
        {
            case PlayerStance.Crouch:
                playerSettings.speedEffector = playerSettings.crouchSpeedEffector;
                break;
            case PlayerStance.Prone:
                playerSettings.speedEffector = playerSettings.proneSpeedEffector;
                break;
            case PlayerStance.Sliding:
                playerSettings.speedEffector = playerSettings.slidingSpeedEffector;
                break;
            default:
                playerSettings.speedEffector = 1;
                break;



        }
      
      



        verticalSpeed *= playerSettings.speedEffector;
        horizontalSpeed *= playerSettings.speedEffector;

        // set speeds in a new Vector3, using smooth damp to get smoother movement
        newMovementSpeed = Vector3.SmoothDamp(newMovementSpeed, new Vector3(horizontalSpeed * inputMovement.x * Time.deltaTime, 0, verticalSpeed * inputMovement.y * Time.deltaTime), ref newMovementSpeedVelocity, characterController.isGrounded ? playerSettings.MovementSmoothing : playerSettings.fallingSmoothing);
        // Makes sure that this is relative to the player's rotation
       Vector3 movementSpeed = transform.TransformDirection(newMovementSpeed);


        if (playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount * Time.deltaTime;
        }


        if (playerGravity < -0.1 && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }


        movementSpeed.y += playerGravity;

        movementSpeed += jumpingForce * Time.deltaTime;


        // Moves player
        characterController.Move(movementSpeed);

      

    }

    private void CalculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.jumpingFallOff);
    }

  

    private void CalculateStance()
    {
        CharacterStance currentStance = playerStandStance;

        switch (playerStance)
        {
            case PlayerStance.Crouch:
                currentStance = playerCrouchStance;
                break;
            case PlayerStance.Prone:
                currentStance = playerProneStance;
                break;
            case PlayerStance.Sliding:
                currentStance = playerProneStance;
                break;
            default:
                currentStance = playerStandStance;
                break;


        }


        
        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, currentStance.cameraHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);

        characterController.height = Mathf.SmoothDamp(characterController.height, currentStance.stanceCollider.height, ref stanceCapsuleHeightVelocity, playerStanceSmoothing);
        characterController.center = Vector3.SmoothDamp(characterController.center, currentStance.stanceCollider.center, ref stanceCapsuleCenterVelocity, playerStanceSmoothing);
    }

    private void Jump()
    {
       if (!characterController.isGrounded || playerStance == PlayerStance.Prone)
       {
            return;
       }
  

       if (playerStance == PlayerStance.Crouch)
       {

            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }


            playerStance = PlayerStance.Stand;
            return;
       }

        // Jump
        jumpingForce = Vector3.up * playerSettings.jumpingHeight;

        playerGravity = 0;



    }

    private void OnDrawGizmos()
    {
        OnNextDrawGizmos?.Invoke();
        OnNextDrawGizmos = null;
    }


    private void UpdateSlopeSliding()
    {
        if (characterController.isGrounded)
        {
            float sphereCastVerticalOffset = characterController.height / 2 - characterController.radius;
            Vector3 castOrigin = transform.position - new Vector3(0, sphereCastVerticalOffset, 0);

            if (Physics.SphereCast(castOrigin, characterController.radius - .01f, Vector3.down, out RaycastHit hit, .1f, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
            {
                Collider collider = hit.collider;
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.black, 3f);

                OnNextDrawGizmos += () =>
                {
                    GUI.color = Color.black;
                    Handles.Label(transform.position + new Vector3(0, 2f, 0), "Angle:" + angle.ToString());
                };

                if (angle > characterController.slopeLimit)
                {
                }

            }

        }
    }
  
    private void Slide()
    {
       if (isSprinting)
       {
            StartCoroutine(CalculateSlide());
       } 
    }

    private IEnumerator CalculateSlide()
    {

        if (playerStance == PlayerStance.Prone) { playerStance = PlayerStance.Sliding; }
        Debug.Log("CURRENTLY SLIDING");
        yield return new WaitForSeconds(playerSettings.slideStamina);
        playerStance = PlayerStance.Stand;
        Debug.Log("STOPPED SLIDING");
    }

    private void Crouch()
    {
        if (playerStance == PlayerStance.Crouch)
        {
            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }

            playerStance = PlayerStance.Stand;
            return;
        }

        if (StanceCheck(playerCrouchStance.stanceCollider.height))
        {
            return;
        }
        playerStance = PlayerStance.Crouch;


    }

    private void Prone()
    {
        if (playerStance == PlayerStance.Prone)
        {

            if (StanceCheck(playerStandStance.stanceCollider.height))
            {
                return;
            }

            playerStance = PlayerStance.Stand;
            return;
        }
        playerStance = PlayerStance.Prone;

    }

    private bool StanceCheck(float stanceCheckHeight)
    {
        Vector3 start = new (feetTransform.position.x, feetTransform.position.y + characterController.radius + stanceCheckErrorMargin, feetTransform.position.z);
        Vector3 end = new (feetTransform.position.x, feetTransform.position.y - characterController.radius - stanceCheckErrorMargin + stanceCheckHeight, feetTransform.position.z);



        return Physics.CheckCapsule(start, end, characterController.radius, playerMask);
    }

    private void ToggleSprint()
    {
        if (inputMovement.y <= 0.1f)
        {
            isSprinting = false;
            return;
        }
        Debug.Log("SPRINTING");
        isSprinting = !isSprinting;
    }
    private void StopSprint()
    {
        if (playerSettings.sprintingHold)
        {
            isSprinting = false;
        }
        Debug.Log("NOT SPRTINTING");


    }

 

   
}
