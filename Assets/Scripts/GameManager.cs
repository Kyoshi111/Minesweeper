using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Field field;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileset;

    private void Start()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        field = GetComponent<Field>();
        
        field.StartGame();

        if (Camera.main != null)
        {
            Camera.main.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
            Camera.main.GetComponent<Camera>().orthographicSize = 10;
        }

        DrawField();
    }

    private void Update()
    {
        if (Camera.main == null || Input.touchCount == 0)
            return;

        var touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        var cellX = (int)touchPosition.x;
        var cellY = (int)touchPosition.y;

        field.Reveal(cellX, cellY);
        
        //Debug.Log($"x = {cellX}, y = {cellY}, IsRevealed = {field.IsRevealed(cellX, cellY)}");

        DrawField();
    }

    private void DrawField()
    {
        for (var x = 0; x < field.Width; x++)
        for (var y = 0; y < field.Height; y++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTile(field, x, y));
        }
    }
}
