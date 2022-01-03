using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject chesspiece;
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];
    public static string[] pieceOrders = { "rook", "knight", "bishop", "queen", "king", "bishop", "knight","rook",
                                           "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn", "pawn"};

    private string currentPlayer = "white";

    private bool gameOver = false; 

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < playerWhite.Length; i++)
        {
            playerWhite[i] = Create(pieceOrders[i], "white", i-i/8*8 ,i/8);
            playerBlack[i] = Create(pieceOrders[i], "black", i - i / 8 * 8, 7-i / 8);
        }

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }

    }


    public GameObject Create(string name, string player, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessPeople cp = obj.GetComponent<ChessPeople>();
        cp.name = name;
        cp.SetPlayer(player);
        cp.SetXBoard(x);
        cp.SetYBoard(y);
        cp.Activate();
        return obj;

    }

    public void SetPosition(GameObject obj)
    {
        ChessPeople cp = obj.GetComponent<ChessPeople>();
        positions[cp.GetXBoard(), cp.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null; 
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public GameObject[] GetPlayer(bool getWhite)
    {
        return (getWhite) ? playerWhite : playerBlack;
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        return true;
    }

    public string GetCurrentPlayer() { return currentPlayer; }

    public bool IsGameOver() { return gameOver; }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }


    public bool Check(GameObject[,] board)
    {
        return false;
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " Wins!";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;

    }
}
