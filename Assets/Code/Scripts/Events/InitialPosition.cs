using UnityEngine;

public class InitialPosition : MonoBehaviour
{
    public Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
}
