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
    private TouchManager touchManager;
    private Camera mainCamera;
    private Vector3 startPrimaryTouchWorldPoint;
    private Vector3 startSecondaryTouchWorldPoint;
    private bool isPrimaryTouching;
    private bool isSecondaryTouching;
    private bool isFirstTouch = true;
    private Coroutine zoomCoroutine;

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
        touchManager = TouchManager.Instance;
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        touchManager.OnPrimaryTap += PrimaryTap;
        touchManager.OnPrimarySlowTap += PrimarySlowTap;
        touchManager.OnStartPrimaryTouch += StartPrimaryTouch;
        touchManager.OnEndPrimaryTouch += EndPrimaryTouch;
        touchManager.OnStartSecondaryTouch += StartSecondaryTouch;
        touchManager.OnEndSecondaryTouch += EndSecondaryTouch;
        touchManager.OnZoomStart += ZoomStart;
        touchManager.OnZoomEnd += ZoomEnd;
    }

    private void OnDisable()
    {
        touchManager.OnPrimaryTap -= PrimaryTap;
        touchManager.OnPrimarySlowTap -= PrimarySlowTap;
        touchManager.OnStartPrimaryTouch -= StartPrimaryTouch;
        touchManager.OnEndPrimaryTouch -= EndPrimaryTouch;
        touchManager.OnStartSecondaryTouch -= StartSecondaryTouch;
        touchManager.OnEndSecondaryTouch -= EndSecondaryTouch;
        touchManager.OnZoomStart -= ZoomStart;
        touchManager.OnZoomEnd -= ZoomEnd;
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
        if (isPrimaryTouching && !isSecondaryTouching)
        {
            var cameraHeight = mainCamera.orthographicSize * 2;
            var cameraWidth = cameraHeight * mainCamera.aspect;
            var position = mainCamera.transform.position;
            var targetPosition = position + (startPrimaryTouchWorldPoint - touchManager.PrimaryTouchWorldPoint);
            
            var leftPivot = cameraWidth / 2;
            var rightPivot = field.Width - cameraWidth / 2;
            var topPivot = field.Height - cameraHeight / 2;
            var bottomPivot = cameraHeight / 2;

            position = new Vector3(
                Mathf.Clamp(targetPosition.x, leftPivot, rightPivot),
                Mathf.Clamp(targetPosition.y, bottomPivot, topPivot),
                targetPosition.z);
            
            mainCamera.transform.position = position;
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
        if (isFirstTouch)
        {
            isFirstTouch = false;
            startTitle.gameObject.SetActive(false);
            PrimarySlowTap(worldPoint);
            return;
        }
        
        if (worldPoint != startPrimaryTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);
        
        if (field.IsRevealed(cellPosition.x, cellPosition.y))
            field.RevealAroundNumber(cellPosition.x, cellPosition.y);
        
        else field.FlagOrUnflag(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void PrimarySlowTap(Vector3 worldPoint)
    {
        if (isFirstTouch) return;
        
        if (worldPoint != startPrimaryTouchWorldPoint) return;
        
        var cellPosition = WorldPointToCellPosition(worldPoint);

        field.Reveal(cellPosition.x, cellPosition.y);
        
        DrawField();
    }

    private void StartPrimaryTouch(Vector3 worldPoint)
    {
        isPrimaryTouching = true;
        startPrimaryTouchWorldPoint = worldPoint;
    }
    
    private void EndPrimaryTouch(Vector3 worldPoint)
    {
        isPrimaryTouching = false;
    }

    private void StartSecondaryTouch(Vector3 worldPoint)
    {
        isSecondaryTouching = true;
        startSecondaryTouchWorldPoint = worldPoint;
    }
    
    private void EndSecondaryTouch(Vector3 worldPoint)
    {
        isSecondaryTouching = false;
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    
    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    private IEnumerator ZoomDetection()
    {
        var previousDistance = touchManager.GetDistanceBetweenTwoTouchPositions();
        
        while (true)
        {
            var distance = touchManager.GetDistanceBetweenTwoTouchPositions();
            var difference = distance - previousDistance;

            //mainCamera.transform.position =
            //    (touchManager.PrimaryTouchWorldPoint + touchManager.SecondaryTouchWorldPoint) / 2;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - difference * zoomSensitivity,
                zoomMin, zoomMax);

            previousDistance = distance;

            yield return null;
        }
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

        flagsMinesCountTextMeshPro.text = $"{field.FlagsCount}/{field.MinesCount}";
    }
}
