using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Song-specific parameters")]
    public float songBPM;               //Song beats per minute, determined by the song to sync up
    public float firstBeatOffset;   //The offset to the first beat of the song in seconds
    public float beatsShownInAdvance;

    [HideInInspector]
    public float secPerBeat;            //Number of seconds for each song in a beat
    public float beatPerSec;            //Number of beats for each song in a second
    //private int firstBeatOffsetIdx = 0;     //The offset to the first beat of the song in seconds
    
    [HideInInspector]
    public float songPosition;      //Current song position, in seconds
    private float songPosInBeats;    //Current song position, in beats

    private float[] notes;          //Position-in-beats of notes in the song
    private int nextIndex = 0;      //Index of the next note to be spawned

    //Seconds passed since song started
    private float dspSongTime;

    //AudioSource that will play the music
    public AudioSource musicSource;
    public AudioListener musicListener;


    // Writing variables
    [SerializeField] private string songFileName; // MUST BE WRITTEN FROM THE INSPECTOR
    private string generalPath = "Assets/Audio/TextFileSongs/";
    private string songFilePath;

    public List<double> notesPositions;

    private void Start()
    {
        instance = this;

        notesPositions = new List<double>();

        //Load the AudioSource to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate seconds in each beat and viceversa
        secPerBeat = 60f / songBPM;
        beatPerSec = songBPM / 60f;

        //Record the time when music starts
        dspSongTime = (float)AudioSettings.dspTime;


        // -- Reading the song file --
        songFilePath += generalPath + songFileName;
        StreamReader reader = new StreamReader(songFilePath);
        string text = "";

        text = reader.ReadLine();
        while (text != null)
        {
            notesPositions.Add(double.Parse(text));
            text = reader.ReadLine();
        }

        reader.Close();

        SceneManager.instance.musicStarted = true;

    }

    private void Update()
    {
        if(SceneManager.instance.musicStarted)
        {
            songPosition = (float)(AudioSettings.dspTime - dspSongTime + firstBeatOffset);
            songPosInBeats = songPosition / secPerBeat;


            if(nextIndex >= notesPositions.Count)
            {
                // A MODIFICAR
                StartCoroutine(SceneManager.instance.GameOver());
            }
            else if (System.Math.Round(songPosition, 2) > notesPositions[nextIndex])
            {
                NoteSpawnerController.instance.SpawnNote(NoteSpawnerController.instance.GetSpawnerTopPosition());
                nextIndex++;
            }

        }

    }

}
