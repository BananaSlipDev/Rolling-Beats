using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class CheckMobileManager : MonoBehaviour
{
    public static CheckMobileManager SharedInstance;
    private bool isMobile;


    public bool IsMobileGet
    {
        get => isMobile;
        
    }
    
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern bool IsMobile();
#endif
    
    
    public GameObject login;
    public GameObject mobile;
    public GameObject keyboardMobile;
    public GameObject keyboardManager; 

    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
            isMobile = IsMobile();
#endif

        if (isMobile)
        {
            login.SetActive(false);
        }
        else
        {
            
            mobile.SetActive(false);
            keyboardMobile.SetActive(false);
            keyboardManager.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
