using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstPlay : MonoBehaviour
{
    public static FirstPlay instance;

    public int whoPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    public void BirdSelect()
    {
        whoPlayer = 1;
        SceneManager.LoadScene("PlayGame");
    }
    public void PigSelect()
    {
        whoPlayer = 2;                
        SceneManager.LoadScene("PlayGame");
    }

    public void SelectPlayerBird()
    {
        whoPlayer = 1;
        SceneManager.LoadScene("PlayGameOnline");
    }
    public void SelectPlayerPig()
    {
        whoPlayer = 2;
        SceneManager.LoadScene("PlayGameOnline");
    }
}
