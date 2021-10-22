using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameO;
    public GameObject leaderboard;
    
    
    private void Awake()
    {
        PlayFabManager.SharedInstance.leaderboardUI = GameObject.FindGameObjectWithTag("Leaderboard");
        PlayFabManager.SharedInstance.leaderboardUI.SetActive(false);
        
    }

    private void Start()
    {
        
       
    }

    // Update is called once per frame
   public void OnclickGameOver()
    {
        gameO.SetActive(false);
        PlayFabManager.SharedInstance.leaderboardUI.SetActive(true);
        
    }
}
