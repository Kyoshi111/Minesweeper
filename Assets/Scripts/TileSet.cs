using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileSet : ScriptableObject
{
    public Tile Unknown => unknown;
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

    public Tile GetTile(Field field, int cellX, int cellY)
    {
        if (field.IsFlagged(cellX, cellY)) return flag;
        if (!field.IsRevealed(cellX, cellY)) return unknown;
        if (field.IsExploded(cellX, cellY)) return exploded;
        if (field.HasMine(cellX, cellY)) return mine;
        return GetNumberTile(field.MinesAround(cellX, cellY));
    }

    private Tile GetNumberTile(int number)
    {
        return number switch
        {
            0 => empty,
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
