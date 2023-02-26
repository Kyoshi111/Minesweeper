using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileset;

    private void Start()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        tileset = FindObjectOfType<Tilemap>().GetComponent<TileSet>();
        field = GetComponent<Field>();
        
        field.GenerateMines();
        
        DrawField();
    }
    
    private void DrawField()
    {
        for (var x = 0; x < field.Width; x++)
        for (var y = 0; y < field.Height; y++)
            tilemap.SetTile(field[x, y].Position, tileset.GetTile(field[x, y]));
    }
}
