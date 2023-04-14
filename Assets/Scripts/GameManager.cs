using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Field field;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileset;
    private TouchManager touchManager;
    private Camera mainCamera;
    private Vector3 startTouchWorldPoint;
    private Vector3 endTouchWorldPoint;
    private bool isTouching;

    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        field = GetComponent<Field>();
        touchManager = TouchManager.Instance;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchManager.OnTap += Tap;
        touchManager.OnSlowTap += SlowTap;
        touchManager.OnStartTouch += StartTouch;
        touchManager.OnEndTouch += EndTouch;
    }

    private void OnDisable()
    {
        touchManager.OnTap -= Tap;
        touchManager.OnSlowTap -= SlowTap;
        touchManager.OnStartTouch -= StartTouch;
        touchManager.OnEndTouch -= EndTouch;
    }

    private void Start()
    {
        field.StartGame();

        SetCameraOnCenter();
        
        DrawField();
    }

    private void Update()
    {
        if (isTouching)
        {
            mainCamera.transform.position += startTouchWorldPoint - touchManager.TouchWorldPoint;
        }
    }
    
    private void Tap(Vector3 worldPoint)
    {
        if (worldPoint != startTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);
        
        if (field.IsRevealed(cellPosition.x, cellPosition.y))
            field.RevealAroundNumber(cellPosition.x, cellPosition.y);
        
        else field.FlagOrUnflag(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void SlowTap(Vector3 worldPoint)
    {
        if (worldPoint != startTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);

        field.Reveal(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void StartTouch(Vector3 worldPoint, float time)
    {
        isTouching = true;
        startTouchWorldPoint = worldPoint;
    }
    
    private void EndTouch(Vector3 worldPoint, float time)
    {
        isTouching = false;
        endTouchWorldPoint = worldPoint;

        Debug.DrawLine(startTouchWorldPoint, endTouchWorldPoint, Color.red, 10.0f);
    }

    private void SetCameraOnCenter()
    {
        mainCamera.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
        mainCamera.GetComponent<Camera>().orthographicSize = 10;
    }

    private Vector3Int WorldPointToCellPosition(Vector2 worldPoint)
    {
        return new Vector3Int((int)worldPoint.x, (int)worldPoint.y, (int)mainCamera.nearClipPlane);
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
