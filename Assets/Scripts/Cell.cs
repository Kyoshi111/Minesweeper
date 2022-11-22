using UnityEngine;

public struct Cell
{
    public Cell(Vector2 position, Type state = Type.Empty, int minesAround = 0, bool isFlagged = false)
    {
        Position = position;
        State = state;
        MinesAround = minesAround;
        IsFlagged = isFlagged;
    }
    
    public enum Type
    {
        Empty,
        Mine,
        Number
    }
    
    public Vector2 Position { get; private set; }
    public Type State { get; set; }
    public int MinesAround { get; set; }
    public bool IsFlagged { get; set; }
}
