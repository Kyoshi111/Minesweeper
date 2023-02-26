using UnityEngine;

public struct Cell
{
    public Cell(int x, int y)
    {
        Position = new Vector3Int(x, y, 0);
        Exploded = false;
        Flagged = false;
        MinesAround = 0;
        Revealed = false;
        Type = Cell.CellType.Empty;
    }
    
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
