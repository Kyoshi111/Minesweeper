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
    private Cell[,] _field { get; set; }
    
    private void Start()
    {
        InitField();
    }

    private void InitField()
    {
        _field = new Cell[FieldWidth, FieldHeight];
        for (var x = 0; x < _field.GetLength(0); x++)
        {
            for (var y = 0; y < _field.GetLength(1); y++)
            {
                _field[x, y] = new Cell { Position = new Vector2(x, y) };
            }
        }
    }

    private void GenerateField()
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
}
