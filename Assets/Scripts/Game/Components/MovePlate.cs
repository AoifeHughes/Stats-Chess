using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{

    [SerializeField] public GameObject controller;

    GameObject reference = null;

    int matX, matY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            // check to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matX, matY);

            Destroy(cp);
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Piece>().GetXBoard(),
            reference.GetComponent<Piece>().GetYBoard());

        reference.GetComponent<Piece>().SetXBoard(matX);
        reference.GetComponent<Piece>().SetYBoard(matY);
        reference.GetComponent<Piece>().SetCoords();
        reference.GetComponent<Piece>().IncNumMoves();
        controller.GetComponent<Game>().SetPosition(reference);
        controller.GetComponent<Game>().NextTurn();
        reference.GetComponent<Piece>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {  
        matX = x;
        matY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
