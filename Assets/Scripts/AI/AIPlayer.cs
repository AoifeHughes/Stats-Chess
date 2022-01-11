using System;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer
{
    private string color;
    private string AIType;
    public bool IsThinking = false;

    public AIPlayer(string color, string AIType = "random")
    {
        this.AIType = AIType;
        this.color = color;
    }

    public (int x, int y, int nx, int ny, bool attack) MakeMove(BoardState state)
    {

        int y;
        int x;
        int nx;
        int ny;
        bool attack;
        switch (AIType)
        {
            case "random":
                (x, y, nx, ny, attack) = MakeRandomMove(state);
                break;
            default:
                throw new InvalidOperationException("AI SPECIFIED NOT FOUND");
        }

        return (x, y, nx, ny, attack);
        
    }

    public (int x, int y, int nx, int ny, bool attack) MakeRandomMove(BoardState state)
    {
        Random rnd = new Random();
        List<(int, int, int, int, bool )> validMoves = new List<(int, int, int, int, bool)>();

        // Pick random piece of color and make legal move
        // first find all pieces first
        foreach (var p in state.IterateBoard()) 
        {
            if (p.color == color)
            {
                // get valid moves
                Movements moves = new Movements();
                foreach (var m in moves.GenerateMovements(p.piece, p.x, p.y, p.color, state, filter_suicide: true))
                {
                    int x = m.x;
                    int y = m.y;
                    bool attack = m.attack;

                    validMoves.Add((p.x, p.y, x, y, attack));
                }
            }
        }

        if (validMoves.Count > 0 )
        {
            int idx = rnd.Next(validMoves.Count);
            return validMoves[idx];
        }
        
        return (-1, -1, -1, -1, false);

    }

    public string GetColor()
    {
        return this.color;
    }

}
