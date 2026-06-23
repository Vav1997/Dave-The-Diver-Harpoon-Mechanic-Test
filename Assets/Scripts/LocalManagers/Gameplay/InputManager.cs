using System;
using UnityEngine;
using InputAction = UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public Action OnLeftMousePerformed;
    public Action OnRightMousePerformed;


    public bool IsLeftMousePressed { get; private set; }
    public bool IsRightMousePressed { get; private set; }
    public bool IsRightMouseHeld { get; private set; }
    public bool IsRightMouseReleased { get; private set; }


    private PlayerInput _playerInput;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }

        _playerInput = new PlayerInput();
        _playerInput.Enable();

        _playerInput.Player.LeftMouse.performed += ctx => {
            IsLeftMousePressed = true;
            OnLeftMousePerformed?.Invoke();
        };
        _playerInput.Player.LeftMouse.canceled += ctx => {
            IsLeftMousePressed = false;
        };

        _playerInput.Player.RightMouse.started += ctx => {
            IsRightMousePressed = true;
            IsRightMouseHeld = true;
        };
        _playerInput.Player.RightMouse.canceled += ctx => {
            IsRightMousePressed = false;
            IsRightMouseHeld = false;
            IsRightMouseReleased = true;
        };
    }

    private void LateUpdate() {
        IsLeftMousePressed = false;
        IsRightMousePressed = false;
        IsRightMouseReleased = false;
    }



    public Vector2 GetMovementVectorNormalised() {
        Vector2 inputVector = _playerInput.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }



    public void OnDestroy() {

        _playerInput.Disable();
    }

}
