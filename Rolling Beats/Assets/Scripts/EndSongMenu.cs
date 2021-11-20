using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndSongMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI accuracyTXT;
    [SerializeField] private TextMeshProUGUI messageT;

    [SerializeField] private Image letter;
    [SerializeField] private Image newRecord;
    [SerializeField] private TextMeshProUGUI perfectTXT; 
    [SerializeField] private TextMeshProUGUI greatTXT;
    [SerializeField] private TextMeshProUGUI missTXT;

    [SerializeField] private TextMeshProUGUI rcoinsTXT;

    [SerializeField] private List<Sprite> letterSprites;

    private int rollingCoinsGained;
    


    void Start()
    {
        SetScore();
        CheckNewRecord();    
        AddCoins(rollingCoinsGained);
        
        PlayFabManager.SharedInstance.getCurrency();

        //Hay que pasarle la canción que acabe de terminar
        // Usar DontDestroyOnLoad
    }

    private void Awake()
    {
        messageT = PlayFabManager.SharedInstance.messageText;
    }

    public void GoToMainMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void RestartSong() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayFabManager.SharedInstance.ActualLevel);
    }


    #region Score stats
    private void SetScore()
    {
        // Set data
        score.text = "" + SceneManager.instance.totalScore;

        int perfects = SceneManager.instance.perfects,
            greats = SceneManager.instance.greats, 
            misses = SceneManager.instance.misses;

        perfectTXT.text = perfects.ToString();
        greatTXT.text = greats.ToString();
        missTXT.text = misses.ToString();


        // CALCULATE LETTER:
        int totalNotes = SceneManager.instance.totalNotes;
        
        // Calculates performance, a % between 0 and 100
        float performance = ((perfects + greats - (misses/2) ));
        performance /= totalNotes;
        performance *= 100;
        performance = Mathf.Round(performance * 100) / 100;


        if(performance < 0)
            performance = 0;


        if(performance == 100 && misses == 0 && greats == 0)    // S+, 100% perfects
        {
            letter.sprite = letterSprites[0];
            rollingCoinsGained = 100;
        }
        else if(performance == 100 && misses == 0)              // S, all scored no misses
        {
            letter.sprite = letterSprites[1];
            rollingCoinsGained = 80;
        }
        else if(performance > 80)                               // A, at least 80% scored
        {
            letter.sprite = letterSprites[2];
            rollingCoinsGained = 60;
        }
        else if(performance > 60)                               // B, at least 60% scored
        {
            letter.sprite = letterSprites[3];
            rollingCoinsGained = 40;
        }
        else if(performance <= 60)                              // C, less than 60% scored
        {
            letter.sprite = letterSprites[4];
            rollingCoinsGained = 20;
        }
            

        accuracyTXT.text = performance + "%";
    }


    private void CheckNewRecord()
    {

        if (SceneManager.instance.totalScore > PlayFabManager.SharedInstance.actualLevelScore)
        {
            newRecord.gameObject.SetActive(true);
        }
        else
            newRecord.gameObject.SetActive(false);
    }
    
    #endregion

    private void AddCoins(int rcoins)
    {
        rcoinsTXT.text = "+" + rcoins;
        PlayFabManager.SharedInstance.AddVC(rcoins);
    }
}
