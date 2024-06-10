using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Models;
public class Wallrunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("References")]
    private Transform orientation;
    private src_CharacterController characterController;
    private CharacterController character;
    public PlayerSettingsModel settings;

    private void Start()
    {
        characterController = GetComponent<src_CharacterController>();
        character = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckForWall();
    }
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {

        }
    }

    private void StartWallRun()
    {

    }

    private void WallRunningMovement()
    {

    }

    private void StopWallRun()
    {

    }
}
