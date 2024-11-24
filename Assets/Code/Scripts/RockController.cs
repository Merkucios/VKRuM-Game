using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RockController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _inputVector;
    private Vector2 _lastInputVector;
    private float _forceStrength;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float forceMultiplier = 10f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader.EnableGameplayInput();
    }

    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
        _inputReader.MoveStateChangedEvent += OnMoveStateChanged;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
        _inputReader.MoveStateChangedEvent -= OnMoveStateChanged;
    }

    private void OnMove(Vector2 direction)
    {
        _inputVector = direction.normalized;
        if (_inputVector != Vector2.zero)
        {
            _lastInputVector = -_inputVector; 
            _forceStrength = direction.magnitude; 
        }

        Debug.Log($"_inputVector: {_inputVector}, _lastInputVector: {_lastInputVector}");
    }

    private void OnMoveStateChanged(bool isPressed)
    {
        if (!isPressed)
        {
            ApplyForce();
        }
    }

    private void ApplyForce()
    {
        if (_lastInputVector == Vector2.zero) return; 

        Vector3 force = new Vector3(0f, 0f, _lastInputVector.y) * _forceStrength * forceMultiplier;
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }
}