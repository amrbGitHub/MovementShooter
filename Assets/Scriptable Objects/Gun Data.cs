using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    [Header("Reloading")]
    public float currentAmmo;
    public float magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

    

 

   
}
