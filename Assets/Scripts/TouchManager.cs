using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : Singleton<TouchManager>
{
    public InputAction touchPositionAction;
    public InputAction touchTapAction;
    public InputAction touchSlowTapAction;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPositionAction = playerInput.actions["TouchPosition"];
        touchTapAction = playerInput.actions["TouchTap"];
        touchSlowTapAction = playerInput.actions["TouchSlowTap"];
    }
}
