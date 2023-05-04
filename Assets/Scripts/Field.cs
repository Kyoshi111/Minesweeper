using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }
    public int FlagsCount { get; private set; }
    public GameState gameState = GameState.NotStarted;
    private bool _areMinesGenerated;
    private Cell[,] _cells;

    public int MinesAround(int cellX, int cellY) => _cells[cellX, cellY].MinesAround;
    public bool IsFlagged(int cellX, int cellY) => _cells[cellX, cellY].IsFlagged;
    public bool IsExploded(int cellX, int cellY) => _cells[cellX, cellY].IsExploded;
    public bool IsRevealed(int cellX, int cellY) => _cells[cellX, cellY].IsRevealed;
    public bool HasMine(int cellX, int cellY) => _cells[cellX, cellY].HasMine;

    public void StartGame()
    {
        GenerateCells();
        gameState = GameState.Continues;
        _areMinesGenerated = false;
    }

    public bool TrySetParams(int width, int height, int minesCount)
    {
        if (gameState == GameState.Continues) return false;

        Width = width;
        Height = height;
        MinesCount = minesCount;

        return true;
    }

    public void Reveal(int cellX, int cellY)
    {
        if (!AreValidCoordinates(cellX, cellY) ||
            _cells[cellX, cellY].IsFlagged)
            return;
        
        if (!_areMinesGenerated) GenerateMinesExcluding3X3At(cellX, cellY);

        _cells[cellX, cellY].IsRevealed = true;

        if (_cells[cellX, cellY].HasMine) Explode(cellX, cellY);
        
        else if (_cells[cellX, cellY].MinesAround == 0) RevealAround(cellX, cellY);
    }

    public void RevealAroundNumber(int cellX, int cellY)
    {
        if (_cells[cellX, cellY].IsRevealed && _cells[cellX, cellY].MinesAround <= FlagsAround(cellX, cellY))
        {
            RevealAround(cellX, cellY);
        }
    }
    
    public void FlagOrUnflag(int cellX, int cellY)
    {
        if (!_areMinesGenerated ||
            !AreValidCoordinates(cellX, cellY) ||
            _cells[cellX, cellY].IsRevealed ||
            _cells[cellX, cellY].IsExploded)
            return;

        if (_cells[cellX, cellY].IsFlagged)
        {
            _cells[cellX, cellY].IsFlagged = false;
            FlagsCount--;
        }
        else
        {
            _cells[cellX, cellY].IsFlagged = true;
            FlagsCount++;
        }
        
        if (MinesCount == FlagsCount) CheckWin();
    }
    
    private void GenerateCells()
    {
        _cells = new Cell[Width, Height];
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            _cells[x, y] = new Cell(x, y);
        }

        _areMinesGenerated = false;
    }

    private int FlagsAround(int cellX, int cellY)
    {
        var flagsAround = 0;
        
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i, j) == (0, 0) || !AreValidCoordinates(cellX + i, cellY + j)) continue;

            if (_cells[cellX + i, cellY + j].IsFlagged) flagsAround++;
        }

        return flagsAround;
    }

    private void RevealAround(int cellX, int cellY)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if (!AreValidCoordinates(cellX + i, cellY + j) ||
                _cells[cellX + i, cellY + j].IsRevealed)
                continue;
            
            Reveal(cellX + i, cellY + j);
        }
    }

    private void CheckWin()
    {
        if (MinesCount != FlagsCount ||
            gameState != GameState.Continues ||
            _cells
                .Cast<Cell>()
                .Any(cell => cell is { HasMine: true, IsFlagged: false })) return;

        gameState = GameState.Win;
    }

    private void Explode(int cellX, int cellY)
    {
        if (!AreValidCoordinates(cellX, cellY)) return;

        _cells[cellX, cellY].IsExploded = true;
        
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            if (!_cells[x, y].HasMine) continue;
            if (_cells[x, y].IsFlagged) _cells[x, y].IsFlagged = false;
            
            _cells[x, y].IsRevealed = true;
        }

        gameState = GameState.Over;
    }

    private void GenerateMinesExcluding3X3At(int cellX, int cellY)
    {
        var count = 0;

        while (count != MinesCount)
        {
            var x = Random.Range(0, Width);
            var y = Random.Range(0, Height);
            
            if (_cells[x, y].HasMine ||
                (Math.Abs(x - cellX) <= 1 && Math.Abs(y - cellY) <= 1))
                continue;

            _cells[x, y].HasMine = true;
            count++;
            
            IncreaseNumbersAround(x, y);
        }

        _areMinesGenerated = true;
    }
    
    private void IncreaseNumbersAround(int cellX, int cellY)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
        {
            if ((i == 0 && j == 0) ||
                !AreValidCoordinates(cellX + i, cellY + j) ||
                _cells[cellX + i, cellY + j].HasMine)
                continue;

            _cells[cellX + i, cellY + j].MinesAround++;
        }
    }

    private bool AreValidCoordinates(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
}
