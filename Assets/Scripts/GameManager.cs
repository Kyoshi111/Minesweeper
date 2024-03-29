using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Field field;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileSet tileset;
    [SerializeField] private TextMeshProUGUI flagsMinesCountTextMeshPro;
    [SerializeField] private TextMeshProUGUI startTitle;
    [SerializeField] private TextMeshProUGUI zoomSensitivityTitle;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private float zoomMin;
    [SerializeField] private float zoomMax;
    [SerializeField] private float zoomSensitivity;
    private TouchManager _touchManager;
    private Camera _mainCamera;
    private Vector3 _startPrimaryTouchWorldPoint;
    private bool _isPrimaryTouching;
    private bool _isSecondaryTouching;
    private bool _isFirstTouch = true;
    private Coroutine _zoomCoroutine;

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void SetZoomSensitivity(float sensitivity)
    {
        zoomSensitivity = sensitivity;
        zoomSensitivityTitle.text = zoomSensitivity.ToString(CultureInfo.InvariantCulture);
    }
    private void Awake()
    {
        tilemap = FindObjectOfType<Tilemap>().GetComponent<Tilemap>();
        field = GetComponent<Field>();
        field.TrySetParams(
            PlayerPrefs.GetInt("Width", 9),
            PlayerPrefs.GetInt("Height", 16),
            PlayerPrefs.GetInt("MinesCount", 20)
        );
        _touchManager = TouchManager.Instance;
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _touchManager.OnPrimaryTap += PrimaryTap;
        _touchManager.OnPrimarySlowTap += PrimarySlowTap;
        _touchManager.OnStartPrimaryTouch += StartPrimaryTouch;
        _touchManager.OnEndPrimaryTouch += EndPrimaryTouch;
        _touchManager.OnStartSecondaryTouch += StartSecondaryTouch;
        _touchManager.OnEndSecondaryTouch += EndSecondaryTouch;
        _touchManager.OnZoomStart += ZoomStart;
        _touchManager.OnZoomEnd += ZoomEnd;
    }

    private void OnDisable()
    {
        _touchManager.OnPrimaryTap -= PrimaryTap;
        _touchManager.OnPrimarySlowTap -= PrimarySlowTap;
        _touchManager.OnStartPrimaryTouch -= StartPrimaryTouch;
        _touchManager.OnEndPrimaryTouch -= EndPrimaryTouch;
        _touchManager.OnStartSecondaryTouch -= StartSecondaryTouch;
        _touchManager.OnEndSecondaryTouch -= EndSecondaryTouch;
        _touchManager.OnZoomStart -= ZoomStart;
        _touchManager.OnZoomEnd -= ZoomEnd;
    }

    private void Start()
    {
        field.StartGame();

        SetCameraOnCenter();
        
        DrawField();

        gameOverMenu.SetActive(false);
        winMenu.SetActive(false);
        startTitle.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_isPrimaryTouching && !_isSecondaryTouching)
        {
            var position = _mainCamera.transform.position;
            var targetPosition = position + (_startPrimaryTouchWorldPoint - _touchManager.PrimaryTouchWorldPoint);

            position = new Vector3(
                Mathf.Clamp(targetPosition.x, 0, field.Width),
                Mathf.Clamp(targetPosition.y, 0, field.Height),
                targetPosition.z);
            
            _mainCamera.transform.position = position;
        }

        switch (field.GameState)
        {
            case GameState.Over:
                gameOverMenu.SetActive(true);
                break;
            case GameState.Win:
                winMenu.SetActive(true);
                break;
        }
    }
    
    private void PrimaryTap(Vector3 worldPoint)
    {
        if (_isFirstTouch)
        {
            _isFirstTouch = false;
            startTitle.gameObject.SetActive(false);
            PrimarySlowTap(worldPoint);
            return;
        }
        
        if (worldPoint != _startPrimaryTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);
        
        if (field.IsRevealed(cellPosition.x, cellPosition.y))
            field.RevealAroundNumber(cellPosition.x, cellPosition.y);
        
        else field.FlagOrUnflag(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void PrimarySlowTap(Vector3 worldPoint)
    {
        if (_isFirstTouch) return;
        
        if (worldPoint != _startPrimaryTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);

        field.Reveal(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void StartPrimaryTouch(Vector3 worldPoint)
    {
        _isPrimaryTouching = true;
        _startPrimaryTouchWorldPoint = worldPoint;
    }
    
    private void EndPrimaryTouch(Vector3 worldPoint)
    {
        _isPrimaryTouching = false;
    }

    private void StartSecondaryTouch(Vector3 worldPoint)
    {
        _isSecondaryTouching = true;
    }
    
    private void EndSecondaryTouch(Vector3 worldPoint)
    {
        _isSecondaryTouching = false;
    }

    private void ZoomStart()
    {
        _zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    
    private void ZoomEnd()
    {
        StopCoroutine(_zoomCoroutine);
    }

    private IEnumerator ZoomDetection()
    {
        var previousDistance = _touchManager.GetDistanceBetweenTwoTouchPositions();
        
        while (true)
        {
            var distance = _touchManager.GetDistanceBetweenTwoTouchPositions();
            var difference = distance - previousDistance;
            
            _mainCamera.orthographicSize = Mathf.Clamp(_mainCamera.orthographicSize - difference * zoomSensitivity,
                zoomMin, zoomMax);

            previousDistance = distance;

            yield return null;
        }
    }

    private void SetCameraOnCenter()
    {
        _mainCamera.transform.position = new Vector3((float)field.Width / 2, (float)field.Height / 2, -10);
        _mainCamera.GetComponent<Camera>().orthographicSize = 10;
    }

    private Vector3Int WorldPointToCellPosition(Vector2 worldPoint)
    {
        return new Vector3Int((int)worldPoint.x, (int)worldPoint.y, (int)_mainCamera.nearClipPlane);
    }

    private void DrawField()
    {
        for (var x = 0; x < field.Width; x++)
        for (var y = 0; y < field.Height; y++)
        {
            tilemap.SetTile(new Vector3Int(x, y, 0), tileset.GetTile(field, x, y));
        }

        flagsMinesCountTextMeshPro.text = $"{field.FlagsCount}/{field.MinesCount}";
    }
}
