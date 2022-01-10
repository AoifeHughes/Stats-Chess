using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] private static Vector2 aspectRatio;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {

        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Screen.width / Screen.height; // cameraHeight * aspect ratio

        float width_per_block = (cameraWidth < cameraHeight) ? cameraWidth / 8 : cameraHeight / 8;
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3((x * width_per_block) - (3.5f * width_per_block), (y * width_per_block) - 3.5f * width_per_block, 10), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                spawnedTile.SetCoords(x, y);
                spawnedTile.transform.localScale = new Vector3(width_per_block, width_per_block, width_per_block);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}