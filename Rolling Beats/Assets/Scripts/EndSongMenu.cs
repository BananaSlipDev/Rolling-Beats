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
    // Start is called before the first frame update


    void Start()
    {
        puntuacion.text = ""+SceneManager.instance.totalScore;
        //Hay que pasarle la canción que acabe de terminar

    }

    // Update is called once per frame
    void Update()
    {
        
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
