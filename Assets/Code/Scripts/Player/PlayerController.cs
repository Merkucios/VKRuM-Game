using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private InputReader _inputReader;

    [SerializeField] private float moveSpeed = 5f; // Скорость движения персонажа

    [Inject]
    public void Construct(InputReader inputReader)
    {
        _inputReader = inputReader;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Получаем компонент Rigidbody
    }

    private void OnEnable()
    {
        // Подписываемся на события ввода
        _inputReader.MoveEvent += OnMove;
    }

    private void OnDisable()
    {
        // Отписываемся от событий ввода
        _inputReader.MoveEvent -= OnMove;
    }

    private void OnMove(Vector2 direction)
    {
        // Применяем силу к Rigidbody на основе направления движения
        Vector3 movement = new Vector3(direction.x, 0, direction.y) * moveSpeed;
        _rigidbody.linearVelocity = new Vector3(movement.x, _rigidbody.linearVelocity.y, movement.z); // Сохраняем вертикальную скорость
    }
}