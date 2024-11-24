using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;

public class OnScreenStickOnlyY : OnScreenControl, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;
    [SerializeField] private RectTransform background; 
    [SerializeField] private float handleRange = 1f; 
    [SerializeField] private float deadZone = 0f; 
    [SerializeField, Tooltip("Input Control путь (<Gamepad>/leftStick)")]
    private string controlPathOverride = "<Gamepad>/leftStick"; 

    private Vector2 _pointerDownPosition;
    private Vector2 _input;

    private void Start()
    {
        if (background == null)
        {
            Debug.LogError("Background is not assigned!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (background != null)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out _pointerDownPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null || handle == null)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out Vector2 pointerPosition);
        Vector2 delta = pointerPosition - _pointerDownPosition;

        delta.x = 0f;

        _input = delta / (background.sizeDelta.y * 0.5f); 
        _input = Vector2.ClampMagnitude(_input, 1f); 

        if (_input.magnitude < deadZone)
            _input = Vector2.zero;

        if (_input.y > 0)
            _input.y = 0;

        handle.anchoredPosition = new Vector2(0f, _input.y * handleRange * (background.sizeDelta.y * 0.5f));

        SendValueToControl(new Vector2(0f, _input.y));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
    }

    protected override string controlPathInternal
    {
        get => controlPathOverride;
        set => controlPathOverride = value; 
    }
}
