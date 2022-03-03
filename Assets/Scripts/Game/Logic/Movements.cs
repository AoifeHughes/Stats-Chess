using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements 
{

    [SerializeField] public GameObject controller;

    private List<(int x, int y, bool attack)> possMoves = new List<(int, int, bool)>();
    private int xBoard, yBoard, num_moves;
    private string color;
    private string name;
    private BoardState state;

    public List<(int x, int y, bool attack)> GenerateMovements(string name, int xBoard, int yBoard, string color, BoardState state, int num_moves = 0, bool filter_suicide=false)
    {
        this.xBoard = xBoard;
        this.yBoard = yBoard;
        this.num_moves = num_moves;
        this.color = color;
        this.state = state;
        this.name = name;

        switch (name)
        {
            case "rook":
                RookMove();
                break;
            case "bishop":
                BishopMove();
                break;
            case "knight":
                KnightMove();
                break;
            case "queen":
                QueenMove();
                break;
            case "king":
                KingMove();
                break;
            case "pawn":
                PawnMove();
                break;
            default:
                break;
        }

        if (filter_suicide)
        {
            RemoveSuicideMoves();
        }

        // When this is zero then checkmate
        return possMoves;

    }


    public void RemoveSuicideMoves()
    {
        List<int> toRemove = new List<int>();
        int idx = 0;
        foreach (var m in possMoves)
        {
            int x = m.x;
            int y = m.y;
            state.SetPiece(name, color, x, y, xBoard, yBoard);

            if (state.IsCheck(color, state))
            {
                toRemove.Add(idx);
            }
            state.Undo();
            idx++;
        }

        if (toRemove.Count > 0)
        {
            possMoves.RemoveAll(x => toRemove.Contains(possMoves.IndexOf(x)));
        }

    }

    private void AddPossMove(int x, int y, bool attk = false)
    {
        possMoves.Add( ( x, y, attk ));
    }

    private void PawnMove()
    {
        int dir = (color == "white") ? 1 : -1;
        int possY;

        if (num_moves == 0)
        {
            possY = yBoard + dir * 2;
            if (state.PositionOnBoard(xBoard, possY) && state.GetPosition(xBoard, possY) == null && state.GetPosition(xBoard, possY - dir) == null)
            {
                AddPossMove(xBoard, possY);
            }
        }
        possY = yBoard + dir;

        if (state.PositionOnBoard(xBoard, possY) && state.GetPosition(xBoard, possY) == null)
        {
            AddPossMove(xBoard, possY);
        }

        for (int i = -1; i < 2; i += 2)
        {
            int possX = xBoard + i;
            if (state.PositionOnBoard(possX, possY) && state.GetPosition(possX, possY) != null && state.GetColor(possX, possY) != color)
            {
                AddPossMove(possX, possY, true);
            }
        } 
        // TODO: Add en passant *spelling
        // TODO: Add transform at end of board


    }

    private void KingMove()
    {
        OmniDir(true);
        // TODO: Castling
    }


    private void RookMove()
    {
        for (int sign = -1; sign < 2; sign += 2)
        {
            LineMovePlate(1 * sign, 0);
            LineMovePlate(0, 1 * sign);
        }
    }

    private void BishopMove()
    {
        for (int x = -1; x < 2; x += 2)
        {
            for (int y = -1; y < 2; y += 2)
            {
                LineMovePlate(x, y);
            }
        }
    }

    private void QueenMove()
    {
        OmniDir();
    }


    private void KnightMove()
    {

        int[] Xs = new int[] { 1, 2, 1, 2 };
        int[] Ys = new int[] { 2, 1, -2, -1 };
        for (int sign = -1; sign < 2; sign += 2)
        {
            for (int i = 0; i < Xs.Length; i++)
            {
                int possX = xBoard + Xs[i] * sign;
                int possY = yBoard + Ys[i];

                if (state.PositionOnBoard(possX, possY))
                {
                    if (state.GetPosition(possX, possY) == null)
                    {
                        AddPossMove(possX, possY);
                    }
                    else if (state.GetColor(possX, possY) != color)
                    {
                        AddPossMove(possX, possY, true);
                    }
                }
            }
        }

    }

    private void OmniDir(bool king = false)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                LineMovePlate(i, j, king);
            }
        }
    }

    private void LineMovePlate(int xInc, int yInc, bool king = false)
    {
        // This is bork!
        int x = xBoard + xInc;
        int y = yBoard + yInc;
        while (state.PositionOnBoard(x, y) && state.GetPosition(x, y) == null)
        {

            AddPossMove(x, y);
            x += xInc;
            y += yInc;
            if (king)
            {
                x = xBoard + xInc;
                y = yBoard + yInc;
                break;
            }
        }
        if (state.PositionOnBoard(x, y) && state.GetPosition(x, y) != null && state.GetColor(x, y) != color)
        {
            AddPossMove(x, y, true);
        }

    }

}