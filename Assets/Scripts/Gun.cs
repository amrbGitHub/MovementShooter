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
    private float missTimer = 0.5f;


    private LayerMask playerMask = 8;
    private float lastShootTime;
    public int magCount = 1;
    public static RaycastHit hit;
    private Animator gunAnimator;

    private void Awake()
    {
        gunAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && magCount > 0)
        {
                Shoot();
            gunAnimator.Play("GunRecoil");
        }
        UIManager.Instance.ammoText.text = $"{magCount}";

    }
    private void Shoot()
    {
        if (lastShootTime + shotDelay < Time.time)
        {
            shootingParticleSystem.Play();
            magCount--;


            Vector3 direction = GetDirection();

            if (Physics.Raycast(bulletSpawnPoint.position, direction, out hit,float.MaxValue, ~playerMask))
            {
                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position,Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                lastShootTime = Time.time;

                if (hit.transform.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.transform.GetComponent<Enemy>();
                    enemy.TakeDamage(100);
                    magCount += 1;
                }
            }
            else
            {
                StartCoroutine(ToggleMissText());
            }
           
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
    private IEnumerator ToggleMissText()
    {
        UIManager.Instance.missText.enabled = true;
        yield return new WaitForSeconds(missTimer);
        UIManager.Instance.missText.enabled = false;

    }



}
