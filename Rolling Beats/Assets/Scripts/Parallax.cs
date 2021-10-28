using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject layer3, layer2, layer1, layer0;
    void Start()
    {
        layer3 = GameObject.Find("FirstLayer");
        layer2 = GameObject.Find("2");
        layer1 = GameObject.Find("1");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (layer3.transform.localPosition.x> -19.1f)
        {
            layer3.transform.Translate(-3f*Time.deltaTime,0,0 );
            layer2.transform.Translate(-0.5f*Time.deltaTime,0,0 );
            layer1.transform.Translate(-0.1f*Time.deltaTime,0,0 );
        }
        else
        {
            layer3.transform.localPosition = new Vector3(0, 0, 0);
        }
        
    }
}
