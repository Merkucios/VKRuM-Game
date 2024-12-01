using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions
{
    [SerializeField] private GameInput _gameInput;
    
    // Player Actions
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction<bool> MoveStateChangedEvent = delegate { };

    public event UnityAction<Vector2> LookEvent = delegate { };
    public event UnityAction AttackEvent = delegate { };
    public event UnityAction AttackCanceledEvent = delegate { };
    public event UnityAction InteractEvent = delegate { }; 
    public event UnityAction JumpEvent = delegate { };
    public event UnityAction JumpCanceledEvent = delegate { };
    public event UnityAction StartedRunning = delegate { };
    public event UnityAction StoppedRunning = delegate { };
   
    // UI Actions
    public event UnityAction<Vector2> NavigateEvent = delegate { };
    public event UnityAction SubmitEvent = delegate { }; 
    public event UnityAction CancelEvent = delegate { }; 
    public event UnityAction<Vector2> PointEvent = delegate { };

    public event UnityAction ClickEvent = delegate { }; 
    public event UnityAction RightClickEvent = delegate { }; 
    public event UnityAction<Vector2> ScrollWheelEvent = delegate { };

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            
            _gameInput.Player.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        DisableAllActionsMaps();
    }

    
    // Gameplay Action map  
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        MoveEvent.Invoke(direction);

        bool isPressed = context.phase != InputActionPhase.Canceled;
        MoveStateChangedEvent.Invoke(isPressed);
    }
    
    public void OnRun(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                StartedRunning.Invoke();
                break;
            case InputActionPhase.Canceled:
                StoppedRunning.Invoke();
                break;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                AttackEvent.Invoke();
                break;
            case InputActionPhase.Canceled:
                AttackCanceledEvent.Invoke();
                break;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            InteractEvent.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnCrounchEvent not using");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            JumpEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            JumpCanceledEvent.Invoke();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnPrevious not using");

    }

    public void OnNext(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnNext not using");

    }

    
    // UI Action map 
    public void OnNavigate(InputAction.CallbackContext context)
    {
        NavigateEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            SubmitEvent.Invoke();
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            CancelEvent.Invoke();
        }    
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        PointEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            ClickEvent.Invoke();
        }    
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            RightClickEvent.Invoke();
        }    
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnMiddleClick not using");
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        ScrollWheelEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnTrackedDevicePosition not using");

    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
        Debug.Log($"Message from InputReader OnTrackedDeviceOrientation not using");
    }
    
    public void EnableGameplayInput()
    {
        _gameInput.UI.Disable();
        _gameInput.Player.Enable();
    }
    
    public void EnableMenuInput()
    {
        _gameInput.Player.Disable();
        _gameInput.UI.Enable();
    }
    
    public void DisableAllActionsMaps()
    {
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
    }

}