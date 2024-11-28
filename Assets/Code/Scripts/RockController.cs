using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RockController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _inputVector;
    private Vector2 _lastInputVector;
    private Vector2 _currentInputVector;
    private float _forceStrength;

    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float forceMultiplier = 10f;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    
    private Vector2 _inputVectorVelocity;
    private bool _isTurning;

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

    private void Update()
    {
        _currentInputVector = Vector2.SmoothDamp(
            _currentInputVector, 
            _lastInputVector, 
            ref _inputVectorVelocity, 
            rotationSmoothTime
        );
    }

    private void FixedUpdate()
    {
        if (_isTurning)
        {
            ApplyTurningForce();
        }
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
        _isTurning = isPressed;
        if (!isPressed)
        {
            ApplyForwardForce();
        }
    }

    private void ApplyForwardForce()
    {
        if (_lastInputVector == Vector2.zero) return;

        Vector3 force = _forceStrength * forceMultiplier * new Vector3(0f, 0f, _currentInputVector.y) ;
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }
    
    private void ApplyTurningForce()
    {
        if (_lastInputVector == Vector2.zero) return;

        Vector3 force = _forceStrength * forceMultiplier * new Vector3(_currentInputVector.x, 0f, 0f) ;
        _rigidbody.AddForce(force, ForceMode.Force);
    }
}