using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Spot : MonoBehaviour
{
    public int Linha;
    public int Coluna;

    public GameObject ImageCross;
    public GameObject ImageCircle;


    private Simbolos _currentSymbol;

    public Simbolos CurrentSymbol
    {
        set
        {
            _currentSymbol = value;

            ImageCross.SetActive(_currentSymbol == Simbolos.X);
            ImageCircle.SetActive(_currentSymbol == Simbolos.O);
        }
        get
        {
            return _currentSymbol;
        }
    }

    private void Start()
    {
        CurrentSymbol = Simbolos.N;   
    }


    private void OnMouseDown()
    {
        if (OndeEstou.instance.fase == 3)
        {
            GetComponentInParent<BoardManager>().SpotClicked(this);
        }
    }   

}
