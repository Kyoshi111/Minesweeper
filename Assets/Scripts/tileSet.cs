using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileSet : MonoBehaviour
{
    [SerializeField] private Tile unknown;
    [SerializeField] private Tile empty;
    [SerializeField] private Tile number1;
    [SerializeField] private Tile number2;
    [SerializeField] private Tile number3;
    [SerializeField] private Tile number4;
    [SerializeField] private Tile number5;
    [SerializeField] private Tile number6;
    [SerializeField] private Tile number7;
    [SerializeField] private Tile number8;
    [SerializeField] private Tile flag;
    [SerializeField] private Tile mine;
    [SerializeField] private Tile exploded;

    public Tile GetTile(Cell cell)
    {
        if (cell.Flagged) return flag;
        if (cell.Exploded) return exploded;
        
        return cell.Type switch
        {
            Cell.CellType.Unknown => unknown,
            Cell.CellType.Empty => empty,
            Cell.CellType.Mine => mine,
            Cell.CellType.Number => GetTileNumber(cell.MinesAround),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Tile GetTileNumber(int number)
    {
        return number switch
        {
            1 => number1,
            2 => number2,
            3 => number3,
            4 => number4,
            5 => number5,
            6 => number6,
            7 => number7,
            8 => number8,
            _ => throw new ArgumentException()
        };
    }
}
