using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // Checks if the note is missed. If so, destroy it and call SceneManager
        if (collision.CompareTag("Note") ||
            collision.CompareTag("LongNote") ||
            collision.CompareTag("LongNoteEnd"))
        {
            SceneManager.instance.Miss();
            Destroy(collision.gameObject);
        }  
    }
}
