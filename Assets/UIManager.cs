using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The UIManager is null");
            }
            return instance;
        }
    }
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI missText;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        missText.enabled = false;
    }

    private void Start()
    {
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
