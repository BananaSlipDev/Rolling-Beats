using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const float INITIAL_AUDIO_VALUE = 0.3f;

    public GameObject MainMenu, SettingsMenu, SongSelectorMenu, ScoresMenu, CreditsMenu;

    [Header("Options")]
    public AudioMixer Mixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SoundSlider;

    [Header("SongSelection")]
    [SerializeField] private List<string> SongList;
    public Text SongText;
    private int CurrentSong = 0;
    
    public TextMeshProUGUI welcomeT;
    public TextMeshProUGUI yourScore;

    public Button getLead;


    private void Start()
    {
        // Sets initial value for the 3 audio options
        MasterSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        MusicSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        SoundSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);

        welcomeT.text = "Welcome " + PlayFabManager.SharedInstance.finalName ;
        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        SongText.text = SongList[0].ToString();
        
        PlayFabManager.SharedInstance.ActualLevel = SongText.text;
        PlayFabManager.SharedInstance.getScoreAndLevel();
        //yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
    }

    private void Awake()
    {
       
        PlayFabManager.SharedInstance.rowsParent = GameObject.Find("TAble");
        MasterSlider.onValueChanged.AddListener(ChangeVolumeMaster);
        MusicSlider.onValueChanged.AddListener(ChangeVolumeMusic);
        SoundSlider.onValueChanged.AddListener(ChangeVolumeSounds);
    }

    private void Update()
    {
        
    }

    public void OpenPanel(GameObject panel) {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);
        CreditsMenu.SetActive(false);

        panel.SetActive(true);
        yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
    }

    public void getCurrentLeaderboard()
    {
        PlayFabManager.SharedInstance.GetLeaderboard(SongText.text);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(true);
        
    }

    public void closeScoreBoard()
    {
        ScoresMenu.SetActive(false);
        SongSelectorMenu.SetActive(true);
        
    }

    #region SongSelector
    public void NextSong() {
        if (CurrentSong + 1 > SongList.Count-1)
        {
            SongText.text = SongList[0].ToString();
            CurrentSong = 0;
            
            

        }
        else {
            SongText.text = SongList[CurrentSong + 1].ToString();
            CurrentSong += 1;

        }
        
        PlayFabManager.SharedInstance.ActualLevel = SongText.text;
        PlayFabManager.SharedInstance.getScoreAndLevel();
        StartCoroutine(MedioSecond());
        
        
    }

    public void PreviousSong() {
        if (CurrentSong - 1 < 0)
        {
            SongText.text = SongList[SongList.Count - 1].ToString();
            CurrentSong = SongList.Count - 1;

        }
        else {
            SongText.text = SongList[CurrentSong - 1].ToString();
            CurrentSong -= 1;
            
            
        }
        PlayFabManager.SharedInstance.ActualLevel = SongText.text;
        PlayFabManager.SharedInstance.getScoreAndLevel();
        StartCoroutine(MedioSecond());
        
    }

    public void StartSong()
    {
        if(!SongText.text.Equals("Coming soon!"))
            UnityEngine.SceneManagement.SceneManager.LoadScene(SongText.text);
    }
    #endregion

    #region Settings
    // Log10 * 20 sets the volume right (sound is not linear)
    public void ChangeVolumeMaster(float v) {
        Mixer.SetFloat("VolMaster", Mathf.Log10(v) * 20);
    }
    public void ChangeVolumeMusic(float v)
    {
        Mixer.SetFloat("VolMusic", Mathf.Log10(v) * 20);
    }
    public void ChangeVolumeSounds(float v)
    {
        Mixer.SetFloat("VolSounds", Mathf.Log10(v) * 20);
    }
    #endregion

    #region Credits

    public void GoToUrl(string url) {
        Application.OpenURL(url);
    }

    #endregion

    public IEnumerator MedioSecond()
    {
        yield return new WaitForSeconds(0.5f);
        yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
    }

}
