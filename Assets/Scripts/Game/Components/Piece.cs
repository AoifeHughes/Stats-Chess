using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    // References 
    [SerializeField] public GameObject controller;
    [SerializeField] public GameObject movePlate;
    [SerializeField] public GridManager chessboard;

    // Positions
    private int xBoard = -1;
    private int yBoard = -1;
    // track color
    private string color;
    private int num_moves = 0;
    // sprites
    public bool IsAI = false;

    IDictionary<string, int> SpriteNumbers = new Dictionary<string, int>()
    {
        {"pawn",0 }, {"knight", 1}, {"bishop",2}, {"rook",3}, {"queen",4},
        {"king",5 }
    };


    private Sprite GetSprite(string name)
    {
        Sprite[] all = Resources.LoadAll<Sprite>("Sprites/chess");

        foreach (var s in all)
        {
            if (s.name == name)
            {
                return s;
            }
        }
        return null;
    }

    public void Activate()
    {
  
        SetCoords();
        int n = SpriteNumbers[this.name];
        if (color == "white")
        {
            n += 6;
        }
        string sn = string.Format("chess_{0}", n);
        this.GetComponent<SpriteRenderer>().sprite = GetSprite(sn);
        this.GetComponent<SpriteRenderer>().sprite.texture.filterMode = FilterMode.Point;

        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio

        float width_per_block = (cameraWidth < cameraHeight) ? cameraWidth / 8 : cameraHeight / 8;
        this.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(width_per_block, width_per_block, width_per_block);



    }

    public void SetCoords()
    {

        chessboard = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();

        Tile tile = chessboard.GetTileAtPosition(new Vector2(xBoard, yBoard));
        Vector3 pos = tile.transform.position;

        this.transform.position = new Vector3(pos[0], pos[1], -1.0f);
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