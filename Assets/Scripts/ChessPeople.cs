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
    // track color
    private string color;

    // sprites
    private string board = "chess";

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
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

    public void SetXBoard(int x) { xBoard=x; }
    public void SetYBoard(int y) { yBoard=y; }
    public void SetPlayer(string p) { color = p; }
}