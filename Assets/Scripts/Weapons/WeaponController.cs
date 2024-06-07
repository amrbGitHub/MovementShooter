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

    bool isInitialized;

    [Header("ADS Settings")]
    [SerializeField] private Transform normalGunPos;
    [SerializeField] private Transform adsGunPos;
    [SerializeField] private float adsTime;
    private float elapsedTime;


    private void Start()
    {
        elapsedTime += Time.deltaTime;


    }
    public void Initialize(src_CharacterController CharacterController)
    {
        characterController = CharacterController;
        isInitialized = true;
    }

    private void Update()
    {
        transform.position = normalGunPos.position;
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
       

    }

    private void Aim()
    {
        float percentageCompelete = elapsedTime / adsTime;

        transform.position = Vector3.Lerp(normalGunPos.position, adsGunPos.position, percentageCompelete);
    }
}

