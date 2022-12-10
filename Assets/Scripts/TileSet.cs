using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSet : MonoBehaviour
{
    [field: SerializeField] public Tile TileUnknown { get; }
    [field: SerializeField] public Tile TileEmpty { get; }
    [field: SerializeField] public Tile Tile1 { get; }
    [field: SerializeField] public Tile Tile2 { get; }
    [field: SerializeField] public Tile Tile3 { get; }
    [field: SerializeField] public Tile Tile4 { get; }
    [field: SerializeField] public Tile Tile5 { get; }
    [field: SerializeField] public Tile Tile6 { get; }
    [field: SerializeField] public Tile Tile7 { get; }
    [field: SerializeField] public Tile Tile8 { get; }
    [field: SerializeField] public Tile TileFlag { get; }
    [field: SerializeField] public Tile TileMine { get; }
    [field: SerializeField] public Tile TileExploded { get; }

    public Tile GetTileNumber(int number)
    {
        return number switch
        {
            1 => Tile1,
            2 => Tile2,
            3 => Tile3,
            4 => Tile4,
            5 => Tile5,
            6 => Tile6,
            7 => Tile7,
            8 => Tile8,
            _ => throw new ArgumentException()
        };
    }
}
