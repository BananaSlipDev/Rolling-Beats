using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ManualBeatMapping : AudioSyncer
{
    [Header("Scaling")]
    public Vector3 beatScale;
    public Vector3 restScale;


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

            // Scales the square (just for visuals)
            StopCoroutine("MoveToScale");
            StartCoroutine("MoveToScale", beatScale);
        }
            
        if (Input.GetKeyDown(KeyCode.X))
        {   // Idem
            notes.Add("1/" + System.Math.Round(songPosition, 2));
            StopCoroutine("MoveToScale");
            StartCoroutine("MoveToScale", beatScale);
        }
            

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // Calculates song position in seconds
        songPosition = audiosource.time;

        transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);
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

    private IEnumerator MoveToScale(Vector3 _target)
    {
        Vector3 _curr = transform.localScale;
        Vector3 _initial = _curr;
        float _timer = 0;

        while (_curr != _target)
        {
            _curr = Vector3.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            transform.localScale = _curr;

            yield return null;
        }

        m_isBeat = false;
    }
}
