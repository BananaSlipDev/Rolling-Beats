using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    [SerializeField] public const int UNITS_TO_MOVE = 3; // Unity-space units to move the notes (in other words, speed)

    //Interval for Unity-space degrees to rotate the notes (rotation speed)
    [SerializeField] private const int MIN_DEGREES_TO_ROTATE = 30;
    [SerializeField] private const int MAX_DEGREES_TO_ROTATE = 100;
    int rot;

    private Transform arrow;

    // Delegate to move the note depending on it's type
    private delegate void myDelegate();
    private myDelegate deleg;
    

    private void Start()
    {
        deleg = CheckNoteType();
        rot = Random.Range(MIN_DEGREES_TO_ROTATE, MAX_DEGREES_TO_ROTATE);
    }

    void Update()
    {
        // Moves the note
        deleg();
    }


    private myDelegate CheckNoteType()
    {
        string noteType = this.transform.tag;

        switch (noteType)
        {
            case "Note":
            default:
                return MoveNote;
            case "LongNote":
                arrow = transform.Find("Arrow").GetComponent<Transform>();
                return MoveLongNote;
            case "LongNoteEnd":
                arrow = transform.Find("Arrow").GetComponent<Transform>();
                return MoveLongNoteEnd;
        }
    }

    #region Moving
    // For now, the functions are the same. This can be expanded in the future.
    private void MoveNote()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM
        this.transform.Rotate(0f, 0f, rot * Time.deltaTime);     // Random rotation speed        
    }

    private void MoveLongNote()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM
        this.transform.Rotate(0f, 0f, rot * Time.deltaTime);     // Random rotation speed

        //Applies reverse rotation to the arrow (so it stays in place)
        arrow.Rotate(0f, 0f, -rot * Time.deltaTime);
        arrow.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
    }

    private void MoveLongNoteEnd()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM
        this.transform.Rotate(0f, 0f, rot * Time.deltaTime);     // Random rotation speed

        //Applies reverse rotation to the arrow (so it stays in place)
        arrow.Rotate(0f, 0f, -rot * Time.deltaTime);
        arrow.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
    }


    #endregion

    
}