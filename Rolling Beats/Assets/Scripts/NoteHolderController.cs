using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHolderController : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite pressedSprite;


    private SpriteRenderer spriteRenderer;

    // Note variables
    private GameObject noteAbove;
    private Collider2D noteAboveCol;
    private bool isNoteAbove = false;

    // NoteHolder colliders
    private CircleCollider2D circleColl;
    private BoxCollider2D boxColl;

    [SerializeField]
    private KeyCode keyToPress; //Must be asigned from the Unity editor
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        // Controls
        if (Input.GetKeyDown(keyToPress))
        { 
            spriteRenderer.sprite = pressedSprite; //Changes the sprite and scores the note if needed

            if (isNoteAbove) //If there's a note above the NoteHolder...
            {
                if (circleColl.IsTouching(noteAboveCol))        // Perfect collider
                    SceneManager.instance.ScoreNote("PERFECT");
                else if (boxColl.IsTouching(noteAboveCol))      // Great collider
                    SceneManager.instance.ScoreNote("GREAT");

                Destroy(noteAbove);
            }
            else
                SceneManager.instance.Miss(); //Fails if beaten without a note
        }
            
        if (Input.GetKeyUp(keyToPress)) //Resets the sprite to default
            spriteRenderer.sprite = defaultSprite;

    }

    // The OnTrigger functions detect if the note is above or not
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Note" )
        {
            isNoteAbove = true;
            noteAbove = other.gameObject;
            noteAboveCol = other;
        }
            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Note")
        {
            isNoteAbove = false;
            noteAbove = null;
            noteAboveCol = null;
        }
            
    }

}
