using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Difficult
{
    easy,
    normal,
    hard,
    pvp
}


public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    
    public int BoardSize = 3;
    public Simbolos SimboloInicial;    
    public List<Simbolos> AiPlayers;
    public Difficult level;

    private Simbolos _jogadorAtual;
    private Simbolos[,] _tabuleiro;
    private Spot[,] _spots; 
    private int linha, coluna;        

    // 1 = vitoria; 2 = derrota; 3 = empate
    private int statusGame;
    public bool terminou = false;
    private float modoNormal = 0; 
    

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

        if (UIMANAGER.instance.getDificuldade() == 2)
        {
            level = Difficult.hard;
        }
        if (UIMANAGER.instance.getDificuldade() == 3)
        {
            level = Difficult.normal;
        }
        if (UIMANAGER.instance.getDificuldade() == 4)
        {
            level = Difficult.pvp;
        }

        _tabuleiro = new Simbolos[BoardSize, BoardSize];
        _spots = new Spot[BoardSize, BoardSize];

        var allSpots = GetComponentsInChildren<Spot>();
        foreach (var spot in allSpots)
        {
            _spots[spot.Linha, spot.Coluna] = spot;
        }

        SimboloInicial = getSimboloInicial();
    }

    private void Start()
    {
        _jogadorAtual = SimboloInicial;
        statusGame = 0;              
        CheckForAiRound();
    }

    public int getStatus()
    {
        return statusGame;
    }

    public void setStatus(int newStatus)
    {
        statusGame = newStatus; 
    }

    public Simbolos getSimboloInicial()
    {
        if (FirstPlay.instance.whoPlayer == 1)
        {
            SimboloInicial = Simbolos.X;
        }
        if (FirstPlay.instance.whoPlayer == 2)
        {
            SimboloInicial = Simbolos.O;
        }

        return SimboloInicial;
    }

    public void setSimboloInicial(Simbolos player)
    {
        _jogadorAtual = player;
    }

    public void SpotClicked(Spot spot)
    {
        if(terminou == false && GetSymbolAt(spot.Linha, spot.Coluna) == Simbolos.N)
        {           
            marcaJogada(spot.Linha, spot.Coluna);
        }
        
    }
    public void SetSymbolAt(int line, int column, Simbolos symbol)
    {
        _tabuleiro[line, column] = symbol;
        _spots[line, column].CurrentSymbol = symbol;        
    }

    public Simbolos GetSymbolAt(int line, int column)
    {
        return _tabuleiro[line, column];
    }   

    public void marcaJogada(int line, int column)
    {
        SetSymbolAt(line, column, _jogadorAtual);
        _jogadorAtual = _jogadorAtual.GetOppositeSymbol();        

        var winner = GetWinner();
        if (winner == Simbolos.O && level != Difficult.pvp)
        {
            terminou = true;
            setStatus(statusGame = 1);            
        }
        if (winner == Simbolos.X && level != Difficult.pvp)
        {
            setStatus(statusGame = 2);
            terminou = true;
        }
        if(contarProfundidade(_tabuleiro) == 0 && winner != Simbolos.O && winner != Simbolos.X) //testa se da empate
        {
            setStatus(statusGame = 3);
            terminou = true;
        }
        if (winner == Simbolos.O && level == Difficult.pvp)
        {
            terminou = true;
            setStatus(statusGame = 4);            
        }
        if (winner == Simbolos.X && level == Difficult.pvp)
        {
            setStatus(statusGame = 5);
            terminou = true;
        }
        CheckForAiRound();
    }   

    private void modoEasy()
    {
        while(_tabuleiro[linha, coluna] == Simbolos.X || _tabuleiro[linha, coluna] == Simbolos.O)
        {
            if (contarProfundidade(_tabuleiro) == 0)
            {
                break;
            }
            linha = Random.Range(0, 3); //numRand.Next(3);
            coluna = Random.Range(0, 3); //numRand.Next(3);            
        }         
        if (_tabuleiro[linha,coluna] == Simbolos.N && _jogadorAtual == Simbolos.O)
        {            
            marcaJogada(linha, coluna);
        }                
    }

    private void CheckForAiRound()
    {        
        var winner = GetWinner();
        if (winner == Simbolos.N)
        {
            if (AiPlayers.Contains(_jogadorAtual))
            {
                if(level == Difficult.hard)
                {                    
                    if (MinMax.DoMinMax(this, _jogadorAtual, out var bestPlay))
                    {
                        marcaJogada(bestPlay.Line, bestPlay.Column);
                    }
                    else
                        setStatus(statusGame = 3);
                }
                if(level == Difficult.easy)
                {
                    modoEasy();
                }
                if(level == Difficult.normal)
                {
                    modoNormal = Random.Range(0, 10);
                    if ((modoNormal >= 2) && MinMax.DoMinMax(this, _jogadorAtual, out var bestPlay))
                    {                                                
                        marcaJogada(bestPlay.Line, bestPlay.Column);                                                
                    }                    
                    else if (modoNormal <= 1)
                    {                                                
                        modoEasy();
                    }     
                }
            }
        }
    }

    public static int contarProfundidade(Simbolos[,] boardAtual)
    {
        int prof = 0;
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (boardAtual[i, j].GetOppositeSymbol() == Simbolos.N)
                {
                    prof += 1;
                }
        return prof;
    }

    public Simbolos GetWinner()
    {
        // horizontal
        for (int line = 0; line < BoardSize; line++)
        {
            if (_tabuleiro[line, 0] == Simbolos.N)
            {
                continue;
            }

            int symbolCount = 0;
            for (int c = 0; c < BoardSize; c++)
            {
                if (_tabuleiro[line, c] == _tabuleiro[line, 0])
                {
                    symbolCount++;
                }
            }

            if (symbolCount == BoardSize)
            {
                return _tabuleiro[line, 0];
            }
        }

        // vertical
        for (int column = 0; column < BoardSize; column++)
        {
            if (_tabuleiro[0, column] == Simbolos.N)
            {
                continue;
            }

            int symbolCount = 0;
            for (int l = 0; l < BoardSize; l++)
            {
                if (_tabuleiro[l, column] == _tabuleiro[0, column])
                {
                    symbolCount++;
                }
            }

            if (symbolCount == BoardSize)
            {
                return _tabuleiro[0, column];
            }
        }

        // diagonal
        int diagonalCount1 = 0;
        int diagonalCount2 = 0;
        for (int index = 0; index < BoardSize; index++)
        {
            if (_tabuleiro[index, index] == _tabuleiro[0, 0] && _tabuleiro[0, 0] != Simbolos.N)
            {
                diagonalCount1++;
            }
            if (_tabuleiro[index, BoardSize - index - 1] == _tabuleiro[0, BoardSize - 1] && _tabuleiro[0, BoardSize - 1] != Simbolos.N)
            {
                diagonalCount2++;
            }
        }
        if (diagonalCount1 == BoardSize)
        {
            return _tabuleiro[0, 0];
        }
        if (diagonalCount2 == BoardSize)
        {
            return _tabuleiro[0, BoardSize - 1];
        }

        return Simbolos.N;
    }

    /*public Simbolos vencedorAtual(Simbolos[,] atual)
    {
        if (vencedorDiagonal(atual, Simbolos.X)) return Simbolos.X;
        if (vencedorDiagonal(atual, Simbolos.O)) return Simbolos.O;
        if (vencedorLinha(atual, Simbolos.X)) return Simbolos.X;
        if (vencedorLinha(atual, Simbolos.O)) return Simbolos.O;
        if (vencedorColuna(atual, Simbolos.X)) return Simbolos.X;
        if (vencedorColuna(atual, Simbolos.O)) return Simbolos.O;
        //if (prof == 0) return Simbolos.N;
        return Simbolos.N;
    }

    public bool vencedorLinha(Simbolos[,] board, Simbolos caracter)
    {
        for (int i = 0; i < 3; i++)
        {
            int cont = 0;
            for (int j = 0; j < 3; j++)
            {
                if (board[i,j] == Simbolos.N)
                {
                    continue;
                }
                if (board[i, j].GetOppositeSymbol() == caracter) break;
                cont++;
            }
            if (cont == 3) return true;
        }
        return false;
    }

    
    public bool vencedorColuna(Simbolos[,] board, Simbolos caracter)
    {
        for (int j = 0; j < 3; j++)
        {
            int cont = 0;
            for (int i = 0; i < 3; i++)
            {
                if (board[i, j] == Simbolos.N)
                {
                    continue;
                }
                if (board[i, j].GetOppositeSymbol() == caracter) break;
                cont++;
            }
            if (cont == 3) return true;
        }
        return false;
    }

    
    public bool vencedorDiagonal(Simbolos[,] board, Simbolos simbol)
    {
        int cont = 0;
        Simbolos _simboloAtual = simbol.GetOppositeSymbol();
        for (int i = 0; i < 3; i++)
        {
            if (board[i, i] == Simbolos.N)
            {
                continue;
            }
            if (board[i, i].GetOppositeSymbol() == simbol) break;
            cont++;
        }
        if (cont == 3) return true;

        cont = 0;
        for (int i = 0; i < 3; i++)
        {
            if (board[i, i] == Simbolos.N)
            {
                continue;
            }
            if (board[i, 2 - i].GetOppositeSymbol() == simbol) break;
            cont++;
        }
        if (cont == 3) return true;

        return false;
    }*/
    
    
}
