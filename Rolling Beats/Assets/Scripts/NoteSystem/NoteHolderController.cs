using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteHolderController : MonoBehaviour
{
    // -- ONLY FOR TESTING --
    public bool testMode = false;
    //-----------------------

    private RullesController rulles;
    private AudioSource sounds;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    private SpriteRenderer spriteRenderer;

    // Note variables
    private GameObject noteAbove;
    private Collider2D noteAboveCol;
    private bool isNoteAbove = false;

    [SerializeField] private List<Material> beatSprites; // Must be assigned from inspector
    private ParticleSystem particles;

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
        rulles = this.GetComponentInParent<RullesController>();

        sounds = this.GetComponentInParent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        particles = GetComponent<ParticleSystem>();
    }
    
    void Update()
    {
        if(!testMode)
        {
            // Controls
            if (CheckMobileManager.SharedInstance.IsMobileGet)
            {
                if (!GameUI.instance.PauseMenu.activeInHierarchy)
                {
                    UpdateinMobile();
                }

            }
            
        }
        else
        {
            if (Input.GetKeyDown(keyToPress))
            {
                switch (keyToPress)
                {
                    case KeyCode.Z:
                        rulles.JumpSprite();
                        break;
                    case KeyCode.X:
                        rulles.DownSprite();
                        break;
                }

                sounds.Play();
                checkRightorMiss();

            }

            if (Input.GetKeyUp(keyToPress)) //Resets the sprite to default
            {
                spriteRenderer.sprite = defaultSprite;
                rulles.IdleSprite();
            }
                
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

            
            if (multiplier* mytouch.position.x < multiplier* Screen.width/2f && mytouch.position.y < (Screen.height - Screen.height/3))
            {
                checkRightorMiss();

                switch(multiplier) //Changes the sprite depending on the touch
                {
                    case 1:
                        rulles.JumpSprite();
                        break;
                    case -1:
                        rulles.DownSprite();
                        break;
                }
            }
        }

        wasTouching = isTouching;

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            spriteRenderer.sprite = defaultSprite;
            rulles.IdleSprite();
        }
    }

    private void checkRightorMiss()
    {
        spriteRenderer.sprite = pressedSprite; //Changes the sprite and scores the note if needed

        if (isNoteAbove) //If there's a note above the NoteHolder...
        {
            if (circleColl.IsTouching(noteAboveCol))        // Perfect collider
            {
                SceneManager.instance.ScoreNote("PERFECT");
                particles.GetComponent<ParticleSystemRenderer>().material = beatSprites[0];//Perfect
            }                
            else if (boxColl.IsTouching(noteAboveCol))      // Great collider
            {
                SceneManager.instance.ScoreNote("GREAT");
                particles.GetComponent<ParticleSystemRenderer>().material = beatSprites[1]; //Great

            }
            Destroy(noteAbove);
        }
        else
        {
            SceneManager.instance.Miss(); //Fails if beaten without a note
            particles.GetComponent<ParticleSystemRenderer>().material = beatSprites[2]; //Miss
        }

        particles.Emit(1);

    }

}