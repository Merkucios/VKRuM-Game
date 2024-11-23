using UnityEngine;

public class RockController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _inputVector;
    
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private float moveSpeed = 5f; 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _inputReader.EnableGameplayInput();
    }
    
    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMove;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(_inputVector.x, 0f, _inputVector.y) * moveSpeed;
        _rigidbody.linearVelocity = movement;
    }

    

    private void OnMove(Vector2 direction)
    {
        _inputVector = direction;
        Debug.Log(_inputVector);

    }
}