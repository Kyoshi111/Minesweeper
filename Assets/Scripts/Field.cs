using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    private bool areMinesGenerated;
    private Cell[,] cells;

    public int MinesAround(int cellX, int cellY) => cells[cellX, cellY].MinesAround;
    public bool IsFlagged(int cellX, int cellY) => cells[cellX, cellY].IsFlagged;
    public bool IsExploded(int cellX, int cellY) => cells[cellX, cellY].IsExploded;
    public bool IsRevealed(int cellX, int cellY) => cells[cellX, cellY].IsRevealed;
    public bool HasMine(int cellX, int cellY) => cells[cellX, cellY].HasMine;

    public void StartGame()
    {
        GenerateCells();
    }

    private void GenerateCells()
    {
        cells = new Cell[Width, Height];
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            cells[x, y] = new Cell(x, y);
        }

        areMinesGenerated = false;
    }
    
    public void Reveal(int cellX, int cellY)
    {
        if (!AreValidCoordinates(cellX, cellY)) return;
        
        if (!areMinesGenerated) GenerateMinesExcluding3X3At(cellX, cellY);
        
        cells[cellX, cellY].IsRevealed = true;

        if (cells[cellX, cellY].HasMine) Explode(cellX, cellY);
        
        if (cells[cellX, cellY].MinesAround == 0) RevealEmptyCellsAround(cellX, cellY);
    }

    private void RevealEmptyCellsAround(int cellX, int cellY)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if (!AreValidCoordinates(cellX + i, cellY + j) ||
                cells[cellX + i, cellY + j].IsRevealed)
                continue;
            
            Reveal(cellX + i, cellY + j);
        }
    }
    
    public void FlagOrUnflag(int cellX, int cellY)
    {
        if (!areMinesGenerated ||
            !AreValidCoordinates(cellX, cellY) ||
            cells[cellX, cellY].IsRevealed ||
            cells[cellX, cellY].IsExploded)
            return;
        
        cells[cellX, cellY].IsFlagged = !cells[cellX, cellY].IsFlagged;
    }

    private void Explode(int cellX, int cellY)
    {
        if (!AreValidCoordinates(cellX, cellY)) return;

        cells[cellX, cellY].IsExploded = true;
        
        RevealAllField();
    }

    private void RevealAllField()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
            cells[x, y].IsRevealed = true;
    }

    private void GenerateMinesExcluding3X3At(int cellX, int cellY)
    {
        var count = 0;

        while (count != MinesCount)
        {
            var x = Random.Range(0, Width);
            var y = Random.Range(0, Height);
            
            if (cells[x, y].HasMine ||
                Math.Abs(x - cellX) <= 1 ||
                Math.Abs(y - cellY) <= 1)
                continue;

            cells[x, y].HasMine = true;
            count++;
            
            IncreaseNumbersAround(x, y);
        }

        areMinesGenerated = true;
    }
    
    private void IncreaseNumbersAround(int cellX, int cellY)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i == 0 && j == 0) ||
                !AreValidCoordinates(cellX + i, cellY + j) ||
                cells[cellX + i, cellY + j].HasMine)
                continue;

            cells[cellX + i, cellY + j].MinesAround++;
        }
    }

    private bool AreValidCoordinates(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
}
