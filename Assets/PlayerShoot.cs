using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Camera fpscam;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 rayOrigin = fpscam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, fpscam.transform.forward, out hit))
            {
                Debug.Log("Hit: " + hit.normal);
            }
        }
    }
}
