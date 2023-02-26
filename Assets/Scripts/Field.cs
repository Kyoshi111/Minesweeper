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
    
    public Cell this[int x, int y]
    {
        get => cells[x, y];
        set => cells[x, y] = value;
    }

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
        //GenerateNumbers();
    }

    private void GenerateCells()
    {
        cells = new Cell[Width, Height];
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            cells[x, y] = new Cell(x, y);
        }
    }
    
    private void GenerateMines()
    {
        var count = 0;

        while (count != MinesCount)
        {
            var x = Random.Range(0, Width);
            var y = Random.Range(0, Height);
            
            if (cells[x, y].Type == Cell.CellType.Mine)
                continue;

            cells[x, y].Type = Cell.CellType.Mine;
            count++;
            
            IncreaseNumbersAround(x, y);
        }
    }

    private void IncreaseNumbersAround(int x, int y)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i == 0 && j == 0) ||
                !AreValidCoordinates(x + i, y + j) ||
                cells[x + i, y + j].Type == Cell.CellType.Mine)
                continue;

            if (cells[x + i, y + j].Type == Cell.CellType.Empty)
                cells[x + i, y + j].Type = Cell.CellType.Number;

            cells[x + i, y + j].MinesAround++;
        }
    }

    private void GenerateNumbers()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            var minesAround = CountMines(x, y);

            if (minesAround == 0) continue;
            
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
                !AreValidCoordinates(x + i, y + j) ||
                cells[x + i, y + j].Type != Cell.CellType.Mine)
                continue;

            minesAround++;
        }

        return minesAround;
    }

    private bool AreValidCoordinates(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
}
