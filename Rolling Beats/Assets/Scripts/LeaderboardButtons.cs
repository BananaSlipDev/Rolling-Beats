using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardButtons : MonoBehaviour
{

    public Button enviar;

    public Button recibir;

    public InputField puntuacion;
    
    // Start is called before the first frame update

    private void Awake()
    {
        enviar.onClick.AddListener(enviarPuntuacion);
        recibir.onClick.AddListener(recibirPuntuacion);
        PlayFabManager.SharedInstance.rowsParent = GameObject.Find("TAble");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void recibirPuntuacion()
    {
        PlayFabManager.SharedInstance.GetLeaderboard();
    }

    void enviarPuntuacion()
    {
        PlayFabManager.SharedInstance.SendLeaderboard(int.Parse(puntuacion.text));
    }
}
