using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;



public class CheckMobileManager : MonoBehaviour
{
    bool isMobile = true;
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        static extern bool IsMobile();
#endif
    
    
    public GameObject login;
    public GameObject mobile;
 
    
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
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
