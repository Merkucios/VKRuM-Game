using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenButtonTurn : OnScreenControl, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("Input Control Path (<Gamepad>/leftStick/left or <Gamepad>/leftStick/right)")]
    private string controlPathOverride = "<Gamepad>/leftStick"; 

    [SerializeField, Tooltip("Direction for movement (-1 for left, 1 for right)")]
    private float directionValue = -1f;

    private bool isPressed; 

    protected override string controlPathInternal
    {
        get => controlPathOverride;
        set => controlPathOverride = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true; 
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        SendValueToControl(Vector2.zero); 
    }

    private void Update()
    {
        if (isPressed)
        {
            SendValueToControl(new Vector2(directionValue, 0f));
        }
    }
}