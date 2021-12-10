using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteHolderController : MonoBehaviour
{
    private AudioSource sounds;
    

    // Sprites, must be assigned from the inspector
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    private SpriteRenderer spriteRenderer;

    // Note variables
    private enum Notes { Normal, Long, LongEnd };
    private Notes noteAboveType;

    // Normal Note
    private GameObject noteAbove;
    private Collider2D noteAboveCol;
    private bool isNoteAbove = false;
    // Long Note
    public bool isLongAbove = false;    

    // Particle systems
    private ParticleSystem hitParticles;
    [SerializeField] private List<Material> beatSprites; // Must be assigned from inspector
    private ParticleSystem longNoteParticles;
    

    // NoteHolder colliders
    private CircleCollider2D circleColl;
    private BoxCollider2D boxColl;

    // Scale Sprites
    private PerfectScaleSprites perfectScale;


    private void Awake() 
    {
        sounds = this.GetComponentInParent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        hitParticles = GetComponent<ParticleSystem>();
        longNoteParticles = transform.Find("LongNoteParticles").GetComponent<ParticleSystem>();

        perfectScale = GameObject.FindWithTag("Background").GetComponent<PerfectScaleSprites>();
    }

    private void Start() 
    {
        hitParticles.Stop();
        longNoteParticles.Stop();
    }

    public void BeatNote()
    {
        sounds.Play();
        spriteRenderer.sprite = pressedSprite;
        CheckRightorMiss();
    }

    public void ReleaseControl()
    {
        spriteRenderer.sprite = defaultSprite;
        longNoteParticles.Stop();

        // If the control is released while a long note is above, misses
        if (isLongAbove)
        {
            SceneManager.instance.Miss();
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[2]; //Miss
        }
            
    }

    private void CheckRightorMiss()
    {
        if (isNoteAbove) //If there's a note above the NoteHolder...
        {
            switch (noteAboveType)
            {
                case Notes.Normal:
                default:
                    HitNormalNote();
                    break;
                case Notes.Long:
                    HitLongNote();
                    break;
                case Notes.LongEnd:
                    // Nothing for the moment
                    break;
            }

            
        }
        else
        {
            SceneManager.instance.Miss(); //Fails if beaten without a note
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[2]; //Miss
        }

        hitParticles.Emit(1); // Emits whatever particle has to

    }

    #region Notes hit functions
    private void HitNormalNote()
    {
        if (circleColl.IsTouching(noteAboveCol))        // Perfect collider
        {
            SceneManager.instance.ScoreNote("PERFECT");
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[0];//Perfect
            perfectScale.OnBeat();//Move Sprites
        }
        else if (boxColl.IsTouching(noteAboveCol))      // Great collider
        {
            SceneManager.instance.ScoreNote("GREAT");
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[1]; //Great
        }

        Destroy(noteAbove);
    }

    private void HitLongNote()
    {
        isLongAbove = true;
        longNoteParticles.Play();
        if (circleColl != null && circleColl.IsTouching(noteAboveCol))        // Perfect collider
        {
            SceneManager.instance.ScoreNote("PERFECT");
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[0];//Perfect
            perfectScale.OnBeat(); //Move Sprites
        }
        else if (boxColl.IsTouching(noteAboveCol))      // Great collider
        {
            SceneManager.instance.ScoreNote("GREAT");
            hitParticles.GetComponent<ParticleSystemRenderer>().material = beatSprites[1]; //Great
        }

        Destroy(noteAbove);
    }

    #endregion

    // The OnTrigger functions detect if the note is above or not
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Note" || other.tag == "LongNote" || other.tag == "LongNoteEnd")
        {
            isNoteAbove = true;
            noteAbove = other.gameObject;

            switch(other.tag) // Asigns a type
            {
                case "Note":
                default:
                    noteAboveType = Notes.Normal;
                    break;
                case "LongNote":
                    noteAboveType = Notes.Long;
                    break;
                case "LongNoteEnd":
                    noteAboveType = Notes.LongEnd;
                    isLongAbove = false; // When the LE-note enters (and without beating), the long ends.
                    Destroy(other.gameObject); // However, destroy the Long end
                    longNoteParticles.Stop(); // Stops the particle system
                    break;
            }

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