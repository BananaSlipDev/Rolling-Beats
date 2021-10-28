using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject UIGame, PauseMenu;

    void Start()
    {
        PauseMenu.SetActive(false);
        UIGame.SetActive(true);
    }


    public void PauseGame() {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        UIGame.SetActive(false);
        Conductor.instance.musicSource.Pause();
        AudioListener.pause = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        UIGame.SetActive(true);
        Conductor.instance.musicSource.Play();
        AudioListener.pause = false;
    }

    public void RestartSong(string song) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(song);
    }

    public void GoToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
