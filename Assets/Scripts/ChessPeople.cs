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

    public void OnMouseUp()
    {
        Debug.Log(this.name);

        DestroyMovePlates();
        InitiateMovePlates();

    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        //TODO: Implement!
        switch (this.name)
        {
            case "queen":
                Debug.Log("Making a moveplate!");
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            default:
                break;
        }
    }

    private void LineMovePlate(int xInc, int yInc)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xInc;
        int y = yBoard + yInc;

        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xInc;
            y += yInc;
        }

        if (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y).GetComponent<ChessPeople>().color != color)
        {
            MovePlateSpawn(x, y, true);
        }
    }

    private void MovePlateSpawn(int matX, int matY, bool att=false)
    {
        int x0 = matX;
        int y0 = matY;
        float x = x0 * 0.66f - 2.3f;
        float y = y0 *0.66f - 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = att;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(x0, y0);
    }

}