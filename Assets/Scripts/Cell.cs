using UnityEngine;

public struct Cell
{
    public enum Type
    {
        Empty,
        Mine,
        Number
    }
    
    public Vector2 Position { get; set; }
    public Type State { get; set; }
    public int MinesAround { get; set; }
    public bool Flagged { get; set; }
    public bool Exploded { get; set; }
    public bool Revealed { get; set; }
}
