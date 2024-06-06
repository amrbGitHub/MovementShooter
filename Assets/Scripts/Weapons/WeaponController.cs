using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Models;

public class WeaponController : MonoBehaviour
{

    private src_CharacterController characterController;

    [Header("Settings")]
    public WeaponSettingsModel settings;

    bool isInitialized;
    Vector3 newWeaponRotation;
    Vector3 newWeaponVelocity;

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }
    public void Initialize(src_CharacterController CharacterController)
    {
        characterController = CharacterController;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized)
        {
            return;
        }

        //Rotate around Y axis
        newWeaponRotation.y += settings.swayAmount * (settings.swayXInverted ? -characterController.inputView.x : characterController.inputView.x) * Time.deltaTime;
        // Rotate around X axis
        newWeaponRotation.x += settings.swayAmount * (settings.swayYInverted ? -characterController.inputView.y : characterController.inputView.y) * Time.deltaTime;

        // newWeaponRotation.x = Mathf.Clamp(newWeaponRotation.x, viewClampYMin, viewClampYMax);

        // We move the camera around the y and x axis 
        transform.rotation = Quaternion.Euler(newWeaponRotation); // returns rotation
    }

}
