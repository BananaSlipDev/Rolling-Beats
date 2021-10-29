using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;


public class AudioSyncScale : AudioSyncer {


	[Header("Scaling")]
	public Vector3 beatScale;
	public Vector3 restScale;

	// Text-writing variables
	[Header("Writing")]
	[SerializeField] private string songFileName; // MUST BE WRITTEN FROM THE INSPECTOR
	private string generalPath = "Assets/Audio/TextFileSongs/";
	private string songFilePath;

	[Header("Notes List")]
	public List<double> notes;

	//Seconds passed since song started
	private float dspSongTime;
	private float songPosition;      //Current song position, in seconds

	

	private void Start()
    {
		//Record the time when music starts
		dspSongTime = (float)AudioSettings.dspTime;

		// Declares list of notes to write
		notes = new List<double>();

		songFilePath += generalPath + songFileName;
    }

	public override void OnBeat()
	{
		base.OnBeat();

		StopCoroutine("MoveToScale");
		StartCoroutine("MoveToScale", beatScale);

		StopCoroutine("addNote");
		StartCoroutine("addNote", songPosition);
	}

	private IEnumerator addNote(double songPos)
    {
		notes.Add(System.Math.Round(songPos, 2)); // Writes the song position por each note (rounded to 2 decimals)
		yield return null;
    }

	// Writing function accesed from the button in the song pre-processing scene
	public void WriteFile()
    {
        if (notes.Count != 0)
        {
			//Write notes position to the .txt file
			StreamWriter writer = new StreamWriter(songFilePath, true);

			for (int i = 0; i < notes.Count; i++)
			{
				writer.WriteLine(notes[i]);
			}
			writer.Close();

			//Re-import the file to update the reference in the editor
			AssetDatabase.ImportAsset(songFilePath);

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

	public override void OnUpdate()
	{
		base.OnUpdate();

		// Calculates song position in seconds
		songPosition = (float)(AudioSettings.dspTime - dspSongTime);

		if (m_isBeat) return;

		transform.localScale = Vector3.Lerp(transform.localScale, restScale, restSmoothTime * Time.deltaTime);

	}

	

}
