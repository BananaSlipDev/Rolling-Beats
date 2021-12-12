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
    

    public GameObject MainMenu, SettingsMenu, SongSelectorMenu, ScoresMenu, CreditsMenu, ShopMenu, LoadScreen;

    public GameObject easy, intermediate, hard, inferno;

    [Header("Options")]
    public SoundManager audioMix;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SoundSlider;

    [Header("SongSelection")]
    [SerializeField] private List<string> SongList;

    [SerializeField] private List<string> songList2;
    [SerializeField] private List<int> songsDificult;
    public Text SongText;
    private int CurrentSong = 0;
    
    public TextMeshProUGUI welcomeT;
    public TextMeshProUGUI yourScore;
    public TextMeshProUGUI yourCoins;
    public TextMeshProUGUI songInfo;

    public Button getLead;

    public GameObject notAvailable;

    public GameObject shopPanel;
    public GameObject panelBuy;

    public String songToBuy;
    public GameObject songPrefab, buyRc1, buyRc2, buyRc3, buyRc4, buySkin;

    private void Awake()
    {
        audioMix = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        //shopPanel = GameObject.Find("Panel");
        PlayFabManager.SharedInstance.rowsParent = GameObject.Find("TAble");
        MasterSlider.value = audioMix.volMaster;
        MusicSlider.value = audioMix.volMusic;
        SoundSlider.value = audioMix.volSounds;
        MasterSlider.onValueChanged.AddListener(ChangeVolumeMaster);
        MusicSlider.onValueChanged.AddListener(ChangeVolumeMusic);
        SoundSlider.onValueChanged.AddListener(ChangeVolumeSounds);
        


        
        ChangeVolumeMaster(audioMix.volMaster);
        ChangeVolumeMusic(audioMix.volMusic);
        ChangeVolumeSounds(audioMix.volSounds);

        
    }
    private void Start()
    {
        StartCoroutine(LoadScreenCR());
        audioMix.setSounds();
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
        yourCoins.text = PlayFabManager.SharedInstance.RollingCoins.ToString();
    }

    public void getCurrentLeaderboard()
    {
        PlayFabManager.SharedInstance.GetLeaderboard(SongText.text);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(true);
        
    }

    public void getCurrentLeaderboardAroundPlayer()
    {
        PlayFabManager.SharedInstance.GetLeaderboardAroundPlayer(SongText.text);
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
            songInfo.text = "Credits to: \n"+songList2[0].ToString();
            checkSongDificult(songsDificult[0]);
            CurrentSong = 0;

        }
        else {
            SongText.text = SongList[CurrentSong + 1].ToString();
            songInfo.text = "Credits to: \n"+songList2[CurrentSong + 1].ToString();
            checkSongDificult(songsDificult[CurrentSong+1]);
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
            songInfo.text = "Credits to: \n"+ songList2[songList2.Count - 1].ToString();
            checkSongDificult(songsDificult[songsDificult.Count-1]);
            CurrentSong = SongList.Count - 1;

        }
        else {
            SongText.text = SongList[CurrentSong - 1].ToString();
            songInfo.text = "Credits to: \n"+songList2[CurrentSong - 1].ToString();
            checkSongDificult(songsDificult[CurrentSong-1]);
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
    public void ChangeVolumeMaster(float v)
    {
        audioMix.volMaster = v;
        audioMix.setMasterVolume();
    }
    public void ChangeVolumeMusic(float v)
    {
        audioMix.volMusic = v;
        audioMix.setMusicVolume();
    }
    public void ChangeVolumeSounds(float v)
    {
        audioMix.volSounds = v;
        audioMix.setSoundVolume();
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
        panelBuy.transform.Find("NotMoney").gameObject.SetActive(false);
        panelBuy.transform.Find("Check").gameObject.SetActive(true);
        
        songToBuy = EventSystem.current.currentSelectedGameObject.name;;
    }

    public void confirmPurchase()
    {
        if (PlayFabManager.SharedInstance.RollingCoins >= PlayFabManager.SharedInstance.itemsAvailable[songToBuy])
        {
            PlayFabManager.SharedInstance.makePurchase(songToBuy);
            StartCoroutine(waitforBool());
        }
        else
        {
            panelBuy.transform.Find("NotMoney").gameObject.SetActive(true);
            panelBuy.transform.Find("Check").gameObject.SetActive(false);
        }
        
        
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
        panelBuy.transform.Find("Check").gameObject.SetActive(true);
        panelBuy.transform.Find("Loading").gameObject.SetActive(false);
        panelBuy.SetActive(false);
    }

    void checkItemsToAddShop()
    {
        foreach (Transform gameObj in shopPanel.transform)
        {
            if (PlayFabManager.SharedInstance.itemsAvailable.ContainsKey(gameObj.name) && gameObj.transform.Find("SongPrice"))
            {
                gameObj.transform.Find("SongPrice").GetComponent<TextMeshProUGUI>().text =
                    PlayFabManager.SharedInstance.itemsAvailable[gameObj.name].ToString();
            }
            
        }
    }
    
    public void fillShop()
    {
        foreach (Transform item in shopPanel.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in PlayFabManager.SharedInstance.itemsAvailable)
        {
            if (item.Value > 50)
            {
                GameObject newGO = Instantiate(songPrefab, shopPanel.transform);
                newGO.GetComponent<Button>().onClick.AddListener(makePurchase);
                newGO.name = item.Key;
                newGO.transform.Find("SongName").GetComponent<TextMeshProUGUI>().text = item.Key;
                newGO.transform.Find("SongPrice").GetComponent<TextMeshProUGUI>().text = item.Value.ToString();
            }
            else if(item.Value==0)
            {
                GameObject newGO = Instantiate(buyRc1, shopPanel.transform);
                newGO.name = item.Key;
            }
            else if(item.Value==1)
            {
                GameObject newGO = Instantiate(buyRc2, shopPanel.transform);
                newGO.name = item.Key;
            }
            else if(item.Value==2)
            {
                GameObject newGO = Instantiate(buyRc3, shopPanel.transform);
                newGO.name = item.Key;
            }
            else if(item.Value==3)
            {
                GameObject newGO = Instantiate(buyRc4, shopPanel.transform);
                newGO.name = item.Key;
            }
            else if(item.Value==4)
            {
                GameObject newGO = Instantiate(buySkin, shopPanel.transform);
                newGO.name = item.Key;
            }
            


        }
    }

    #endregion

    private void checkSongDificult(int level)
    {
        switch (level)
        {
            case 0:
                easy.SetActive(true);
                intermediate.SetActive(false);
                hard.SetActive(false);
                inferno.SetActive(false);
                break;
            case 1:
                easy.SetActive(false);
                intermediate.SetActive(true);
                hard.SetActive(false);
                inferno.SetActive(false);
                
                break;
            case 2:
                easy.SetActive(false);
                intermediate.SetActive(false);
                hard.SetActive(true);
                inferno.SetActive(false);
                
                break;
            case 3:
                easy.SetActive(false);
                intermediate.SetActive(false);
                hard.SetActive(false);
                inferno.SetActive(true);
                
                break;
        }
    }

    public IEnumerator MedioSecond()
    {
        yield return new WaitForSeconds(0.8f);
        yourScore.text = PlayFabManager.SharedInstance.actualLevelScore.ToString();
        
    }

    public IEnumerator OneSecond()
    {
        yield return new WaitForSeconds(1f);
        yourCoins.text = PlayFabManager.SharedInstance.RollingCoins.ToString();
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
        welcomeT.text = "Welcome " + PlayFabManager.SharedInstance.finalName ;
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        SongSelectorMenu.SetActive(false);
        ScoresMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ShopMenu.SetActive(false);
        LoadScreen.SetActive(true);

        SongText.text = SongList[0].ToString();
        songInfo.text = "Credits to: \n"+songList2[0];
        checkSongDificult(songsDificult[0]);
        
        PlayFabManager.SharedInstance.ActualLevel = SongText.text;
        PlayFabManager.SharedInstance.getScoreAndLevel();
        PlayFabManager.SharedInstance.GetInventory();
        PlayFabManager.SharedInstance.getCurrency();
        PlayFabManager.SharedInstance.getShop();
        
        
        yield return new WaitForSeconds(2f);
        fillShop();
        yield return new WaitForSeconds(1f);
        checkItemsToAddShop();
        StartCoroutine(OneSecond());
        MainMenu.SetActive(true);

        // Finishes loading screen
        SimpleLoadingRotation.FinishLoading();
        LoadScreen.SetActive(false);
    }

    IEnumerator waitforBool()
    {
        panelBuy.transform.Find("Check").gameObject.SetActive(false);
        panelBuy.transform.Find("Loading").gameObject.SetActive(true);
        while (PlayFabManager.SharedInstance.isProcessed == false)
        {
            yield return null;
        }
        
        StartCoroutine(OneSecond());
        PlayFabManager.SharedInstance.isProcessed = false;
            
        
    }
}
