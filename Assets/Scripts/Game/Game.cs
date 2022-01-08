using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8, 8];
    private BoardState currentState;
    private List<BoardState> history = new List<BoardState>();

    private bool gameOver = false;

    private void BoardStateToPieces(BoardState state)
    {
        for (int x = 0; x < state.GetLength(0); x++)
        {
            for (int y = 0; y < state.GetLength(1); y++)
            {
                if (state.GetPosition(x, y) == null)
                {
                    continue;
                }
                string n = state.GetPosition(x, y);
                string color = state.GetColor(x, y);
                positions[x, y] = Create(n, color, x, y);
            }
        }
    }

    public BoardState GetCurrentState() { return this.currentState; }

    private void SetBoard(GameObject[] white, GameObject[] black)
    {
        // Set a board layout
    }

    private void StoreBoard(GameObject[] white, GameObject[] black)
    {
        // save a layout to a variable
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = new BoardState();

        BoardStateToPieces(currentState);
    }


    public GameObject Create(string name, string player, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Piece cp = obj.GetComponent<Piece>();
        cp.name = name;
        cp.SetPlayer(player);
        cp.SetXYBoard(x,y);  
        cp.Activate();
        return obj;
        
    }

    public GameObject GetCPRef(int x, int y)
    {
        return positions[x, y];
    }

    public void HandleMovement(GameObject obj, int x, int y, bool attack)
    {
        if (attack)
        {
            Destroy(positions[x, y]);
        }

        Piece piece = obj.GetComponent<Piece>();
        int prevX = piece.GetXBoard();
        int prevY = piece.GetYBoard();

        //Do board state
        currentState.SetPiece(piece.GetName(), piece.GetPlayer(), x, y, prevX, prevY);

        // Do visuals

        positions[prevX, prevY] = null;
        piece.SetXYBoard(x, y);
        piece.IncNumMoves();
        positions[x, y] = obj;
        NextTurn();
        piece.DestroyMovePlates();


    }


    public string GetCurrentPlayer() { return currentState.GetCurrentPlayer(); }

    public bool IsGameOver() { return gameOver; }

    public void NextTurn()
    {
        currentState.SwapPlayer();
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

    internal bool PositionOnBoard(int possX, int possY)
    {
        throw new NotImplementedException();
    }
}
