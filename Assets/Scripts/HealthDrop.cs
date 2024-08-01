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
    [SerializeField]
    private float respawnTimer;
    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
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
                StartCoroutine(RespawnDrop());
            }
        }
    }
     public void RotateObject()
    {
        float yPos = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.time * dropSpeed) + 1) * 0.5f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }

    IEnumerator RespawnDrop()
    {
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        yield return new WaitForSeconds(respawnTimer);
        boxCollider.enabled = true;
        meshRenderer.enabled = true;
    }

}


