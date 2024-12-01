using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    private bool timerActive = false;
    private float timer = 0f;
    private float delay = 3f;

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, если объект с тегом "Ball" вышел из триггера
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Ball вышел из триггера. Запуск таймера.");
            timerActive = true;
            timer = delay; // Устанавливаем начальное значение таймера
        }
    }

    private void Update()
    {
        // Если таймер активен, уменьшаем время
        if (timerActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timerActive = false; // Останавливаем таймер
                TurnManager.Instance.passed = true; // Меняем переменную в TurnManager
                Debug.Log("Таймер завершён. Переменная passed изменена.");
            }
        }
    }
}
