using UnityEngine;

public struct Cell
{
    public enum CellType
    {
        Unknown,
        Empty,
        Mine,
        Number,
    }
    
    public Vector3Int Position { get; set; }
    public CellType Type { get; set; }
    public int MinesAround { get; set; }
    public bool Flagged { get; set; }
    public bool Exploded { get; set; }
    public bool Revealed { get; set; }
}
