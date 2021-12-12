using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileKeyboardManager : MonoBehaviour
{

    [SerializeField] private GameObject keyboard;

    [SerializeField] private InputField email;

    [SerializeField] private InputField pass;
    [SerializeField] private InputField username;
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayFabManager.SharedInstance.emailInput = email;
        PlayFabManager.SharedInstance.passwordInput = pass;
        PlayFabManager.SharedInstance.username = username;
    }

    // Update is called once per frame
    void Update()
    {
        if (email.isFocused)
        {
            openKeyboard();
            KeyboardScript.SharedInstance.TextField = email;
        }

        if (pass.isFocused)
        {
            openKeyboard();
            KeyboardScript.SharedInstance.TextField = pass;
        }

        if (username.isFocused)
        {
            openKeyboard();
            KeyboardScript.SharedInstance.TextField = username;
        }
    }

    public void openKeyboard()
    {
        keyboard.SetActive(true);
    }

   
}
