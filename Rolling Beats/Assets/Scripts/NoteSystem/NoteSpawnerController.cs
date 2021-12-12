using UnityEngine;
using System.Collections.Generic;

public class NoteSpawnerController : MonoBehaviour
{
    public GameObject notePrefab; // Must be asigned from the PREFABS FOLDER in the Unity editor
    public GameObject longNotePrefab;
    public GameObject longNoteEndPrefab;

    public static NoteSpawnerController instance;

    // Spawner's gameobjects. For now only uses the position.
    private GameObject spawnerTop;
    private GameObject spawnerBottom;

    [SerializeField] private List<Sprite> normalNoteSprites;
    [SerializeField] private List<Sprite> longNoteSprites;
    [SerializeField] private List<Sprite> longNoteEndSprites;
    [SerializeField] private List<Sprite> longNoteArrowSprites;

    
    

    /* The following variable is used to match long notes with their end.
        For it to work, sprites of longNotes and LongNoteEnds must be
        added with the exact same color order.
    */
    private int longNoteEndSpriteMemory = 0;
    private int bottomLongNoteEndSpriteMemory = 0;


    private void Start()
    {
        instance = this;

        spawnerTop = this.transform.GetChild(0).gameObject;
        spawnerBottom = this.transform.GetChild(1).gameObject;
    }

    //Spawns a note above the spawner
    public void SpawnNote(Vector3 p)
    {
        // Possible sprites must be assigned from inspector!!
        int randomSpriteIdx = (int)Random.Range(0, normalNoteSprites.Count);
        notePrefab.GetComponent<SpriteRenderer>().sprite = normalNoteSprites[randomSpriteIdx];

        Object.Instantiate(notePrefab, p, Quaternion.identity);
    }

    public void SpawnLongNote(Vector3 p)
    {
        // Possible sprites must be assigned from inspector!!
        int randomSpriteIdx = (int)Random.Range(0, longNoteSprites.Count);
        longNotePrefab.GetComponent<SpriteRenderer>().sprite = longNoteSprites[randomSpriteIdx];
        longNotePrefab.transform.Find("Arrow").GetComponent<SpriteRenderer>().sprite = longNoteArrowSprites[randomSpriteIdx];

        
        longNoteEndSpriteMemory = randomSpriteIdx;

        Object.Instantiate(longNotePrefab, p, Quaternion.identity);
    }

    public void SpawnLongNoteEnd(Vector3 p)
    {
        // Possible sprites must be assigned from inspector!!

        longNoteEndPrefab.GetComponent<SpriteRenderer>().sprite = longNoteEndSprites[longNoteEndSpriteMemory];
        longNoteEndPrefab.transform.Find("Arrow").GetComponent<SpriteRenderer>().sprite = longNoteArrowSprites[longNoteEndSpriteMemory];
        
        Object.Instantiate(longNoteEndPrefab, p, Quaternion.identity);
    }


    public Vector3 GetSpawnerTopPosition()
    {
        return spawnerTop.transform.position;
    }
    public Vector3 GetSpawnerBottomPosition()
    {
        return spawnerBottom.transform.position;
    }
}