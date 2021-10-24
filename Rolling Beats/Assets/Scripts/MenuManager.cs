using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const float INITIAL_AUDIO_VALUE = 0.3f;

    public GameObject MainMenu, SettingsMenu, SongSelectorMenu, ScoresMenu;

    [Header("MainMenu")]
    public Button StartButton;
    public Button ScoreBoardButton;
    public Button SettingsButton;

    [Header("Options")]
    public AudioMixer Mixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SoundSlider;
    public Button AcceptButton;

    [Header("SongSelection")]
    private ArrayList SongList = new ArrayList();
    public Text SongText;
    private int CurrentSong = 0;


    private void Start()
    {
        // Sets initial value for the 3 audio options
        MasterSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        MusicSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        SoundSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);

        MainMenu.SetActive(true);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);

        SongList.Add("PlayScene");
        SongList.Add("Song02");
        SongList.Add("Song03");

        SongText.text = SongList[0].ToString();
    }

    private void Awake()
    {
        MasterSlider.onValueChanged.AddListener(ChangeVolumeMaster);
        MusicSlider.onValueChanged.AddListener(ChangeVolumeMusic);
        SoundSlider.onValueChanged.AddListener(ChangeVolumeSounds);
    }

    public void OpenPanel(GameObject panel) {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);

        panel.SetActive(true);
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
    }

    public void StartSong()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SongText.text);
        //SceneManager.LoadScene(song);
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

}
