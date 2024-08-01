using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Health health;
    public int damageOutput;

    private void Start()
    {
        health = GetComponent<Health>();
    }

    public void TakeDamage(int damage)
    {
       health.hitPoints -= damage;
    }

    public int GiveDamage(int damage)
    {
        return damage;
    }
    private void Die()
    {
        if (health.IsDead())
        {
            Destroy(gameObject);
        }
    }





}