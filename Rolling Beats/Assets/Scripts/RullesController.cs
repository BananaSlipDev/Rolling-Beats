using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RullesController : MonoBehaviour
{
    [SerializeField] private List<Sprite> rullesSprites; // Must be assigned from inspector

    private SpriteRenderer rulles;
    private GameObject noteHolderTop;
    private GameObject noteHolderBottom;

    private void Start()
    {
        rulles = this.transform.Find("Rulles").gameObject.GetComponent<SpriteRenderer>();
        noteHolderTop = this.transform.Find("NoteHolderTop").gameObject;
        noteHolderBottom = this.transform.Find("NoteHolderBottom").gameObject;
    }


    public void IdleSprite()
    {
        rulles.sprite = rullesSprites[0];
        rulles.transform.position = new Vector3(noteHolderBottom.transform.position.x - 2, noteHolderBottom.transform.position.y + 1, noteHolderBottom.transform.position.z);
    }

    public void DownSprite()
    {
        rulles.sprite = rullesSprites[1];
        rulles.transform.position = new Vector3(noteHolderBottom.transform.position.x - 2, noteHolderBottom.transform.position.y + 1, noteHolderBottom.transform.position.z);
    }

    public void JumpSprite()
    {
        rulles.sprite = rullesSprites[2];
        rulles.transform.position = new Vector3(noteHolderTop.transform.position.x - 2, noteHolderBottom.transform.position.y + 3, noteHolderBottom.transform.position.z);
    }
}
