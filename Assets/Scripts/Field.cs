using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Field
{
    private readonly int width;
    private readonly int height;
    private readonly int minesCount;
    private readonly Cell[,] cells;

    public Cell this[int x, int y]
    {
        get => cells[x, y];
        set => cells[x, y] = value;
    }

    public Field(int width, int height, int minesCount)
    {
        this.width = width;
        this.height = height;
        this.minesCount = minesCount;
        cells = new Cell[width, height];
        
        GenerateCells();
    }

    private void GenerateCells()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                cells[x, y] =
                    new Cell()
                    {
                        Position = new Vector2(x, y),
                        Exploded = false,
                        Flagged = false,
                        MinesAround = 0,
                        Revealed = false,
                        State = Cell.Type.Empty
                    };
            }
        }
    }
    
    private void GenerateMines()
    {
        var count = 0;

        while (count != minesCount)
        {
            var x = Random.Range(0, cells.GetLength(0));
            var y = Random.Range(0, cells.GetLength(1));
            
            if (cells[x, y].State == Cell.Type.Mine)
                continue;

            cells[x, y].State = Cell.Type.Mine;
            count++;
        }
    }

    private void GenerateNumbers()
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var minesAround = CountMines(x, y);

                if (minesAround <= 0) continue;
                
                cells[x, y].State = Cell.Type.Number;
                cells[x, y].MinesAround = minesAround;
            }
        }
    }
    
    private int CountMines(int x, int y)
    {
        var minesAround = 0;
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) ||
                    x + i < 0 ||
                    x + i >= width ||
                    y + j < 0 ||
                    y + j >= cells.GetLength(1))
                    continue;

                if (cells[x + i, y + j].State == Cell.Type.Mine) minesAround++;
            }
        }

        return minesAround;
    }
}
