using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BulletDrop : MonoBehaviour
{
    [SerializeField]
    private int replenishAmount;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float dropSpeed;
    [SerializeField]
    private float MinHeight;
    [SerializeField]
    private float MaxHeight;

    // Update is called once per frame
    void Update()
    { 
        RotateObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CheckRestock(other.gameObject.GetComponentInChildren<Gun>());
        }
    }

    private void CheckRestock(Gun playerAmmo)
    {
       if (playerAmmo != null)
       {
            if (playerAmmo.magCount < replenishAmount)
            {
                playerAmmo.magCount = replenishAmount;
                Destroy(this.gameObject);
            }
       }
    }

    private void RotateObject()
    {
        float yPos = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.time * dropSpeed) + 1) * 0.5f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate( 0, rotationSpeed * Time.deltaTime, 0);
    }
}
