using UnityEngine;

public class NoteSpawnerController : MonoBehaviour
{
    public GameObject notePrefab; // Must be asigned from the PREFABS FOLDER in the Unity editor
    public static NoteSpawnerController instance;

    // Spawner's gameobjects. For now only uses the position.
    private GameObject spawnerTop;
    private GameObject spawnerBottom;

    private void Start()
    {
        instance = this;

        spawnerTop = this.transform.GetChild(0).gameObject;
        spawnerBottom = this.transform.GetChild(1).gameObject;
    }

    //Spawns a note above the spawner
    public void SpawnNote(Vector3 p)
    {
        Object.Instantiate(notePrefab, p, Quaternion.identity);
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