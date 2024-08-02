using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int hitPoints;
    public int maxHealth;
    private int noHealth = 0;

    private bool isDead = false;

    private void Update()
    {
        maintainHealth();
        UIManager.Instance.healthText.text = $"{hitPoints}";
    }

    private void maintainHealth()
    {
        if (hitPoints > maxHealth)
        {
            hitPoints = maxHealth;
        }
        else if (hitPoints <= noHealth)
        {
            hitPoints = noHealth;
            isDead = true;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }


     
}
