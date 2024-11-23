using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform target; 
    [SerializeField] private float radius = 10f; 
    [SerializeField] private float rotationSpeed = 10f; 

    private float currentAngle; 

    void Update()
    {
        if (target == null) return;
        
        currentAngle += rotationSpeed * Time.deltaTime;

        float angleRad = currentAngle * Mathf.Deg2Rad;

        float x = target.position.x + Mathf.Cos(angleRad) * radius;
        float z = target.position.z + Mathf.Sin(angleRad) * radius;

        transform.position = new Vector3(x, transform.position.y, z);

        transform.LookAt(target);
    }
}