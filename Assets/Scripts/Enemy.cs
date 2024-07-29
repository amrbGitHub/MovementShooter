using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private Health health;
    public int damageOutput;
    public void Damage(int damage)
    {
        health.hitPoints -= damage;
    }

    
 

}