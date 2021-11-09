using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame 
    public List<GameObject> layers;
    public List<float> speeds;
    private List<Vector3> initialCoords;

    void Start()
    {
        initialCoords = new List<Vector3>();
        for (int i = 0; i < layers.Count; i++)
        {
            
            Debug.Log(layers[i].GetComponent<Transform>().position);
            initialCoords.Add(layers[i].transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            layers[i].transform.Translate(-speeds[i] * Time.deltaTime, 0, 0);
        }
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].transform.position.x <= initialCoords[i].x - (layers[i].GetComponent<SpriteRenderer>().bounds.extents.x * 2))
            {
                layers[i].transform.position = initialCoords[i];
            }
        }

    }
}
