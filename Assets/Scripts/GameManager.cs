using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public int FieldWidth { get; private set; }
    [field: SerializeField] public int FieldHeight { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    private Cell[,] _field;
    
    private void Start()
    {
        GenerateCells();
    }

    private void GenerateCells()
    {
        _field = new Cell[FieldWidth, FieldHeight];
        for (var x = 0; x < _field.GetLength(0); x++)
        {
            for (var y = 0; y < _field.GetLength(1); y++)
            {
                _field[x, y] =
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

        while (count != MinesCount)
        {
            var x = Random.Range(0, _field.GetLength(0));
            var y = Random.Range(0, _field.GetLength(1));
            
            if (_field[x, y].State == Cell.Type.Mine)
                continue;

            _field[x, y].State = Cell.Type.Mine;
            count++;
        }
    }

    private void GenerateNumbers()
    {
        for (var x = 0; x < _field.GetLength(0); x++)
        {
            for (var y = 0; y < _field.GetLength(1); y++)
            {
                var minesCount = CountMines(x, y);

                if (minesCount <= 0) continue;
                
                _field[x, y].State = Cell.Type.Number;
                _field[x, y].MinesAround = minesCount;
            }
        }
    }

    private int CountMines(int x, int y)
    {
        var minesCount = 0;
        for (var i = -1; i <= 1; i++)
        {
            for (var j = -1; j <= 1; j++)
            {
                if ((i == 0 && j == 0) ||
                    x + i < 0 ||
                    x + i >= _field.GetLength(0) ||
                    y + j < 0 ||
                    y + j >= _field.GetLength(1))
                    continue;

                if (_field[x + i, y + j].State == Cell.Type.Mine) minesCount++;
            }
        }

        return minesCount;
    }
}
