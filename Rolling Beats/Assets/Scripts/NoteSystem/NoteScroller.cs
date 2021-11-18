using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    [SerializeField] public const int UNITS_TO_MOVE = 2; // Unity-space units to move the notes (in other words, speed)

    //Interval for Unity-space degrees to rotate the notes (rotation speed)
    [SerializeField] private const int MIN_DEGREES_TO_ROTATE = 10;
    [SerializeField] private const int MAX_DEGREES_TO_ROTATE = 60;

    // Delegate to move the note depending on it's type
    private delegate void myDelegate();
    private myDelegate deleg;

    // Variables for the LongNote:
    private GameObject longEnd;
    private bool parentSwitched = false;

    private SpriteRenderer fillSpriteRenderer;

    private void Start()
    {
        deleg = CheckNoteType();
    }

    void Update()
    {
        // Moves the note
        deleg();
    }


    private myDelegate CheckNoteType()
    {
        string noteType = this.transform.tag;

        switch (noteType) // Asigns a type
        {
            case "Note":
            default:
                return MoveNote;
            case "LongNote":
                fillSpriteRenderer = transform.Find("Fill").GetComponent<SpriteRenderer>();
                return MoveLongNote;
            case "LongNoteEnd":
                return MoveLongNoteEnd;
        }
    }

    #region Moving
    private void MoveNote()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM
        this.transform.Rotate(0f, 0f, Random.Range(MIN_DEGREES_TO_ROTATE, MAX_DEGREES_TO_ROTATE) * Time.deltaTime);     // Random rotation speed
    }

    private void MoveLongNoteEnd()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM

        if(transform.Find("Fill"))
        {
            // Finds the fill and sets the position to the parent's
            Transform fill = this.transform.Find("Fill").transform;
            fillSpriteRenderer = fill.GetComponent<SpriteRenderer>();
            fill.transform.position = fill.transform.parent.position;

            // Rotates the star
            int rot = Random.Range(MIN_DEGREES_TO_ROTATE, MAX_DEGREES_TO_ROTATE);
            this.transform.Rotate(0f, 0f, rot * Time.deltaTime);     // Random rotation speed
            fill.Rotate(0f, 0f, -rot * Time.deltaTime);

            // If the start has already been destroyed, start decreasing the fill
            if(!SearchForStart() && fillSpriteRenderer.size.x < 0)
                fillSpriteRenderer.size += new Vector2(Conductor.instance.beatPerSec * NoteScroller.UNITS_TO_MOVE * Time.deltaTime, 0f);
        }
        
    }

    private void MoveLongNote()
    {
        // Moves the notes UNITS_TO_MOVE per beat of the song
        this.transform.position -= new Vector3(Conductor.instance.beatPerSec * UNITS_TO_MOVE * Time.deltaTime, 0f, 0f); // Fixed speed depending on BPM

        int rot = Random.Range(MIN_DEGREES_TO_ROTATE, MAX_DEGREES_TO_ROTATE);
        this.transform.Rotate(0f, 0f, rot * Time.deltaTime);     // Random rotation speed
        
        if(transform.Find("Fill"))
        {
            Transform fill = this.transform.Find("Fill").transform;
            fill.Rotate(0f, 0f, -rot * Time.deltaTime);
        }

        // While the end hasn't spawned, fill
        if(!SearchForEnd())
            fillSpriteRenderer.size += new Vector2(Conductor.instance.beatPerSec * NoteScroller.UNITS_TO_MOVE * Time.deltaTime, 0f);
        
        else if(!parentSwitched) // If the end is found, switch the parents
            ChangeFillParent();
 
            
        
    }

#endregion

    #region Searching
    private bool SearchForEnd() // Searchs for the end of a long note
    {  
        longEnd = GameObject.Find("LongNoteEnd(Clone)");

        if(longEnd) // Si existe el LongNoteEnd
            if(longEnd.transform.position.y == this.transform.position.y &&
                longEnd.transform.position.x > this.transform.position.x) // Si está en el mismo carril y detrás de la nota actual (busca su pareja)
                return true;

        return false;
    }

    private bool SearchForStart() // Searchs for the start of a long note
    {  
        GameObject longStart = GameObject.Find("LongNote(Clone)");

        if(longStart) // Si existe el LongNoteEnd
            if(longStart.transform.position.y == this.transform.position.y &&
                longStart.transform.position.x < this.transform.position.x) // Si está en el mismo carril y delante de la nota actual (busca su pareja)
                return true;

        return false;
    }

    private void ChangeFillParent() // Switch the fill from the start to the end
    {
        fillSpriteRenderer.gameObject.transform.parent = longEnd.transform; // Changes the parent
        fillSpriteRenderer.size *= new Vector2(-1, 1);
        parentSwitched = true;
    }

    #endregion
    
}