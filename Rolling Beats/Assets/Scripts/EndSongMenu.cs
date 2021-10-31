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

        //Hay que pasarle la canción que acabe de terminar

    }

    private void Awake()
    {
        messageT = PlayFabManager.SharedInstance.messageText;
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    public void RestartSong(string song) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(song);
    }
    
}
