using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    [SerializeField] float walkingSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float jumpThrust = 20f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] float groundCheckRadius;
    [SerializeField] Transform groundCheckObject;
  

    // Start is called before the first frame update
    void Start()
    {
        // Fetch Rigidbody from game object
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveWithInputs();
     
       
    }

    private void Update()
    {
        Jump();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckObject.position, groundCheckRadius);
    }

    void MoveWithInputs()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        // store user inputs in movement vector
        Vector3 inputs = transform.right *  inputX + transform.forward * inputY;
        inputs.Normalize();
        // check if sprint button pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // multiply speed but sprint multiplier
            speed = sprintSpeed;
        }
        else
        {
            speed = walkingSpeed;
        }
        //Apply movement vector which is multiplied by deltaTime and speed
        rb.MovePosition(transform.position + inputs * Time.deltaTime * speed);
    }

    void Jump()
    {
        // Get key input to jump 
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            // Add jump force if jump button is pressed
            rb.AddForce(transform.up * jumpThrust, ForceMode.Impulse);
            
        }
    }

    bool GroundCheck()
    {
        // returns if colliding with anything thats NOT the player
        return Physics.CheckSphere(groundCheckObject.position, groundCheckRadius, ~playerMask);
    }
}
