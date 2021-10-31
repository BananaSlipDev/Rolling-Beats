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
        if (layers[3].transform.localPosition.x < -45.69)
        {
            //Debug.Log(layers[3].)
            layers[3].transform.position = coordIniciales;
        }
        if (layers[2].transform.localPosition.x < -24.5)
        {
            layers[2].transform.position = coordIniciales2;
        }
        if (layers[10].transform.localPosition.x < -13.47)
        {
            layers[10].transform.position = coordIniciales3;
        }
        //if (layer3.transform.localPosition.x> -40.2f)
        //{
        //    layer3.transform.Translate(-3f*Time.deltaTime,0,0 );
        //    layer2.transform.Translate(-0.5f*Time.deltaTime,0,0 );
        //    layer1.transform.Translate(-0.1f*Time.deltaTime,0,0 );
        //}
        //else
        //{
        //    layer3.transform.position = coordIniciales;
        //}
    }
}
