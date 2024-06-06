using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    private DefaultInput defaultInput;

    private void Awake()
    {
        defaultInput = new DefaultInput();
        defaultInput.Gun.Reload.performed += e => reloadInput?.Invoke();
        defaultInput.Enable();

        
    }

    private void Update()
    {
     
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
        
    }
}
