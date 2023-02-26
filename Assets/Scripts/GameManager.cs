using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Debug = System.Diagnostics.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileset;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        field = GetComponent<Field>();
        
        field.GenerateMines();

        mainCamera.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
        mainCamera.GetComponent<Camera>().orthographicSize = 10;

        DrawField();
    }
    
    private void DrawField()
    {
        for (var x = 0; x < field.Width; x++)
        for (var y = 0; y < field.Height; y++)
            tilemap.SetTile(field[x, y].Position, tileset.GetTile(field[x, y]));
    }
}
