using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{

    [SerializeField] private const int UNITS_TO_MOVE = 2; // Unity-space units to move the notes (in other words, speed)

    //Interval for Unity-space degrees to rotate the notes (rotation speed)
    [SerializeField] private const int MIN_DEGREES_TO_ROTATE = 10;
    [SerializeField] private const int MAX_DEGREES_TO_ROTATE = 60;


    void Update()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        if (SceneManager.instance.musicStarted)
        {
            this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM
            this.transform.Rotate(0f, 0f, Random.Range(MIN_DEGREES_TO_ROTATE, MAX_DEGREES_TO_ROTATE) * Time.deltaTime);     // Random rotation speed
        }
    }
}