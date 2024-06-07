using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform muzzle;

    float timeSinceLastShot;


    [Header("ADS Settings")]
    private Transform normalGunPos;
    [SerializeField] private Transform adsGunPos;
    [SerializeField] private float adsTime;
    private float elapsedTime;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            Debug.Log("RELOADING");
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;
        gunData.reloading = false;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    private void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {

                    Debug.Log(hitInfo.transform.name);

                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);

                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                 OnGunShot();

            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
        elapsedTime += Time.deltaTime;

        normalGunPos.position = transform.position;

        //transform.position = normalGunPos.position;

       if (Input.GetMouseButtonDown(1))
        {
            Aim();
        }




        Debug.DrawRay(muzzle.position, muzzle.forward, Color.green);
    }

    private void OnGunShot()
    {

    }

    private void Aim()
    {
        float percentageCompelete = elapsedTime / adsTime;

        transform.position = Vector3.Lerp(normalGunPos.position, adsGunPos.position, percentageCompelete);
    }
}
