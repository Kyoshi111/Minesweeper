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
        mainCamera.ScreenToWorldPoint(primaryPositionAction.ReadValue<Vector2>());
    
    public Vector3 SecondaryTouchWorldPoint => 
        mainCamera.ScreenToWorldPoint(secondaryPositionAction.ReadValue<Vector2>());

    private Camera mainCamera;
    private PlayerInput playerInput;
    private InputAction primaryPositionAction;
    private InputAction secondaryPositionAction;
    private InputAction primaryTapAction;
    private InputAction primarySlowTapAction;
    private InputAction primaryPressAction;
    private InputAction secondaryPressAction;

    public float GetDistanceBetweenTwoTouchPositions() =>
        Vector2.Distance(primaryPositionAction.ReadValue<Vector2>(),
            secondaryPositionAction.ReadValue<Vector2>());
    
    private void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        primaryPositionAction = playerInput.actions["PrimaryTouchPosition"];
        secondaryPositionAction = playerInput.actions["SecondaryTouchPosition"];
        primaryTapAction = playerInput.actions["PrimaryTouchTap"];
        primarySlowTapAction = playerInput.actions["PrimaryTouchSlowTap"];
        primaryPressAction = playerInput.actions["PrimaryTouchPress"];
        secondaryPressAction = playerInput.actions["SecondaryTouchPress"];
    }

    private void Start()
    {
        primaryTapAction.performed += PrimaryTap;
        primarySlowTapAction.performed += PrimarySlowTap;
        primaryPressAction.started += StartPrimaryTouch;
        primaryPressAction.canceled += EndPrimaryTouch;
        secondaryPressAction.started += StartSecondaryTouch;
        secondaryPressAction.canceled += EndSecondaryTouch;
        secondaryPressAction.started += StartZoom;
        secondaryPressAction.canceled += EndZoom;
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
