using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    [field: SerializeField] public bool AreMinesGenerated { get; private set; }
    private Cell[,] cells;

    public Cell this[int x, int y]
    {
        get => cells[x, y];
        set => cells[x, y] = value;
    }
    
    public void GenerateCells()
    {
        cells = new Cell[Width, Height];
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            cells[x, y] = new Cell(x, y);
        }

        AreMinesGenerated = false;
    }

    public void GenerateMinesExcluding3X3At(Cell cell)
    {
        var count = 0;

        while (count != MinesCount)
        {
            var x = Random.Range(0, Width);
            var y = Random.Range(0, Height);
            
            if (cells[x, y].Type == CellType.Mine ||
                Math.Abs(x - cell.Position.x) <= 1 ||
                Math.Abs(y - cell.Position.y) <= 1)
                continue;

            cells[x, y].Type = CellType.Mine;
            count++;
            
            IncreaseNumbersAround(x, y);
        }

        AreMinesGenerated = true;
    }
    
    private void IncreaseNumbersAround(int x, int y)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i == 0 && j == 0) ||
                !AreValidCoordinates(x + i, y + j) ||
                cells[x + i, y + j].Type == CellType.Mine)
                continue;

            if (cells[x + i, y + j].Type == CellType.Empty)
                cells[x + i, y + j].Type = CellType.Number;

            cells[x + i, y + j].MinesAround++;
        }
    }

    private bool AreValidCoordinates(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
}
