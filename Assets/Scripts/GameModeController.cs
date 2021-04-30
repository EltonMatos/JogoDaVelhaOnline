using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.UI;


public class GameModeController : MonoBehaviour
{
    public InputField ServerAddressField;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        var netManager = NetworkManager.Singleton;
        //netManager = GetComponent<U>
    }
}
