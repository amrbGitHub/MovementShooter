using System.Runtime.CompilerServices;
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

    [Header("ADS Settings")]
    [SerializeField] private Transform normalGunPos;
    [SerializeField] private Transform adsGunPos;
    [SerializeField] private float adsTime;
    private float elapsedTime;

    bool isInitialized;



    private void Start()
    {
        
    }
    public void Initialize(src_CharacterController CharacterController)
    {
        characterController = CharacterController;
        isInitialized = true;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
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

        if (Input.GetMouseButton(1))
        {
            Aim();
        }
        else
        {
            transform.localPosition = normalGunPos.localPosition;
        }

    }

    private void Aim()
    {
        float percentageCompelete = elapsedTime / adsTime;

        transform.localPosition = Vector3.Lerp(normalGunPos.localPosition, adsGunPos.localPosition, percentageCompelete);
    }
}


