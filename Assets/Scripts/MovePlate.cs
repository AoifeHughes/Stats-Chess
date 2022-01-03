using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{

    public GameObject controller;

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

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessPeople>().GetXBoard(),
            reference.GetComponent<ChessPeople>().GetYBoard());

        reference.GetComponent<ChessPeople>().SetXBoard(matX);
        reference.GetComponent<ChessPeople>().SetYBoard(matY);
        reference.GetComponent<ChessPeople>().SetCoords();


    }

}
