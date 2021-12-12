using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer Mixer;
    public float volMaster;
    public float volMusic;
    public float volSounds;


    // Start is called before the first frame update
    void Start()
    {
        volMaster = 1;
        volMusic = 1;
        volSounds = 1;
        setSounds();
        DontDestroyOnLoad(this);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSounds()
    {
        Mixer.SetFloat("VolMaster", Mathf.Log10(volMaster) * 20);
        Mixer.SetFloat("VolMusic", Mathf.Log10(volMusic) * 20);
        Mixer.SetFloat("VolSounds", Mathf.Log10(volSounds) * 20);
    }
    public void setMasterVolume()
    {
        Mixer.SetFloat("VolMaster", Mathf.Log10(volMaster) * 20);
    }
    public void setMusicVolume()
    {
        Mixer.SetFloat("VolMusic", Mathf.Log10(volMusic) * 20);
    }
    public void setSoundVolume()
    {
       Mixer.SetFloat("VolSounds", Mathf.Log10(volSounds) * 20);
    }
}
