using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioClip[] clip;
    public AudioSource audioS;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioS = GetComponent<AudioSource>();
    }    

    void Update()
    {      

        if (!audioS.isPlaying)
        {            
            if (OndeEstou.instance.fase >= 2)
            {
                audioS.clip = clip[0];
                audioS.Play();
            }
            else
            {
                audioS.clip = GetRandom();
                audioS.Play();
            }
            
        }
    }

    AudioClip GetRandom()
    {
        return clip[Random.Range(0, clip.Length)];
    }
}
