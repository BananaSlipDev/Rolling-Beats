using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Song-specific parameters")]
    public float songBPM;               //Song beats per minute, determined by the song to sync up
    public float firstBeatOffset;       //The offset to the first beat of the song in seconds
    public float secondsTilEnd;         //Seconds until the last note spawned for the song to end

    [HideInInspector] public float secPerBeat;            //Number of seconds for each song in a beat
    [HideInInspector] public float beatPerSec;            //Number of beats for each song in a second    

    [HideInInspector] public float songPosition;      //Current song position, in seconds
    private float songPosInBeats;    //Current song position, in beats

    private float dspSongTime;      //Seconds passed since song started
    private int nextIndex = 0;      //Index of the next note to be spawned

    // Audio components
    [HideInInspector] public AudioSource musicSource;
    [HideInInspector] public AudioListener musicListener;

    // Text asset that includes the notes
    public TextAsset textAsset;

    [SerializeField] private List<double> notesPositions;
    [SerializeField] private List<int> notesLines;   // 0 - Top      /  1 - Bottom
    [SerializeField] private List<String> notesType;    // N - Normal   /  L - Long  / LE - Long End

    private string[] texto;
    private string[] texto2;

    public bool wasWrited = false;

    private void Awake()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
        instance = this;

        notesPositions = new List<double>();
        notesLines = new List<int>();

        //Load the AudioSource to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();
        musicListener = GetComponent<AudioListener>();

        //Calculate seconds in each beat and viceversa
        secPerBeat = 60f / songBPM;
        beatPerSec = songBPM / 60f;

        // Reads the file and stores the notes
        StoreNotes();
    }

    private void Start()
    {
        SceneManager.instance.musicStarted = true;
    }

    private void Update()
    {
        if (!wasWrited)
        {
            SceneManager.instance.musicStarted = true;
            wasWrited = true;
        }

        songPosition = musicSource.time + firstBeatOffset;
        songPosInBeats = songPosition / secPerBeat;

        // Spawns a note if it has to.
        SpawnNote();


    }

    private bool SpawnNote() // Spawns notes. Returns true if spawns, false if not.
    {
        if (nextIndex >= notesPositions.Count)
        {
            // A MODIFICAR
            StartCoroutine(SceneManager.instance.GameOver(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
        }
        else if (System.Math.Round(songPosition, 2) > notesPositions[nextIndex]) // If there are notes left...
        {
            if (notesLines[nextIndex] == 0) // Top lane
            {
                if (notesType[nextIndex].Equals("N"))
                    NoteSpawnerController.instance.SpawnNote(NoteSpawnerController.instance.GetSpawnerTopPosition());
                else if (notesType[nextIndex].Equals("L"))
                {
                    NoteSpawnerController.instance.SpawnLongNote(NoteSpawnerController.instance.GetSpawnerTopPosition());
                }    
                else if (notesType[nextIndex].Equals("LE"))
                {
                    NoteSpawnerController.instance.SpawnLongNoteEnd(NoteSpawnerController.instance.GetSpawnerTopPosition());
                }
                    
            }
            else if(notesLines[nextIndex] == 1) // Bottom lane
            {
                if (notesType[nextIndex].Equals("N"))
                    NoteSpawnerController.instance.SpawnNote(NoteSpawnerController.instance.GetSpawnerBottomPosition());
                else if (notesType[nextIndex].Equals("L"))
                {
                    NoteSpawnerController.instance.SpawnLongNote(NoteSpawnerController.instance.GetSpawnerBottomPosition());
                }
                else if (notesType[nextIndex].Equals("LE"))
                {
                    NoteSpawnerController.instance.SpawnLongNoteEnd(NoteSpawnerController.instance.GetSpawnerBottomPosition());
                }
            }

            nextIndex++;
            return true; //A note was spawned
        }

        return false; //A note wasn't spawned
    }

    private void StoreNotes()
    {
        // Reads the file and stores the notes
        string textAssetTxt = textAsset.text;

        texto = textAssetTxt.Split('\n');
        for (int i = 0; i < texto.Length; i++)
        {
            texto2 = texto[i].Split('/');
            notesType.Add(texto2[0]);                   // Tipo
            notesLines.Add(int.Parse(texto2[1]));       // Carril
            notesPositions.Add(double.Parse(texto2[2]));// Segundo
        }
    }
}
