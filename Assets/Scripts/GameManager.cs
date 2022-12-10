using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public int FieldWidth { get; private set; }
    [field: SerializeField] public int FieldHeight { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileSet;

    private Field field;
    
    private void Start()
    {
        field = new Field(FieldWidth, FieldHeight, MinesCount);
    }

    private void Update()
    {
        if (field.Width != FieldWidth ||
            field.Height != FieldHeight ||
            field.MinesCount != MinesCount)
            field = new Field(FieldWidth, FieldHeight, MinesCount);
    }
}
