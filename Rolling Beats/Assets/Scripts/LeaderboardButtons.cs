using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardButtons : MonoBehaviour
{

    public Button sendPunt;

    public Button getPunt;

    public InputField score;
    
    // Start is called before the first frame update

    private void Awake()
    {
        sendPunt.onClick.AddListener(enviarPuntuacion);
        getPunt.onClick.AddListener(recibirPuntuacion);
        PlayFabManager.SharedInstance.rowsParent = GameObject.Find("TAble");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void recibirPuntuacion()
    {
        PlayFabManager.SharedInstance.GetLeaderboard();
    }

    void enviarPuntuacion()
    {
        PlayFabManager.SharedInstance.SendLeaderboard(int.Parse(score.text));
    }
    
}
