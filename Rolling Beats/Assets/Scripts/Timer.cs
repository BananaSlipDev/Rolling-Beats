using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{    
    private Image loadingBar;
    private float totalSongSeconds;

    private void Start()
    {
        loadingBar = GetComponent<Image>();
        loadingBar.fillAmount = 0f; 
        totalSongSeconds = Conductor.instance.musicSource.clip.length;
    }

    private void Update()
    {
        float passedTime = Conductor.instance.songPosition;

        if(passedTime > 0.5)
            loadingBar.fillAmount = passedTime/totalSongSeconds; 
    }
    

}
