using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Models;

public class WeaponController : MonoBehaviour
{

    private src_CharacterController characterController;

    [Header("Settings")]
    public WeaponSettingsModel settings;
    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

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
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //calculate target rotation 
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);

    }
}
