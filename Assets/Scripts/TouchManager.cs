using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class TouchManager : Singleton<TouchManager>
{
    #region Events

    public delegate void Tap(Vector3 position);
    public event Tap OnPrimaryTap;
    public delegate void SlowTap(Vector3 position);
    public event SlowTap OnPrimarySlowTap;
    public delegate void StartTouch(Vector3 position);
    public event StartTouch OnStartPrimaryTouch;
    public event StartTouch OnStartSecondaryTouch;
    public delegate void EndTouch(Vector3 position);
    public event EndTouch OnEndPrimaryTouch;
    public event EndTouch OnEndSecondaryTouch;
    public delegate void ZoomStart();
    public event ZoomStart OnZoomStart;
    public delegate void ZoomEnd();
    public event ZoomEnd OnZoomEnd;

    #endregion
    
    public Vector3 PrimaryTouchWorldPoint => 
        _mainCamera.ScreenToWorldPoint(_primaryPositionAction.ReadValue<Vector2>());
    
    public Vector3 SecondaryTouchWorldPoint => 
        _mainCamera.ScreenToWorldPoint(_secondaryPositionAction.ReadValue<Vector2>());

    private Camera _mainCamera;
    private PlayerInput _playerInput;
    private InputAction _primaryPositionAction;
    private InputAction _secondaryPositionAction;
    private InputAction _primaryTapAction;
    private InputAction _primarySlowTapAction;
    private InputAction _primaryPressAction;
    private InputAction _secondaryPressAction;

    public float GetDistanceBetweenTwoTouchPositions() =>
        Vector2.Distance(_primaryPositionAction.ReadValue<Vector2>(),
            _secondaryPositionAction.ReadValue<Vector2>());
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerInput = GetComponent<PlayerInput>();
        _primaryPositionAction = _playerInput.actions["PrimaryTouchPosition"];
        _secondaryPositionAction = _playerInput.actions["SecondaryTouchPosition"];
        _primaryTapAction = _playerInput.actions["PrimaryTouchTap"];
        _primarySlowTapAction = _playerInput.actions["PrimaryTouchSlowTap"];
        _primaryPressAction = _playerInput.actions["PrimaryTouchPress"];
        _secondaryPressAction = _playerInput.actions["SecondaryTouchPress"];
    }

    private void Start()
    {
        _primaryTapAction.performed += PrimaryTap;
        _primarySlowTapAction.performed += PrimarySlowTap;
        _primaryPressAction.started += StartPrimaryTouch;
        _primaryPressAction.canceled += EndPrimaryTouch;
        _secondaryPressAction.started += StartSecondaryTouch;
        _secondaryPressAction.canceled += EndSecondaryTouch;
        _secondaryPressAction.started += StartZoom;
        _secondaryPressAction.canceled += EndZoom;
    }

    private void PrimaryTap(InputAction.CallbackContext context)
    {
        OnPrimaryTap?.Invoke(PrimaryTouchWorldPoint);
    }
    
    private void PrimarySlowTap(InputAction.CallbackContext context)
    {
        OnPrimarySlowTap?.Invoke(PrimaryTouchWorldPoint);
    }

    private void StartPrimaryTouch(InputAction.CallbackContext context)
    {
        OnStartPrimaryTouch?.Invoke(PrimaryTouchWorldPoint);
    }
    
    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        OnEndPrimaryTouch?.Invoke(PrimaryTouchWorldPoint);
    }
    
    private void StartSecondaryTouch(InputAction.CallbackContext context)
    {
        OnStartSecondaryTouch?.Invoke(SecondaryTouchWorldPoint);
    }

    private void EndSecondaryTouch(InputAction.CallbackContext context)
    {
        OnEndSecondaryTouch?.Invoke(SecondaryTouchWorldPoint);
    }

    private void StartZoom(InputAction.CallbackContext context)
    {
        OnZoomStart?.Invoke();
    }
    
    private void EndZoom(InputAction.CallbackContext context)
    {
        OnZoomEnd?.Invoke();
    }
}
