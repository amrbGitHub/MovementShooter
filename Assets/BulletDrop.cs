using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BulletDrop : MonoBehaviour
{
    private int replenishAmount = 2;
    [SerializeField] 
    private float rotationSpeed = 100f;
    [SerializeField]
    private float dropSpeed;
    [SerializeField]
    private float MinHeight;
    [SerializeField]
    private float MaxHeight;
    // Update is called once per frame
    void Update()
    {
        if (Gun.hit.transform != null)
        {
            if (Gun.hit.transform.gameObject == transform.gameObject)
            {
                Restock();
            }
        }

        RotateObject();
    }

    private void Restock()
    {
        Gun.magCount = Gun.magCount + replenishAmount;
        Destroy(gameObject);
    }

    private void RotateObject()
    {
        float yPos = Mathf.Lerp(MinHeight, MaxHeight, (Mathf.Sin(Time.time * dropSpeed) + 1) * 0.5f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        transform.Rotate( rotationSpeed * Time.deltaTime, 0, rotationSpeed * Time.deltaTime);
    }
}
