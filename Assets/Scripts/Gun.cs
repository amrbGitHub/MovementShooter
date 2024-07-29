using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem shootingParticleSystem;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject bulletHole;
    [SerializeField]
    private TrailRenderer bulletTrail;
    [SerializeField]
    private float shotDelay = 0.5f;
    [SerializeField]

    private LayerMask playerMask;
    private float lastShootTime;
    public int magCount = 1;
    public static RaycastHit hit;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && magCount > 0)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        if (lastShootTime + shotDelay < Time.time)
        {
            shootingParticleSystem.Play();
            Vector3 direction = GetDirection();

            if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit,float.MaxValue,playerMask))
            {
                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position,Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                lastShootTime = Time.time;
            }
            magCount--;
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        direction.Normalize();

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }
    
}
