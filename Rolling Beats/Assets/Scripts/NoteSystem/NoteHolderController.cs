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
    private GameObject noteAbove;
    private Collider2D noteAboveCol;
    private bool isNoteAbove = false;

    [SerializeField] private List<Material> beatSprites; // Must be assigned from inspector
    private ParticleSystem particles;

    // NoteHolder colliders
    private CircleCollider2D circleColl;
    private BoxCollider2D boxColl;


    void Start()
    {
        sounds = this.GetComponentInParent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        circleColl = GetComponent<CircleCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        particles = GetComponent<ParticleSystem>();
    }

    public void BeatNote()
    {
        sounds.Play();
        CheckRightorMiss();
    }

    public void ReleaseControl()
    {
        spriteRenderer.sprite = defaultSprite;
    }

    private void CheckRightorMiss()
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