using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    [SerializeField]
    private const int UNITS_TO_MOVE = 2; // Unity-space units to move the notes (in other words, speed)
    void Update()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        if (SceneManager.instance.musicStarted)
        {
            this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f);
        }
    }
}
