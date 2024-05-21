using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] private Transform cam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cam.position;
        transform.rotation = cam.rotation;
    }
}
