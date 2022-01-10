using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject chesspiece;
    private AIPlayer AIBlack, AIWhite;
    private bool IsBlackAI = true;
    private bool IsWhiteAI = false; 
    private GameObject[,] positions = new GameObject[8, 8];
    private BoardState currentState;
    private List<BoardState> history = new List<BoardState>();
    private bool gameOver = false;
    



    // Start is called before the first frame update
    void Start()
    {

        // FOR TESTING ONLY WHITE IS ALWAYS A PLAYER

        if (IsBlackAI)
        {
            AIBlack = new AIPlayer("black");
        }
        if (IsWhiteAI)
        {
            AIWhite = new AIPlayer("white");
        }

        currentState = new BoardState();
        BoardStateToPieces(currentState);
    }

    public void Update()
    {
        if (!gameOver)
        {
            int timer = 2;
            if (AIBlack != null)
            {
                if (currentState.GetCurrentPlayer() == "black")
                {
                    if (!AIBlack.IsThinking)
                    {
                        AIBlack.IsThinking = true;
                        (int x, int y, int nx, int ny, bool attack) move = AIBlack.MakeMove(currentState);
                        HandleMovement(GetCPRef(move.x, move.y), move.nx, move.ny, move.attack, timer);
                    }
                }
            }

            if (AIWhite != null)
            {
                if (currentState.GetCurrentPlayer() == "white")
                {
                    if (!AIWhite.IsThinking)
                    {
                        AIWhite.IsThinking = true;
                        (int x, int y, int nx, int ny, bool attack) move = AIWhite.MakeMove(currentState);
                        HandleMovement(GetCPRef(move.x, move.y), move.nx, move.ny, move.attack, timer);
                    }
                }
            }
        }

        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    public bool IsPlayerAI(string player)
    {
        if (player == "white")
        {
            return IsWhiteAI;
        }
        return IsBlackAI;
        
    }

    public GameObject Create(string name, string player, int x, int y, bool IsAI = false)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Piece cp = obj.GetComponent<Piece>();
        cp.name = name;
        cp.IsAI = IsAI;
        cp.SetPlayer(player);
        cp.SetXYBoard(x,y);  
        cp.Activate();
        return obj;
        
    }

    public BoardState GetCurrentState() { return this.currentState; }
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


    public GameObject GetCPRef(int x, int y)
    {
        return positions[x, y];
    }


    private IEnumerator DelayMovements(GameObject obj, int x, int y, bool attack, int wait = 0)
    {
        yield return new WaitForSeconds(wait);
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

        if (AIBlack != null)
        {
            AIBlack.IsThinking = false;
        }
        if (AIWhite != null)
        {
            AIWhite.IsThinking = false;
        }
    }


    public void HandleMovement(GameObject obj, int x, int y, bool attack, int wait = 0)
    {
        Debug.Log("Handling movements!");
        StartCoroutine(DelayMovements(obj, x, y, attack, wait));
    }


    public string GetCurrentPlayer() { return currentState.GetCurrentPlayer(); }

    public bool IsGameOver() { return gameOver; }

    public void NextTurn()
    {
        currentState.SwapPlayer();
    }


    public void Winner(string playerWinner)
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " Wins!";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;

    }

}
