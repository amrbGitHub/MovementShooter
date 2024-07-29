using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) 
        {
            
            CheckAndReplenish(other.gameObject.GetComponentInChildren<Health>());

        }
    }

    public void CheckAndReplenish(Health playerHealth)
    {
        if (playerHealth != null)
        {
            if (playerHealth.hitPoints < playerHealth.maxHealth)
            {
                playerHealth.hitPoints += replenishAmount;
                Destroy(this.gameObject);
            }
        }
    }
     public void RotateObject()
    {
        float yPos = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.time * dropSpeed) + 1) * 0.5f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}


