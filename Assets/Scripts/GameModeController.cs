using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.UI;


public class GameModeController : MonoBehaviour
{
    public static GameModeController Instance { get; private set;}

    public event Action OnHostStarted;
    public event Action OnClientStarted;
    public event Action OnClientConnected;


    private void Awake()
    {
        Instance = this;
    }


    public void StartHost()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallbak;
        NetworkManager.Singleton.StartHost();

        OnHostStarted?.Invoke();
    }

    private void OnClientConnectedCallbak(ulong clientId)
    {
        Debug.LogFormat("Client connected: {0} (is server: {1})", clientId, NetworkManager.Singleton.IsServer);

        if (NetworkManager.Singleton.IsServer)
        {
            OnClientConnected?.Invoke();
        }
    }

    public void StartClient(string serverAddress)
    {
        var netManager = NetworkManager.Singleton;
        netManager.GetComponent<UNetTransport>().ConnectAddress = serverAddress;
        netManager.StartClient();

        OnClientStarted?.Invoke();   
        //NetworkManager.Singleton.OnClientConnectedCallback += clientId => OnClientStarted?.Invoke();
    }
}
