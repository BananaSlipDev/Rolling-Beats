using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance;

    [Header("Song-specific parameters")]
    public float songBPM;               //Song beats per minute, determined by the song to sync up
    public float firstBeatOffset = 0;   //The offset to the first beat of the song in seconds
    public float beatsShownInAdvance;

    public float secPerBeat;            //Number of seconds for each song in a beat
    public float beatPerSec;            //Number of beats for each song in a second
    private int firstBeatOffsetIdx = 0;     //The offset to the first beat of the song in seconds
    

    private float songPosition;      //Current song position, in seconds
    private float songPosInBeats;    //Current song position, in beats
    

    public float[] notes;           //Position-in-beats of notes in the song
    private int nextIndex = 0;      //Index of the next note to be spawned

    //Seconds passed since song started
    private float dspSongTime;

    //AudioSource that will play the music
    public AudioSource musicSource;

    //// Job for sampling the song
    //public struct MyJob : IJob
    //{
    //    public AudioClip clip;
    //    public int beatOffsetIdx;
    //    public float[] notes;

    //    public void Execute()
    //    {
    //        notes = new float[clip.samples * clip.channels];
    //        clip.GetData(notes, beatOffsetIdx);
    //    }
    //}

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


        //// por alguna razon Unity peta con esto XD  asi que guarda antes de ejecutar crack
        //notes = new float[musicSource.clip.samples * musicSource.clip.channels];
        //musicSource.clip.GetData(notes, firstBeatOffsetIdx);
        //MyJob jobData = new MyJob();
        //jobData.clip = musicSource.clip;
        //jobData.beatOffsetIdx = firstBeatOffsetIdx;

        //JobHandle handle = jobData.Schedule();
        //handle.Complete();
        //notes = jobData.notes;


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
                // A CAMBIAR. DE MOMENTO SOLO SPAWNEA EN EL SPAWNER DE ARRIBA
                // DEBE HABER UNA CONDICIÓN QUE ESTABLEZCA SI HACERLO EN EL DE ARRIBA O EL DE ABAJO
                NoteSpawnerController.instance.SpawnNote(NoteSpawnerController.instance.GetSpawnerTopPosition());

                nextIndex++;
            }
            else
            {
                // A MODIFICAR
                //StartCoroutine(SceneManager.instance.GameOver());
            }
            
        }

    }

}
