using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private float KnockbackForce = 10f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            animator.enabled = false;
            Knockback();
        }
    }

   private void Knockback()
    {
        transform.position = -Vector3.forward * Time.deltaTime * KnockbackForce;
   }
}