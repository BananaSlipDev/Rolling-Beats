using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSongMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Hay que pasarle la canción que acabe de terminar
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }


    public void RestartSong(string song) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(song);
    }
    
}
