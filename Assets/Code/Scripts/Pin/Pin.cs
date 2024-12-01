using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private PinManagerSO pinsManager; // Ссылка на SO
    [SerializeField] private GameObject pinModel; // Дочерний объект для визуализации
    [SerializeField] private float fragmentLifetime = 10f; // Время жизни фрагментов

    private bool isKnockedDown = false;

    private void Awake()
    {
        // Если pinModel не задан, используем текущий объект
        if (pinModel == null)
        {
            pinModel = gameObject;
        }

        pinsManager.RegisterPin(); // Регистрируем кеглю в менеджере
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isKnockedDown && collision.gameObject.CompareTag("Ball"))
        {
            KnockDown();
        }
    }

    private void KnockDown()
    {
        isKnockedDown = true;

        // Сообщаем менеджеру о сбитой кегле
        pinsManager.KnockDownPin();

        // Отключаем компоненты для предотвращения дальнейших взаимодействий
        DisablePin();

        // Удаляем фрагменты после заданного времени
        StartCoroutine(DestroyFragmentsAfterDelay());
    }

    private void DisablePin()
    {
        if (pinModel != null)
        {
            // Отключаем все коллайдеры на объекте
            foreach (var collider in pinModel.GetComponentsInChildren<Collider>())
            {
                Debug.Log(collider);
                collider.enabled = false;
            }

            // Отключаем рендеринг объекта
            foreach (var renderer in pinModel.GetComponentsInChildren<MeshRenderer>())
            {
                Debug.Log(renderer);
                renderer.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning($"{name}: pinModel is not set.");
        }
    }

    private IEnumerator DestroyFragmentsAfterDelay()
    {
        yield return new WaitForSeconds(fragmentLifetime);

        // Удаляем объект из сцены
        if (pinModel != null)
        {
            Debug.Log(pinModel);
            Destroy(pinModel);
        }
    }
}