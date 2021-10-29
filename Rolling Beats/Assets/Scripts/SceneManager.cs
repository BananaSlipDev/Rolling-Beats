using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    [SerializeField]
    public static int totalScore = 0;


    private void Start()
    {
        totalScore = 0;
        instance = this;

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
                Debug.Log("Perfect!");
                break;
            case "GREAT":
                points = GREAT_SCORE;
                Debug.Log("Great!");
                break;
            default:
                break;
        }

        totalScore += points * combo;   // Adds the note points to the total score
        combo += 1;                     // Increases the combo
        //Debug.Log("Total score: " + totalScore);
    }

    public void Miss() //Resets the combo and... damages the health?
    {
        Debug.Log("Miss...");
        combo = 1;
        // ... 
    }

    public void sendtotalScore()
    {
        PlayFabManager.SharedInstance.SendLeaderboard(totalScore);
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(Conductor.instance.secondsTilEnd);
        sendtotalScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndSong");

    }
}
