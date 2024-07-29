using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int hitPoints;
    public int maxHealth;

    private void Update()
    {
        maintainHealth();
    }

    private void maintainHealth()
    {
        if (hitPoints > maxHealth)
        {
            hitPoints = maxHealth;
        }
    }

}
