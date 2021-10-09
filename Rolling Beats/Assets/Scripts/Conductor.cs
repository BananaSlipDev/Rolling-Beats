using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{

    //Song-specific parameters
    public float songBPM;           //Song beats per minute, determined by the song to sync up
    public float firstBeatOffset;   //The offset to the first beat of the song in seconds
    public float secPerBeat;        //Number of seconds for each song to beat

    
    public float songPosition;      //Current song position, in seconds
    public float songPosInBeats;    //Current song position, in beats

    //Seconds passed since song started
    public float dspSongTime;

    //AudioSource that will play the music
    public AudioSource musicSource;

    private void Start()
    {
        //Load the AudioSource to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate seconds in each beat
        secPerBeat = 60f / songBPM;

        //Record the time when music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start music
        musicSource.Play();
    }

    private void Update()
    {
        //How many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //How many beats since song started
        songPosInBeats = songPosition / secPerBeat;
    }

}
