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
        messageText.text = "Registered and logged in!";
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = emailInput.text,
            Password = passwordInput.text
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
        messageText.text = "Logged In";
        Debug.Log("Succesfull");
    }
    
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log("error");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Score",
                    Value = score
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
            StatisticName = "Score",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
        
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " +item.PlayFabId+ "  "+item.DisplayName+" "+item.StatValue);
        }
        
    }
}
