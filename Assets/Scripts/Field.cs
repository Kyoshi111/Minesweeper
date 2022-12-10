using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    private Cell[,] cells;
    private Tilemap tilemap;
    private TileSet tileSet;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tileSet = GetComponent<TileSet>();
        
        GenerateAll();
        
        Draw();
    }

    private void Draw()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            tilemap.SetTile(cells[x, y].Position, tileSet.GetTile(cells[x, y]));
    }

    private void GenerateAll()
    {
        GenerateCells();
        GenerateMines();
        GenerateNumbers();
    }

    private void GenerateCells()
    {
        cells = new Cell[Width, Height];
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            cells[x, y] = new Cell
                {
                    Position = new Vector3Int(x, y, 0),
                    Exploded = false,
                    Flagged = false,
                    MinesAround = 0,
                    Revealed = false,
                    Type = Cell.CellType.Empty
                };
        }
    }
    
    private void GenerateMines()
    {
        var count = 0;

        while (count != MinesCount)
        {
            var x = Random.Range(0, cells.GetLength(0));
            var y = Random.Range(0, cells.GetLength(1));
            
            if (cells[x, y].Type == Cell.CellType.Mine)
                continue;

            cells[x, y].Type = Cell.CellType.Mine;
            count++;
        }
    }

    private void GenerateNumbers()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var minesAround = CountMines(x, y);

            if (minesAround <= 0) continue;
            
            cells[x, y].Type = Cell.CellType.Number;
            cells[x, y].MinesAround = minesAround;
        }
    }
    
    private int CountMines(int x, int y)
    {
        var minesAround = 0;
        
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i == 0 && j == 0) ||
                x + i < 0 ||
                x + i >= Width ||
                y + j < 0 ||
                y + j >= cells.GetLength(1))
                continue;

            if (cells[x + i, y + j].Type == Cell.CellType.Mine) minesAround++;
        }

        return minesAround;
    }
    
    public Cell this[int x, int y]
    {
        get => cells[x, y];
        set => cells[x, y] = value;
    }
}
