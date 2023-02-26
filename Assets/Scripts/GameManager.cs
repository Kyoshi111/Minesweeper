using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        field = GetComponent<Field>();
        
        field.GenerateCells();

        Camera.main.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
        Camera.main.GetComponent<Camera>().orthographicSize = 10;

        DrawField();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

        var cell = field[(int)touchPosition.x, (int)touchPosition.y];
        
        if (!field.AreMinesGenerated)
            field.GenerateMinesExcluding3X3At(cell);

        cell.Revealed = true;
        
        DrawCell(cell);
        
        Debug.Log($"{cell.Position}\t{cell.Type}\t{cell.MinesAround}");
    }

    private void DrawField()
    {
        for (var x = 0; x < field.Width; x++)
        for (var y = 0; y < field.Height; y++)
        {
            var cell = field[x, y];
            DrawCell(cell);
        }
    }

    private void DrawCell(Cell cell)
    {
        tilemap.SetTile(cell.Position, cell.Revealed ? tileset.GetTile(cell) : tileset.Unknown);
    }
}
