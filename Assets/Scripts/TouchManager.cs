using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[DefaultExecutionOrder(-1)]
public class TouchManager : Singleton<TouchManager>
{
    #region Events

    public delegate void Tap(Vector3 position);
    public event Tap OnTap;
    public delegate void SlowTap(Vector3 position);
    public event SlowTap OnSlowTap;
    public delegate void StartTouch(Vector3 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector3 position, float time);
    public event EndTouch OnEndTouch;

    #endregion
    
    public Vector3 TouchWorldPoint => mainCamera.ScreenToWorldPoint(positionAction.ReadValue<Vector2>());

    private Camera mainCamera;
    private PlayerInput playerInput;
    private InputAction positionAction;
    private InputAction tapAction;
    private InputAction slowTapAction;
    private InputAction pressAction;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        positionAction = playerInput.actions["TouchPosition"];
        tapAction = playerInput.actions["TouchTap"];
        slowTapAction = playerInput.actions["TouchSlowTap"];
        pressAction = playerInput.actions["TouchPress"];
    }

    private void Start()
    {
        tapAction.performed += TapPrimary;
        slowTapAction.performed += SlowTapPrimary;
        pressAction.started += StartPrimary;
        pressAction.canceled += EndPrimary;
    }

    private void TapPrimary(InputAction.CallbackContext context)
    {
        OnTap?.Invoke(TouchWorldPoint);
    }
    
    private void SlowTapPrimary(InputAction.CallbackContext context)
    {
        OnSlowTap?.Invoke(TouchWorldPoint);
    }

    private void StartPrimary(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(TouchWorldPoint, (float)context.startTime);
    }
    
    private void EndPrimary(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(TouchWorldPoint, (float)context.time);
    }
}
