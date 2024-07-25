using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ragdollRoot;
    [SerializeField] private bool startRagdoll = false;

    public Rigidbody[] rigidbodies;
    private CharacterJoint[] joints;
    private Collider[] colliders;
    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        joints = GetComponentsInChildren<CharacterJoint>();
        colliders = GetComponentsInChildren<Collider>();

      
    }

    public void EnableRagdoll()
    {
      animator.enabled = false;
        foreach (CharacterJoint joint in joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
        }
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
        foreach(CharacterJoint joint in joints)
        {
            joint.enableCollision = false;
        }
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }    
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!startRagdoll)
            {
                startRagdoll = true;
            }
            else if (startRagdoll)
            {
                startRagdoll = false;
            }
        }

        if (startRagdoll)
        {
            EnableRagdoll();
        }
        else
        {
            EnableAnimator();
        }
    }
}
