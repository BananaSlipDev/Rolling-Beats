using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Song-specific parameters")]
    public float songBPM;           //Song beats per minute, determined by the song to sync up
    public float firstBeatOffset = 0;   //The offset to the first beat of the song in seconds
    public int firstBeatOffsetIdx;   //The offset to the first beat of the song in seconds
    public float secPerBeat;        //Number of seconds for each song in a beat
    public float beatPerSec;        //Number of beats for each song in a second
    
    public float songPosition;      //Current song position, in seconds
    public float songPosInBeats;    //Current song position, in beats
    public float beatsShownInAdvance;

    public float[] notes;           //Position-in-beats of notes in the song
    int nextIndex = 0;              //Index of the next note to be spawned

    //Seconds passed since song started
    private float dspSongTime;

    //AudioSource that will play the music
    public AudioSource musicSource;

    private void Start()
    {
        instance = this;

        //Load the AudioSource to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate seconds in each beat and viceversa
        secPerBeat = 60f / songBPM;
        beatPerSec = songBPM / 60f;

        //Record the time when music starts
        dspSongTime = (float)AudioSettings.dspTime;
        
        
        // por alguna razón Unity peta con esto XD  así que guarda antes de ejecutar crack
        //notes = new float[musicSource.clip.samples * musicSource.clip.channels];
        //musicSource.clip.GetData(notes, firstBeatOffsetIdx);

    }

    private void Update()
    {
        if(SceneManager.instance.musicStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
            songPosInBeats = songPosition / secPerBeat;


            // Note spawning
            if(nextIndex < notes.Length && notes[nextIndex] < songPosInBeats)
            {
                NoteSpawnerController.instance.SpawnNote();

                // initialize the fields of music note
                nextIndex++;
            }
        }

    }

}
