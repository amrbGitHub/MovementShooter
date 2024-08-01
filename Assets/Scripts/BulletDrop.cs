using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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
    [SerializeField]
    private float respawnTimer;

    private bool isCollected = false;



    // Update is called once per frame
    void Update()
    { 
        RotateObject();

        if (isCollected)
        {
            gameObject.SetActive(false);
        }
        else if (!isCollected) 
        {
            gameObject.SetActive(true);
        }
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            CheckRestock(other.gameObject.GetComponentInChildren<Gun>());
            StartCoroutine(RespawnDrop());
        }
    }

    private void CheckRestock(Gun playerAmmo)
    {
       if (playerAmmo != null)
       {
            if (playerAmmo.magCount < replenishAmount)
            {
                playerAmmo.magCount = replenishAmount;
            }
       }
    }

    private void RotateObject()
    {
        float yPos = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.time * dropSpeed) + 1) * 0.5f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate( 0, rotationSpeed * Time.deltaTime, 0);
    }

    IEnumerator RespawnDrop()
    {
        isCollected = true;
        yield return new WaitForSeconds(respawnTimer);
        isCollected = false;
        
    }


    
}
