using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _attack;
    [SerializeField] private GameObject _moveable;


    private int x;
    private int y;

    [SerializeField] public GameObject controller;
    GameObject reference;

    public void SetCoords(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Init(bool isOffset)
    {
        this.GetComponent<SpriteRenderer>().color = isOffset ? _offsetColor : _baseColor;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (_moveable.activeSelf || _attack.activeSelf)
        {
            controller.GetComponent<Game>().HandleMovement(reference, x, y, _attack.activeSelf);
        }

        // Setup Movement
        DestroyMoveMarks();
        //Check if there's a piece
        BoardState state = controller.GetComponent<Game>().GetCurrentState();
        string curPlayer = controller.GetComponent<Game>().GetCurrentPlayer();
        if (!controller.GetComponent<Game>().IsPlayerAI(curPlayer))
        {
            GameObject objRef = controller.GetComponent<Game>().GetCPRef(x, y);
            if (objRef != null)
            {
                Piece p = objRef.GetComponent<Piece>();
                if (curPlayer == p.GetPlayer())
                {
                    GridManager chessboard = GameObject.FindGameObjectWithTag("GridManager").GetComponent<GridManager>();
                    Movements moves = new Movements();
                    List<(int x, int y, bool attack)> possMoves = moves.GenerateMovements(p.name, x, y, p.GetPlayer(), state, p.GetNumMoves(), true);

                    foreach (var m in possMoves)
                    {
                        int mx = m.x;
                        int my = m.y;
                        bool attack = m.attack;
                        chessboard.GetTileAtPosition(new Vector2(mx, my)).TurnOnMarkers(attack, objRef);
                    }


                }
            }

        }
    }

    public void DestroyMoveMarks()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i].GetComponent<Tile>().TurnOffMarkers();
        }
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    public void TurnOnMarkers(bool attack, GameObject refPiece)
    {
        reference = refPiece;
        if (attack)
        {
            _attack.SetActive(true);
        }
        else
        {
            _moveable.SetActive(true);
        }
    }

    public void TurnOffMarkers()
    {
        reference = null ;
        _attack.SetActive(false);
        _moveable.SetActive(false);
    }
}