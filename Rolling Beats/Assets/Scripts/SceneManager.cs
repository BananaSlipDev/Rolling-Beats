using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance; //Public & Static instance for the SceneManager to be accessed by other scripts

    public bool musicStarted = false;

    [SerializeField]
    private List<GameObject> noteList = new List<GameObject>();
    private GameObject[] noteArray;


    // Score parameters
    private const int PERFECT_SCORE = 100;
    private const int GREAT_SCORE = 50;

    public static int totalScore = 0;
    private int combo = 1;

    
    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        // Keeps a track of the notes that are or have been on screen  ...  A�N NO LO USAMOS!!
        noteArray = GameObject.FindGameObjectsWithTag("Note");
        foreach (GameObject n in noteArray)
        {
            if (!noteList.Contains(n))
                noteList.Add(n);
        }

        /*
         * PRUEBA: inicia la cancion al pulsar cualquier tecla
         */
        if (!musicStarted && Input.anyKeyDown)
        {
            musicStarted = true;
            Conductor.instance.musicSource.Play();
        }
            
        ///////////////////////
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
        Debug.Log("Total score: " + totalScore);
    }

    public void Miss() //Resets the combo and... damages the health?
    {
        Debug.Log("Miss...");
        combo = 1;
        // ... 
    }
}
