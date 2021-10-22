using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawnerController : MonoBehaviour
{
    public GameObject notePrefab; // Must be asigned from the PREFABS FOLDER in the Unity editor
    public static NoteSpawnerController instance;

    private void Start()
    {
        instance = this;
    }


    //Spawns a note above the spawner
    public void SpawnNote()
    {

        Object.Instantiate(notePrefab, this.transform);
    }

}
