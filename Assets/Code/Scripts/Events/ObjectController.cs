using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private Rigidbody[] childRigidbodies;
    private Collider[] childColliders;

    private void Start()
    {
        // Получаем все Rigidbody и Colliders дочерних объектов
        childRigidbodies = GetComponentsInChildren<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();

        // Изначально отключаем физику у всех дочерних объектов
        foreach (Rigidbody rb in childRigidbodies)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in childColliders)
        {
            col.isTrigger = true; // Только для обнаружения касания
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Проверяем, касается ли объект с тегом "Ball"
        if (collision.gameObject.CompareTag("Ball"))
        {
            ActivatePhysics();
        }
    }

    private void ActivatePhysics()
    {
        // Включаем физику у всех дочерних объектов
        foreach (Rigidbody rb in childRigidbodies)
        {
            rb.isKinematic = false; // Отключаем кинематику, включаем физику
        }

        foreach (Collider col in childColliders)
        {
            col.isTrigger = false; // Делаем коллайдеры активными для физики
        }
        this.GetComponent<BoxCollider>().enabled = false;
    }
}
