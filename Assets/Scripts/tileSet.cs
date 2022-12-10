using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSet : MonoBehaviour
{
    [SerializeField] private Tile tileUnknown;
    [SerializeField] private Tile tileEmpty;
    [SerializeField] private Tile tile1;
    [SerializeField] private Tile tile2;
    [SerializeField] private Tile tile3;
    [SerializeField] private Tile tile4;
    [SerializeField] private Tile tile5;
    [SerializeField] private Tile tile6;
    [SerializeField] private Tile tile7;
    [SerializeField] private Tile tile8;
    [SerializeField] private Tile tileFlag;
    [SerializeField] private Tile tileMine;
    [SerializeField] private Tile tileExploded;

    public Tile GetTile(Cell cell)
    {
        if (cell.Flagged) return tileFlag;
        if (cell.Exploded) return tileExploded;
        
        return cell.Type switch
        {
            Cell.CellType.Unknown => tileUnknown,
            Cell.CellType.Empty => tileEmpty,
            Cell.CellType.Mine => tileMine,
            Cell.CellType.Number => GetTileNumber(cell.MinesAround),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private Tile GetTileNumber(int number)
    {
        return number switch
        {
            1 => tile1,
            2 => tile2,
            3 => tile3,
            4 => tile4,
            5 => tile5,
            6 => tile6,
            7 => tile7,
            8 => tile8,
            _ => throw new ArgumentException()
        };
    }
}
