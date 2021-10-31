using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager SharedInstance;

    [Header("UI")]
    

    public TextMeshProUGUI messageText;

    public InputField emailInput;
    public InputField passwordInput;
    public InputField username;

    public GameObject loginUI;
    public GameObject userUI;
    public GameObject leaderboardUI;
    public GameObject mobile;
    public GameObject userMobileUI;

    public GameObject rowPrefab;
    public GameObject rowsParent;

    public String finalName;

    private int Level1Score;


    private void Awake()
    {
        if (SharedInstance == null)
        {
            SharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    public void RegisterButton()
    {
        if(passwordInput.text.Length <6)
        {
            messageText.text = "PAssword too short";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
           
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered. Now you can Login!";
        sendScoreAndLevel(0);
       
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters =  new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess,OnError);
    }

    public void ResetPasswordButton()
    {
        
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier+Random.Range(0,1000),
            CreateAccount = true,
            InfoRequestParameters =  new GetPlayerCombinedInfoRequestParams
            {
            GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessNoUser, OnError);
    }
    
    void OnLoginSuccessNoUser(LoginResult results)
    {
        mobile.SetActive(false);
        loginUI.SetActive(false);
        messageText.gameObject.SetActive(false);
        userMobileUI.SetActive(true);
        //generateRandomUser();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    void OnLoginSuccess(LoginResult results)
    {
        loginUI.SetActive(false);
        messageText.gameObject.SetActive(false);
        messageText.text = "Logged In";
        finalName = results.InfoResultPayload.PlayerProfile.DisplayName;
        string name = null;
        if(results.InfoResultPayload.PlayerProfile!=null)
            name = results.InfoResultPayload.PlayerProfile.DisplayName;

        getScoreAndLevel();
        if (name == null)
        {
            userUI.SetActive(true);
        }
            
            
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
           
            //SceneManager.LoadScene("PruebaCambioEscena")
            //;
        }
    }
    
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("error");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int value)
    {
        if (value > Level1Score)
        {
            sendScoreAndLevel(value);
        }
        
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Leaderboard",
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Leaderboard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGO = Instantiate(rowPrefab, rowsParent.transform);
            TextMeshProUGUI[] text = newGO.GetComponentsInChildren<TextMeshProUGUI>();
            text[0].text = (item.Position +1).ToString();
            text[1].text = item.DisplayName;
            text[2].text = item.StatValue.ToString();
            
        }
        
    }

    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username.text,
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnDisplayNameUpdate, OnError);
    }
    
    public void generateRandomUser(String name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name,
        };
        
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnDisplayNameUpdate2, OnErrorRepitedName);
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        finalName= obj.DisplayName;
        userUI.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    
    private void OnDisplayNameUpdate2(UpdateUserTitleDisplayNameResult obj)
    {
        finalName= obj.DisplayName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void goToScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlayScene");
        //SceneManager.LoadScene("PruebaCambioEscena");
    }

    private void OnErrorRepitedName(PlayFabError error)
    {
        //TODO SI EL NOMBRE ES EL MISMO
    }

    public void sendScoreAndLevel(int score)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Level1Score", score.ToString()},
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void OnDataSend(UpdateUserDataResult result)
    {
        
    }

    public void getScoreAndLevel()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    public void OnDataReceived(GetUserDataResult result)
    {
        if (result != null && result.Data.ContainsKey("Level1Score"))
        {
            Level1Score = int.Parse(result.Data["Level1Score"].Value);
        }
        
        
    }

    

    
}
