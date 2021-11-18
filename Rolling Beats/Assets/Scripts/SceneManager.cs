using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance; //Public & Static instance for the SceneManager to be accessed by other scripts

    private const int FRAMERATE = 60;   // Target framerate for the game (FPS)

    [HideInInspector]
    public bool musicStarted = false;

    // Score parameters
    private const int PERFECT_SCORE = 100;
    private const int GREAT_SCORE = 50;
    private int combo = 1;

    private int perfects = 0; // Perfects achieved in the song, etc
    private int greats = 0;    
    private int misses = 0;
    
    public int totalScore = 0;

    private void Start()
    {
        //PlayFabManager.SharedInstance.actualScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        totalScore = 0;
        instance = this;
        
       
        PlayFabManager.SharedInstance.ActualLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        PlayFabManager.SharedInstance.getScoreAndLevel();

        // Sets a fixed framerate
        //Application.targetFrameRate = FRAMERATE;
    }


    public void ScoreNote(string scoreType) //Adds score note to the current score
    {
        int points = 0;

        switch (scoreType)
        {
            case "PERFECT":
                points = PERFECT_SCORE;
                perfects++;
                break;
            case "GREAT":
                points = GREAT_SCORE;
                greats++;
                break;
            default:
                break;
        }

        totalScore += points * combo;   // Adds the note points to the total score
        combo += 1;                     // Increases the combo

        // Updates UI
        GameUI.instance.UpdateScoreAndComboText(totalScore.ToString(), "x"+ combo.ToString());
    }

    public void Miss() //Resets the combo and... damages the health?
    {
        combo = 1;
        misses++;
        // ... damage the health ...

        // Updates UI
        GameUI.instance.UpdateScoreAndComboText(totalScore.ToString(), "x" + combo.ToString());
    }

    public void sendtotalScore(String sceneName)
    {
        PlayFabManager.SharedInstance.SendLeaderboard(totalScore, sceneName);
    }

    public IEnumerator GameOver(String sceneName)
    {
        yield return new WaitForSeconds(Conductor.instance.secondsTilEnd);
        sendtotalScore(sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndSong");

    }
}
