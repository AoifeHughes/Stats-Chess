using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPeople : MonoBehaviour
{

    // References 
    public GameObject controller; 
    public GameObject movePlate; 
    // Positions
    private int xBoard = -1;
    private int yBoard = -1;
    private List<List<int>> possMoves = new List<List<int>>();
    // track color
    private string color;
    private int num_moves = 0;
    // sprites
    private string board = "chess";

    public void SetMoveplate(GameObject obj) { movePlate = obj; }
    public void SetController(GameObject obj) { controller = obj; }


    public void Activate()
    {
        SetCoords();
        string loc = string.Format("Sprites/{0}/{1}_{2}", board, this.color, this.name);
        this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(loc);
        this.GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;

    }

    public void SetCoords()
    {
        float factor = 0.66f;
        float shift = -2.3f;
        float x = xBoard * factor + shift;
        float y = yBoard * factor + shift;
        this.transform.position = new Vector3(x, y, -1.0f);
    }


    public int GetXBoard() { return xBoard; }
    public int GetYBoard() { return yBoard; }
    public string GetPlayer() { return color; }
    public int GetNumMoves() { return num_moves; }

    public void SetXBoard(int x) { xBoard=x; }
    public void SetYBoard(int y) { yBoard=y; }
    public void SetPlayer(string p) { color = p; }
    public void IncNumMoves() { num_moves += 1; }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (controller.GetComponent<Game>().GetCurrentPlayer() == color && !controller.GetComponent<Game>().IsGameOver())
        {
            ResetMoveList();
            DestroyMovePlates();
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {

        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    private void GeneratePossMoves()
    {
        ResetMoveList();
        switch (this.name)
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
    }

    public List<List<int>> GetPossMoves()
    {
        GeneratePossMoves();
        return possMoves;
    }


    public void InitiateMovePlates()
    {

        GeneratePossMoves();

        for (int i = 0; i < possMoves.Count; i++)
        {
            int mx = possMoves[i][0];
            int my = possMoves[i][1];
            bool attk = (possMoves[i][2] != 0) ? true : false;

            MovePlateSpawn(mx, my, attk);
        }

    }

    private void AddPossMove(int x, int y, int attk=0)
    {

        // Put in additional checks here!
        // 1. Check movements of all opp color pieces
        //    if any can take king, then this move is not acceptable
        // Game sc = controller.GetComponent<Game>();


        possMoves.Add(new List<int>() { x, y, attk });
    }

    private void PawnMove()
    {
        Game sc = controller.GetComponent<Game>();

        int dir = (color == "white") ? 1 : -1;
        int possY;

        if (num_moves == 0)
        {
            possY = yBoard + dir * 2;
            if (sc.PositionOnBoard(xBoard, possY) && sc.GetPosition(xBoard, possY) == null && sc.GetPosition(xBoard, possY-dir) == null)
            {
                AddPossMove(xBoard, possY);
            }
        }
        possY = yBoard + dir;

        if (sc.PositionOnBoard(xBoard, possY) && sc.GetPosition(xBoard, possY) == null)
        {
            AddPossMove(xBoard, possY);
        }

        for (int i = -1; i < 2; i += 2)
        {
            int possX = xBoard + i;
            if (sc.PositionOnBoard(possX, possY) && sc.GetPosition(possX, possY) != null && sc.GetPosition(possX, possY).GetComponent<ChessPeople>().color != color)
            {
                    AddPossMove(possX, possY, 1);       
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
        for (int x = -1; x < 2; x+=2)
        {
            for (int y = -1; y < 2; y+=2)
            {
                LineMovePlate(x,y);
            }
        }
    }

    private void QueenMove()
    {
        OmniDir();
    }


    private void KnightMove()
    {
        Game sc = controller.GetComponent<Game>();
        int[] Xs = new int[] {1,2,1,2};
        int[] Ys = new int[] {2,1,-2,-1};
        for (int sign = -1; sign < 2; sign+=2)
        {
            for (int i = 0; i < Xs.Length; i++)
            {
                int possX = xBoard + Xs[i]*sign;
                int possY = yBoard + Ys[i];

                if (sc.PositionOnBoard(possX, possY))
                {
                    if (sc.GetPosition(possX, possY) == null)
                    {
                        AddPossMove(possX, possY);
                    }
                    else if (sc.GetPosition(possX, possY).GetComponent<ChessPeople>().color != color)
                    {
                        AddPossMove(possX, possY, 1);
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
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xInc;
        int y = yBoard + yInc;
        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x, y) == null)
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
        if (sc.PositionOnBoard(x,y) && sc.GetPosition(x, y) != null && sc.GetPosition(x,y).GetComponent<ChessPeople>().color != color)
        {
            AddPossMove(x, y, 1);
        }

    }

    private void MovePlateSpawn(int matX, int matY, bool attk=false)
    {
        int x0 = matX;
        int y0 = matY;
        float x = x0 * 0.66f - 2.3f;
        float y = y0 *0.66f - 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = attk;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(x0, y0);
        mpScript.Start();
    }

    public void ResetMoveList() { possMoves = new List<List<int>>(); }

}