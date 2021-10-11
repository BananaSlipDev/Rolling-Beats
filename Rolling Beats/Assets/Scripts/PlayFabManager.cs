using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{

    [Header("UI")]
    public Text messageText;

    public InputField emailInput;
    public InputField passwordInput;
    public InputField username;
    public InputField puntuacion;

    public GameObject loginUI;
    public GameObject userUI;
    public GameObject leaderboardUI;

    public GameObject rowPrefab;
    public Transform rowsParent;

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
    
    

    public string nombre;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = nombre,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    void OnLoginSuccess(LoginResult results)
    {
        loginUI.SetActive(false);
        messageText.text = "Logged In";
        Debug.Log("Succesfull");
        string name = null;
        if(results.InfoResultPayload.PlayerProfile!=null)
            name = results.InfoResultPayload.PlayerProfile.DisplayName;
        
        if (name ==null)
            userUI.SetActive(true);
        else
        {
            leaderboardUI.SetActive(true);
        }
    }
    
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("error");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Leaderboard",
                    Value = int.Parse(puntuacion.text)
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesfull statits");
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
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            Text[] text = newGO.GetComponentsInChildren<Text>();
            text[0].text = (item.Position +1).ToString();
            text[1].text = item.DisplayName;
            text[2].text = item.StatValue.ToString();
            Debug.Log(item.Position + " " +item.DisplayName+" "+item.StatValue);
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

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Update display name");
        userUI.SetActive(false);
        leaderboardUI.SetActive(true);
    }
}
