using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private KeyCode keyToPress;

    private Touch mytouch;
    

    [SerializeField]
    private int multiplier;

    private bool wasTouching = false;
    
    
    //Must be asigned from the Unity editor
    

    void Start()
    {
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
    }
    
    void Update()
    {
        // Controls
        if (CheckMobileManager.SharedInstance.IsMobileGet)
        {
            UpdateinMobile();
        }
        else
        {
            if (Input.GetKeyDown(keyToPress))
            { 
                checkRightorMiss();
            }
            
            if (Input.GetKeyUp(keyToPress)) //Resets the sprite to default
                spriteRenderer.sprite = defaultSprite;
        }
        
        
       

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

    private void UpdateinMobile()
    {
        bool isTouching = Input.touchCount > 0;
        if (isTouching && Input.touches[0].phase == TouchPhase.Began && !wasTouching)
        {
            Input.touches[0].phase = TouchPhase.Canceled;
            
            mytouch = Input.GetTouch(0);
            if (multiplier* mytouch.position.x < multiplier* Screen.width/2f)
            {
                checkRightorMiss();
            }
        }

        wasTouching = isTouching;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private void checkRightorMiss()
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
    

}
