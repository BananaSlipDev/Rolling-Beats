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

    public int actualLevelScore;
    public string ActualLevel;

    public String randomName;
    public String actualScene;

    public List<String> songs;

    public int RollingCoins = 0;

    public Dictionary<String, int> itemsAvailable = new Dictionary<string, int>();


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
            messageText.text = "Password too short";
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
        //sendScoreAndLevel(0);
       
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
        songs = new List<string>();
        songs.Add("Tutorial");
        songs.Add("Snake Eyes");
        songs.Add("Amongus");
        songs.Add("Battlecry");
    }

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
        messageText.gameObject.SetActive(true);
        userMobileUI.SetActive(true);
        
        //generateRandomUser();
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    void OnLoginSuccess(LoginResult results)
    {
        
        loginUI.SetActive(false);
        messageText.gameObject.SetActive(true);
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
        //messageText.text = error.ErrorMessage;
        Debug.Log("error");
        Debug.Log(error.ErrorMessage);
    }

    public void SendLeaderboard(int value, String level)
    {
        if (value > actualLevelScore)
        {
            sendScoreAndLevel(value, level);
        }
        
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = level,
                    Value = value
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        
    }

    public void GetLeaderboard(string level)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = level,
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
            DisplayName = name+ Random.Range(0,10000),
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
        generateRandomUser(randomName);
    }

    public void sendScoreAndLevel(int score, String level)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {level, score.ToString()},
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
        if (result != null && result.Data.ContainsKey(ActualLevel))
        {
            actualLevelScore = int.Parse(result.Data[ActualLevel].Value);
        }
        else
        {
            actualLevelScore = 0;
        }
        
        
    }

    public void addCoins()
    {
        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest(), OnCurrencyAdded, OnError);
        
        
    }

    public void OnCurrencyAdded(ModifyUserVirtualCurrencyResult result)
    {
        
    }

    //Get the received item listed in the Server and add it to the user Inventory
    public void makePurchase(String mapName)
    {
        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest {
            // In your game, this should just be a constant matching your primary catalog
            CatalogVersion = "Inventory",
            ItemId = mapName,
            Price = itemsAvailable[mapName],
            VirtualCurrency = "RC"
        }, PurchaseSuccess, OnError);
    }

    void PurchaseSuccess(PurchaseItemResult result)
    {
        GetInventory();
        getCurrency();
    }
    
    public void GetInventory() 
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),OnGetInventory,error => Debug.LogError(error.GenerateErrorReport()));
    }
 
 
 
 
    public void OnGetInventory(GetUserInventoryResult result)
    {
        foreach (var eachItem in result.Inventory)
        {
            if(!songs.Contains(eachItem.DisplayName))
                songs.Add(eachItem.DisplayName);
        }
           
    }

    public void getCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetCurrency, error=> Debug.LogError("NoCurrency"));
    }
    
    public void OnGetCurrency(GetUserInventoryResult result)
    {
        RollingCoins = result.VirtualCurrency["RC"];

    }
    
    public void AddVC(int coins)
    {
        AddUserVirtualCurrencyRequest vcRequest = new AddUserVirtualCurrencyRequest();

        vcRequest.VirtualCurrency = "RC";
        vcRequest.Amount = coins;

        PlayFabClientAPI.AddUserVirtualCurrency(vcRequest, onAddedSucces, error=> Debug.LogError(error.Error));
        
    }

    public void onAddedSucces(ModifyUserVirtualCurrencyResult result)
    {
        //Debug.Log("RC Added");
    }

    public void getShop()
    {
        GetCatalogItemsRequest vcRequest = new GetCatalogItemsRequest();

        vcRequest.CatalogVersion = "Inventory";
        
        PlayFabClientAPI.GetCatalogItems(vcRequest,itemsGet, error => Debug.LogError(error.Error));
    }

    void itemsGet(GetCatalogItemsResult result)
    {
        foreach (var item in result.Catalog)
        {
            if(!itemsAvailable.ContainsKey(item.DisplayName))
                itemsAvailable.Add(item.DisplayName,Convert.ToInt32(item.VirtualCurrencyPrices["RC"]));
            
        }
    }
    
    
    

    

    

    
}
