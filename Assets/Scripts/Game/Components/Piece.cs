using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    // References 
    [SerializeField] public GameObject controller;
    [SerializeField] public GameObject movePlate;


    // Positions
    private int xBoard = -1;
    private int yBoard = -1;
    // track color
    private string color;
    private int num_moves = 0;
    // sprites
    private string board = "chess";
    public bool IsAI = false;

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
    public string GetName() { return this.name; }
    public void SetXYBoard(int x, int y)
    {
        xBoard = x;
        yBoard = y;
        SetCoords();

    }
    public void SetPlayer(string p) { color = p; }
    public void IncNumMoves() { num_moves += 1; }

    public void OnMouseUp()
    {
        if (!IsAI)
        {
            controller = GameObject.FindGameObjectWithTag("GameController");
            if (controller.GetComponent<Game>().GetCurrentPlayer() == color && !controller.GetComponent<Game>().IsGameOver())
            {
                DestroyMovePlates();
                InitiateMovePlates();
            }
        }
    }


    public void InitiateMovePlates()
    {
        BoardState state = controller.GetComponent<Game>().GetCurrentState();
        Movements moves = new Movements();
        List<(int x, int y, bool attack)> possMoves = moves.GenerateMovements(this.name, xBoard, yBoard, color, state, num_moves, true);

        foreach (var m in possMoves)
        {
            int mx = m.x;
            int my = m.y;
            bool attack = m.attack;
            MovePlateSpawn(mx, my, attack);
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

}