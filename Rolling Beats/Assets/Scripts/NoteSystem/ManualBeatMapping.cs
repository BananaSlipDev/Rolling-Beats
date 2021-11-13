using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ManualBeatMapping : AudioSyncer
{
    [Header("Writing")]
    [SerializeField] private string songFileName; // MUST BE WRITTEN FROM THE INSPECTOR
    private string generalPath = "Assets/Audio/TextFileSongs/";
    private string songFilePath;

    public List<string> notes;

    private float songPosition;      //Current song position, in seconds

    private void Update()
    {
        songPosition = audiosource.time;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Adds a note to the list
            notes.Add("0/" + System.Math.Round(songPosition, 2));
        }
            
        if (Input.GetKeyDown(KeyCode.X))
        {   // Idem
            notes.Add("1/" + System.Math.Round(songPosition, 2));
        }
            

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Calculates song position in seconds
        songPosition = audiosource.time;
    }

    public void WriteFile()
    {
        songFilePath += generalPath + songFileName;

        if (notes.Count != 0)
        {
            //Write notes position to the .txt file
            StreamWriter writer = new StreamWriter(songFilePath, true);

            for (int i = 0; i < notes.Count; i++)
            {
                writer.WriteLine(notes[i]);
            }
            writer.Close();

            Debug.Log("Data writed succesfully.");
        }
        else
        {
            Debug.LogError("Could not write data. Please check the notes array.");
        }
    }

}
