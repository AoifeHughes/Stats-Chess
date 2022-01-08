using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{

    private string[,] state;
    private string[,] colors;

    public static string[] defaultOrder = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight","rook",
                                           "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn"};

    private string currentPlayer = "white";

    public BoardState()
    {
        //setup state
        this.state = new string[8, 8];
        this.colors = new string[8, 8];


        for (int i = 0; i < defaultOrder.Length; i++)
        {
            SetPiece(defaultOrder[i], "white", i - i / 8 * 8, i / 8);
            SetPiece(defaultOrder[i], "black", i - i / 8 * 8, 7 - i / 8);
        }

    }

    public void SetPiece(string name, string color, int x, int y, int prevX = -1, int prevY = -1)
    {
        state[x, y] = name;
        colors[x, y] = color;

        if (prevX >= 0 && prevY >= 0)
        {
            state[prevX, prevY] = null;
            colors[prevX, prevY] = null;
        }
    }

    // TODO setting custom games! 
    public BoardState(string[] white, string[] black) { throw new NotImplementedException(); }


    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public int GetLength(int ax)
    {
        return state.GetLength(ax);
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= state.GetLength(0) || y >= state.GetLength(1))
        {
            return false;
        }
        return true;

    }

    public string GetPosition(int x, int y)
    {
        return this.state[x, y];
    }

    public string GetColor(int x, int y)
    {
        return this.colors[x, y];
    }

    public void SwapPlayer()
    {
        if (this.currentPlayer == "white")
        {
            this.currentPlayer = "black";
        }
        else
        {
            this.currentPlayer = "white";
        }
    }

    public void printBoard()
    {
        int rowLength = state.GetLength(0);
        int colLength = state.GetLength(1);

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                if (state[i, j] == null)
                {
                    continue;
                }
                Debug.Log(string.Format("{1},{2}: {0} ", state[i, j], i, j));
            }
        }
    }

}
