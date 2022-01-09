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
    GameObject reference = null;

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
        // reimpliment

        if (_moveable.activeSelf || _attack.activeSelf)
        {
        controller = GameObject.FindGameObjectWithTag("GameController");
        controller.GetComponent<Game>().HandleMovement(reference, x, y, _attack.activeSelf);
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

    public void TurnOnMarkers(bool attack)
    {
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
        _attack.SetActive(false);
        _moveable.SetActive(false);
    }
}