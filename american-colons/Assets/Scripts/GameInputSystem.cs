using System;
using UnityEngine;

public class GameInputSystem : MonoBehaviour
{
    public event EventHandler<OnStartBuildingEventArgs> OnStartBuilding;
    public event EventHandler OnCancel;
    public event EventHandler OnBuildDestroy;
    public event EventHandler OnStartDestruction;

    public class OnStartBuildingEventArgs : EventArgs {
        public int selectedBuilding;
    }


    private PlayerInputActions playerInputActions;

    private void Awake() 
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Building.StartBuilding.performed += StartBuilding_performed;
        playerInputActions.Building.Cancel.performed += Cancel_performed;
        playerInputActions.Building.BuildDestroy.performed += BuildDestroy_performed;
        playerInputActions.Building.StartDestruction.performed += StartDestruction_performed;


        playerInputActions.Camera.Enable();
        playerInputActions.Building.Enable();
    }

    private void Cancel_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnCancel?.Invoke(this, EventArgs.Empty);
    }

    private void StartBuilding_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStartBuilding?.Invoke(this, new OnStartBuildingEventArgs { 
            selectedBuilding = (int)obj.ReadValue<float>()
        });
    }

    private void BuildDestroy_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnBuildDestroy?.Invoke(this, EventArgs.Empty);
    }

    private void StartDestruction_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnStartDestruction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() 
    {
        return playerInputActions.Camera.Movement.ReadValue<Vector2>();
    }

    public float GetRotationDirection()
    {
        return playerInputActions.Camera.Rotation.ReadValue<float>();
    }

    public Vector2 GetMousePosition()
    {
        return playerInputActions.Camera.Position.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDragDeltaVector()
    {
        return playerInputActions.Camera.MouseDrag.ReadValue<Vector2>();
    }

    public float GetScrollValue()
    {
        float scrollValue =  playerInputActions.Camera.Scroll.ReadValue<float>();
        if (scrollValue > 0)
            return 1;
        else if (scrollValue < 0)
            return -1;
        else 
            return 0;
    }
}
