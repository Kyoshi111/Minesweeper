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

    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        field = GetComponent<Field>();
        touchManager = TouchManager.Instance;
        mainCamera = Camera.main;
    }


    private void OnEnable()
    {
        touchManager.touchSlowTapAction.performed += Reveal;
        touchManager.touchTapAction.performed += FlagOrUnflag;
    }

    private void OnDisable()
    {
        touchManager.touchSlowTapAction.performed -= Reveal;
        touchManager.touchTapAction.performed -= FlagOrUnflag;
    }

    private void Start()
    {
        field.StartGame();

        SetCameraOnCenter();
    }

    private void Update()
    {
        DrawField();
    }

    private void Reveal(InputAction.CallbackContext context)
    {
        var cellPosition = ScreenPointToCellPosition(touchManager.touchPositionAction.ReadValue<Vector2>());
        field.Reveal(cellPosition.x, cellPosition.y);
    }

    private void FlagOrUnflag(InputAction.CallbackContext context)
    {
        var cellPosition = ScreenPointToCellPosition(touchManager.touchPositionAction.ReadValue<Vector2>());
        field.FlagOrUnflag(cellPosition.x, cellPosition.y);
    }

    private void SetCameraOnCenter()
    {
        mainCamera.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
        mainCamera.GetComponent<Camera>().orthographicSize = 10;
    }

    private Vector3Int ScreenPointToCellPosition(Vector2 screenPoint)
    {
        var worldPosition = mainCamera.ScreenToWorldPoint(screenPoint);
        return new Vector3Int((int)worldPosition.x, (int)worldPosition.y, (int)worldPosition.z);
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
