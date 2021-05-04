using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class UIMANAGER : MonoBehaviour
{
    public static UIMANAGER instance;

    private Animator panelWinAnim, panelLoseAnim, panelPigWinAnim, panelBirdWinAnim, panelTiedAnim;
    private Button winBtnMenu, winBtnAgain;
    private Button loseBtnMenu, loseBtnAgain;
    private Button birdBtn, pigBtn;   

    private Button BTN_Easy, BTN_Hard, BTN_Normal;
        
    public int dificuldade;   
    
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
        SceneManager.sceneLoaded += Carrega;
    }

    void Carrega(Scene cena, LoadSceneMode modo)
    {
        if(OndeEstou.instance.fase == 3)
        {
            //painel
            panelWinAnim = GameObject.Find("Menu_Win").GetComponent<Animator>();
            panelLoseAnim = GameObject.Find("Menu_Lose").GetComponent<Animator>();
            panelPigWinAnim = GameObject.Find("Menu_Pig").GetComponent<Animator>();
            panelBirdWinAnim = GameObject.Find("Menu_Bird").GetComponent<Animator>();
            panelTiedAnim = GameObject.Find("Menu_Tied").GetComponent<Animator>();

            //BTN_win
            winBtnAgain = GameObject.Find("BTN_Reiniciar").GetComponent<Button>();
            winBtnMenu = GameObject.Find("BTN_Menu").GetComponent<Button>();
            //BTN_Lose
            loseBtnAgain = GameObject.Find("BTN_Reiniciar_Lose").GetComponent<Button>();
            loseBtnMenu = GameObject.Find("BTN_Menu_Lose").GetComponent<Button>();
        }
    }

    void Start()
    {
        //fase LevelMenu
        if (OndeEstou.instance.fase == 1)
        {
            BTN_Easy = GameObject.FindWithTag("easymode").GetComponent<Button>();
            BTN_Normal = GameObject.FindWithTag("normalmode").GetComponent<Button>();
            BTN_Hard = GameObject.FindWithTag("hardmode").GetComponent<Button>();            
        }
    }

    void Update()
    {
        //fase PlayGame
        if (OndeEstou.instance.fase == 3)
        {
            AtivarAnimacao();
        }        
    }
    public int getDificuldade()
    {
        return dificuldade;
    }

    public void setDificuldade(int dificul)
    {
        dificuldade = dificul;
    }

    public void ReiniciarJogo()
    {
        if(BoardManager.instance.level == Difficult.hard)
        {
            difficultHard();
        }
        if (BoardManager.instance.level == Difficult.easy)
        {
            difficultEasy();
        }
        if(BoardManager.instance.level == Difficult.normal)
        {
            difficultNormal();
        }
        if (BoardManager.instance.level == Difficult.pvpLocal)
        {
            PvPLocal();
        }
        if (BoardManager.instance.level == Difficult.pvpOn)
        {
            PvPOnline();
        }
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");                
    }
    

    public void difficultEasy()
    {
        dificuldade = 1;
        SceneManager.LoadScene("WhoStarts");        
    }

    public void difficultHard()
    {
        dificuldade = 2;
        SceneManager.LoadScene("WhoStarts");        
    }

    public void difficultNormal()
    {
        dificuldade = 3;
        SceneManager.LoadScene("WhoStarts");        
    }

    public void PvPLocal()
    {
        dificuldade = 4;
        SceneManager.LoadScene("WhoStarts");        
    }

    public void PvPOnline()
    {
        dificuldade = 5;
    }

    public void AtivarAnimacao()
    {
        if (BoardManager.instance.getStatus() == 1)
        {
            //derrota
            AudioManager.instance.audioS.Stop();            
            panelLoseAnim.Play("MenuLoseAnim");
        }
        if (BoardManager.instance.getStatus() == 2)
        {
            //vitoria
            AudioManager.instance.audioS.Stop();            
            panelWinAnim.Play("MenuWinAnim");
        }
        if (BoardManager.instance.getStatus() == 3)
        {
            //empate
            AudioManager.instance.audioS.Stop();
            panelTiedAnim.Play("MenuTiedAnim");
        }
        if (BoardManager.instance.getStatus() == 4)
        {
            //pigWin
            AudioManager.instance.audioS.Stop();            
            panelPigWinAnim.Play("MenuPigWinAnim");
        }
        else if (BoardManager.instance.getStatus() == 5)
        {
            //birdWin
            AudioManager.instance.audioS.Stop();           
            panelBirdWinAnim.Play("MenuBirdWinAnim");
        }
    }    

}
