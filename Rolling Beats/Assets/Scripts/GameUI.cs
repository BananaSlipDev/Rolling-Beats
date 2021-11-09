using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    public GameObject UIGame, PauseMenu;

    // TextMeshPro to update
    private TMP_Text currentScoreTXT;
    private TMP_Text currentComboTXT;

    void Start()
    {
        instance = this;

        currentScoreTXT = UIGame.transform.Find("Points_Background").transform.Find("ScoreTXT").GetComponent<TMP_Text>();
        currentComboTXT = UIGame.transform.Find("Combo_Background").transform.Find("ComboTXT").GetComponent<TMP_Text>();

        PauseMenu.SetActive(false);
        UIGame.SetActive(true);
    }

    // Updates UIs text. Called from SceneManager
    public void UpdateScoreAndComboText(string newScore, string newCombo)
    {
        currentScoreTXT.SetText(newScore);
        currentComboTXT.SetText(newCombo);
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

    public void RestartSong() {
        ResumeGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMenu() {
        ResumeGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
