using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const float INITIAL_AUDIO_VALUE = 1f;

    public GameObject MainMenu, SettingsMenu, SongSelectorMenu, ScoresMenu, CreditsMenu, ShopMenu, LoadScreen;

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
    public TextMeshProUGUI yourCoins;

    public Button getLead;

    public GameObject notAvailable;

    public GameObject shopPanel;
    public GameObject panelBuy;

    public String songToBuy;
    public GameObject songPrefab;


    private void Start()
    {
        
        StartCoroutine(LoadScreenCR());
        // Sets initial value for the 3 audio options
       //MasterSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
       //MusicSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
       //SoundSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);

       //welcomeT.text = "Welcome " + PlayFabManager.SharedInstance.finalName ;
       //MainMenu.SetActive(true);
       //SettingsMenu.SetActive(false);
       //SongSelectorMenu.SetActive(false);
       //ScoresMenu.SetActive(false);
       //CreditsMenu.SetActive(false);

       //SongText.text = SongList[0].ToString();
       //
       //PlayFabManager.SharedInstance.ActualLevel = SongText.text;
       //PlayFabManager.SharedInstance.getScoreAndLevel();
       //PlayFabManager.SharedInstance.GetInventory();
       //PlayFabManager.SharedInstance.getCurrency();
       //PlayFabManager.SharedInstance.getShop();
        
        
        
        
        

        //yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
    }

    private void Awake()
    {
        //shopPanel = GameObject.Find("Panel");
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
        ShopMenu.SetActive(false);

        panel.SetActive(true);
        yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
        yourCoins.text = ": " +PlayFabManager.SharedInstance.RollingCoins;

        
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

        if (!PlayFabManager.SharedInstance.songs.Contains(SongText.text))
        {
            notAvailable.SetActive(true);
        }
        else
        {
            notAvailable.SetActive(false);
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
        
        if (!PlayFabManager.SharedInstance.songs.Contains(SongText.text))
        {
            notAvailable.SetActive(true);
        }
        else
        {
            notAvailable.SetActive(false);
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

    #region Shop

    
    public void makePurchase()
    {
        panelBuy.SetActive(true);
        songToBuy = EventSystem.current.currentSelectedGameObject.name;;
    }

    public void confirmPurchase()
    {
        PlayFabManager.SharedInstance.makePurchase(songToBuy);
        StartCoroutine(OneSecond());
        panelBuy.SetActive(false);
        
    }

    public void cancelPurchase()
    {
        panelBuy.SetActive(false);
        
    }
    
    void checkPurchasedSongs()
    {
        foreach (Transform gameObj in shopPanel.transform)
        {

            if (PlayFabManager.SharedInstance.songs.Contains(gameObj.name))
            {
                gameObj.GetComponent<Button>().enabled = false;
                gameObj.transform.Find("Panel").gameObject.SetActive(true);
               
            }
            
        }
    }

    void checkItemsToAddShop()
    {
        foreach (Transform gameObj in shopPanel.transform)
        {
            if (PlayFabManager.SharedInstance.itemsAvailable.ContainsKey(gameObj.name))
            {
                gameObj.transform.Find("SongPrice").GetComponent<TextMeshProUGUI>().text =
                    PlayFabManager.SharedInstance.itemsAvailable[gameObj.name].ToString();
            }
            
        }
    }

    #endregion

    public IEnumerator MedioSecond()
    {
        yield return new WaitForSeconds(0.5f);
        yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
    }

    public IEnumerator OneSecond()
    {
        yield return new WaitForSeconds(1f);
        yourCoins.text = ": " +PlayFabManager.SharedInstance.RollingCoins;
        checkPurchasedSongs();
        if (!PlayFabManager.SharedInstance.songs.Contains(SongText.text))
        {
            notAvailable.SetActive(true);
        }
        else
        {
            notAvailable.SetActive(false);
        }
    }

    public IEnumerator LoadScreenCR()
    {
        MasterSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        MusicSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);
        SoundSlider.SetValueWithoutNotify(INITIAL_AUDIO_VALUE);

        welcomeT.text = "Welcome " + PlayFabManager.SharedInstance.finalName ;
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        LoadScreen.SetActive(true);

        SongText.text = SongList[0].ToString();
        
        PlayFabManager.SharedInstance.ActualLevel = SongText.text;
        PlayFabManager.SharedInstance.getScoreAndLevel();
        PlayFabManager.SharedInstance.GetInventory();
        PlayFabManager.SharedInstance.getCurrency();
        PlayFabManager.SharedInstance.getShop();
        StartCoroutine(OneSecond());
        
        //fillShop();
        yield return new WaitForSeconds(3f);
        fillShop();
        yield return new WaitForSeconds(2f);
        checkItemsToAddShop();
        MainMenu.SetActive(true);
        LoadScreen.SetActive(false);
    }

    public void fillShop()
    {
        foreach (Transform item in shopPanel.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in PlayFabManager.SharedInstance.itemsAvailable)
        {
            GameObject newGO = Instantiate(songPrefab, shopPanel.transform);
            newGO.GetComponent<Button>().onClick.AddListener(makePurchase);
            newGO.name = item.Key;
            newGO.transform.Find("SongName").GetComponent<TextMeshProUGUI>().text = item.Key;
            newGO.transform.Find("SongPrice").GetComponent<TextMeshProUGUI>().text = item.Value.ToString();


        }
    }

    

}
