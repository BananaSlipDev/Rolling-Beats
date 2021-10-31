using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame 
    public List<GameObject> layers;
    public List<float> speeds;
    Vector3 coordIniciales;
    Vector3 coordIniciales2;
    Vector3 coordIniciales3;
    void Start()
    {
        coordIniciales = layers[3].transform.position;
        coordIniciales2 = layers[2].transform.position;
        coordIniciales3 = layers[10].transform.position;
        //layer3 = GameObject.Find("FirstLayer");
        //layer2 = GameObject.Find("2");
        //layer1 = GameObject.Find("1");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i = 0; i < layers.Count; i++)
        {
            layers[i].transform.Translate(-speeds[i] * Time.deltaTime, 0, 0);
        }
        if (layers[3].transform.localPosition.x < -49.48)
        {
            //Debug.Log(layers[3].)
            layers[3].transform.position = coordIniciales;
        }
        if (layers[2].transform.localPosition.x < -28.28)
        {
            layers[2].transform.position = coordIniciales2;
        }
        if (layers[10].transform.localPosition.x < -33.82)
        {
            layers[10].transform.position = coordIniciales3;
        }
    }
}
