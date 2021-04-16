using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax : MonoBehaviour
{
    public struct Play
    {
        public int Line, Column;
        public int Score;
    }

    private static Simbolos[,] _boardData;

    //Executa uma rodada de algoritmo MinMax podendo ser recursivamente
    public static bool DoMinMax(BoardManager board, Simbolos player, out Play bestPlay)
    {
        bool isMin = (player == Simbolos.O);

        bestPlay.Line = bestPlay.Column = -1;
        bestPlay.Score = (isMin ? 999 : -999);

        //Verifica se tem vencedor
        Simbolos winner = board.GetWinner(); 
        if (winner != Simbolos.N)
        {
            switch (winner)
            {
                case Simbolos.O:
                    bestPlay.Score = -100;
                    break;
                case Simbolos.X:
                    bestPlay.Score = 100;
                    break;
            }

            return false;
        }
        
        //Se não houver ganhador, tenta encontrar outra jogada válida para o jogador atual
        bool foundAnyPlay = false;
        for (int l = 0; l < board.BoardSize; l++)
        {
            for (int c = 0; c < board.BoardSize; c++)
            {
                if (board.GetSymbolAt(l, c) != Simbolos.N)
                {
                    continue;
                }

                foundAnyPlay = true;

                board.SetSymbolAt(l, c, player);

                DoMinMax(board, player.GetOppositeSymbol(), out var nextPlay);

                if ((isMin && nextPlay.Score <= bestPlay.Score)
                    || (!isMin && nextPlay.Score >= bestPlay.Score))
                {
                    if (bestPlay.Score != nextPlay.Score || Random.value < 0.2f)
                    {
                        bestPlay.Score = nextPlay.Score;
                        bestPlay.Line = l;
                        bestPlay.Column = c;
                    }
                }

                board.SetSymbolAt(l, c, Simbolos.N);
            }
        }

        if (!foundAnyPlay)
        {
            bestPlay.Score = 0;
        }       

        return foundAnyPlay;
    }
}
