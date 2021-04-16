using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Simbolos
{
    N,
    X,
    O
}
public static class ExtensaoSimbolos
{
    public static Simbolos GetOppositeSymbol(this Simbolos symbol)
    {
        switch (symbol)
        {
            case Simbolos.X: return Simbolos.O;
            case Simbolos.O: return Simbolos.X;
            default: return Simbolos.N;
        }
    }

    static void Example()
    {
        Simbolos x = Simbolos.O;        
        Simbolos other1 = x.GetOppositeSymbol();
    }
}
