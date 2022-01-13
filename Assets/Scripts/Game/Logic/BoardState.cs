﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardState : ICloneable
{

    private string[,] state;
    private string[,] colors;

    public enum Conditions
    {
        Check,
        Checkmate,
        Draw,
        Play
    }

    public static string[] defaultOrder = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight","rook",
                                           "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn"};

    private string currentPlayer = "white";


    private List<(string[,], string[,])> history;

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
        history = new List<(string[,], string[,])>();
        AddToHistory();

    }

    private void AddToHistory()
    {

        string[,] cS = (string[,])state.Clone();
        string[,] cC = (string[,])colors.Clone();
        history.Add((cS, cC));

    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public int CountPieces()
    {
        int count = 0;
        for (int k = 0; k < state.GetLength(0); k++)
            for (int l = 0; l < state.GetLength(1); l++)
                if (state[k,l] != null)
                {
                    count++;
                }
        return count;
    }

    public void Undo()
    {
        //TODO: This is realllly slow, need a better way of doing it!
        state = (string[,])history[history.Count-1].Item1.Clone();
        colors = (string[,])history[history.Count-1].Item2.Clone();
    }


    public void SetPiece(string name, string color, int x, int y, int prevX = -1, int prevY = -1, bool record = true)
    {


        state[x, y] = name;
        colors[x, y] = color;

        if (prevX >= 0 && prevY >= 0)
        {
            state[prevX, prevY] = null;
            colors[prevX, prevY] = null;
        }

        if (record)
        {
            AddToHistory();
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

    public Conditions CheckPlayState(string color, BoardState state)
    {   // is color in check, basically
        (int x, int y) kingXY = FindKing(color);

        bool check = false;
        Movements moves = new Movements();

        // now check if color can move
        int tn = 0;
        foreach (var p in IterateBoard())
        {   
            if (p.color != color)
            {
                foreach (var m in moves.GenerateMovements(p.piece, p.x, p.y, p.color, state))
                {
                    if (!m.attack)
                    {
                        continue;
                    }
                    if (m.x == kingXY.x && m.y == kingXY.y)
                    {
                        check = true;
                    }
                }
            }
            else
            {
                tn += moves.GenerateMovements(p.piece, p.x, p.y, p.color, state, filter_self_check: true).Count;
            }
        }

        if (check && tn == 0)
        {
            return Conditions.Checkmate;
        }

        if (tn == 0)
        {
            return Conditions.Draw;
        }
        return Conditions.Play;
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
          
                if (!m.attack)
                {
                    continue;
                } 

                if (m.x == kingXY.x && m.y == kingXY.y)
                {
                    return true;
                }
            }
        }
        return false;
    }

}
