using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{    
    private Image loadingBar;
    private float totalSongSeconds;
    private bool clockTimes;

    private void Start()
    {
        clockTimes = false;
        loadingBar = GetComponent<Image>();
        loadingBar.fillAmount = 0f; 
        totalSongSeconds = Conductor.instance.musicSource.clip.length;
    }

    private void Update()
    {
        if (!clockTimes)
        {
            float passedTime = Conductor.instance.songPosition;

            if (passedTime > 0.5)
            {
                Debug.Log("Reloj: " + passedTime + "/" + totalSongSeconds + "=" + passedTime / totalSongSeconds);
                loadingBar.fillAmount = passedTime / totalSongSeconds;
                if (passedTime / totalSongSeconds >= 1)
                {
                    clockTimes = true;
                }
            }
        }
    }
}
