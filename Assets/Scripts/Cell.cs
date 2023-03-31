using UnityEngine;

public struct Cell
{
    public Cell(int x, int y)
    {
        Position = new Vector3Int(x, y, 0);
        IsExploded = false;
        IsFlagged = false;
        HasMine = false;
        MinesAround = 0;
        IsRevealed = false;
    }

    public Vector3Int Position { get; set; }
    public int MinesAround { get; set; }
    public bool IsFlagged { get; set; }
    public bool HasMine { get; set; }
    public bool IsExploded { get; set; }
    public bool IsRevealed { get; set; }
}
