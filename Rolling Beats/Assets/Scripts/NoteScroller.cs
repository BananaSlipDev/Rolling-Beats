using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScroller : MonoBehaviour
{
    void Update()
    {
        //
        // TO BE UPDATED !!!!!
        //
        if(SceneManager.instance.musicStarted)
        {
            this.transform.position -= new Vector3(Conductor.instance.songBPM * Time.deltaTime, 0f, 0f);
        }
    }
}
