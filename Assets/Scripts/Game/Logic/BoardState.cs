using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardState
{

    private string[,] state;
    private string[,] colors;

    public static string[] defaultOrder = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight","rook",
                                           "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn"};

    private string currentPlayer = "white";


    private List<(string[,], string[,])> history = new List<(string[,], string[,])>();

    public BoardState()
    {
        //setup state
        this.state = new string[8, 8];
        this.colors = new string[8, 8];


        for (int i = 0; i < defaultOrder.Length; i++)
        {
            SetPiece(defaultOrder[i], "white", i - i / 8 * 8, i / 8, record: false);
            SetPiece(defaultOrder[i], "black", i - i / 8 * 8, 7 - i / 8, record: false);
        }
        history.Add((state, colors));
    }

    public void Undo()
    {
        this.state = history.Last().Item1;
        this.colors = history.Last().Item2;
    }


    public void SetPiece(string name, string color, int x, int y, int prevX = -1, int prevY = -1, bool record = true)
    {
        if (record)
        {
            history.Add(((string[,])state.Clone(), (string[,])colors.Clone()));
        }

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

    public IEnumerable<(string piece, string color, int x, int y)> IterateBoard()
    {
        for (int i = 0; i < state.GetLength(0); i++)
        {
            for (int j = 0; j < state.GetLength(1); j++)
            {
                yield return (state[i, j],colors[i, j], i, j);
            }
        }
    }


    private (int x,  int y) FindKing(string color)
    {
        foreach (var p in IterateBoard())
        {
            if (p.color == color && p.piece == "king")
            {
                return (p.x, p.y);
            }
        }

        return (-1, -1); // fail state i.e. no king
    }

    public bool IsCheck(string color, BoardState state)
    {
        (int x, int y) kingXY = FindKing(color);

        // for each piece of oppo color, check if they can move to king position!

        foreach (var p in IterateBoard())
        {
            Movements moves = new Movements();
            foreach (var m in moves.GenerateMovements(p.piece, p.x, p.y, p.color, state))
            {
                int x = m.x;
                int y = m.y;
                bool attack = m.attack;

                if (!attack)
                {
                    continue;
                } 

                if (x == kingXY.x && y == kingXY.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

}
