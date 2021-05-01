using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public InputField ServerAddressField;

    public void StartHost()
    {
        GameModeController.Instance.StartHost();
        //gameObject.SetActive(false);
    }

    public void StarClient()
    {
        GameModeController.Instance.StartClient(ServerAddressField.text);
        //gameObject.SetActive(false);
    }
}
