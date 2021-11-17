using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSongMenu : MonoBehaviour
{
    public TextMeshProUGUI puntuacion;
    public TextMeshProUGUI messageT;


    void Start()
    {
        puntuacion.text = "YOUR SCORE: "+SceneManager.instance.totalScore;
        PlayFabManager.SharedInstance.AddVC();
        PlayFabManager.SharedInstance.getCurrency();

        //Hay que pasarle la canción que acabe de terminar
        // Usar DontDestroyOnLoad
    }

    private void Awake()
    {
        messageT = PlayFabManager.SharedInstance.messageText;
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    // CAMBIAR
    public void RestartSong(string song) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(song);
    }
    
}
