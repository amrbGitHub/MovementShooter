using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 24f;
    [SerializeField] float duration = 3f;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StartCoroutine(CloseAfterSomeSeconds(duration));    
    }

    private IEnumerator CloseAfterSomeSeconds(float duration)
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
