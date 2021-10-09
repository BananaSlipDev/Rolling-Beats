using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHolderController : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private KeyCode keyToPress;

    public Sprite defaultSprite;
    public Sprite pressedSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        // Logic for changin NoteHolder's sprite
        if (Input.GetKeyDown(keyToPress))
            spriteRenderer.sprite = pressedSprite;

        if (Input.GetKeyUp(keyToPress))
            spriteRenderer.sprite = defaultSprite;

    }
}
