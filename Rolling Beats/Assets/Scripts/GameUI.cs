using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{

    public GameObject UIGame, PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu.SetActive(false);
        UIGame.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame() {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        UIGame.SetActive(false);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        UIGame.SetActive(true);
    }

    public void RestartSong(string song) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(song);
    }

    public void GoToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
