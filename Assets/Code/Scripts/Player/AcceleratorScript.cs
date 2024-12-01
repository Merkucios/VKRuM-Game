using UnityEngine;

public class AcceleratorScript : MonoBehaviour
{
    [SerializeField] private float _sensetive;
    [SerializeField] private float _power;
    [SerializeField] private float forceMultiplier = 10f;
    private Rigidbody _rb;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving) {
            Move();
        } 
        else {
            LaunchBall();
        }   
    }

    private void Move()
    {
        Vector3 acceleration = new Vector3(Input.acceleration.x, 0, 0);
        _rb.AddForce(acceleration * _sensetive);
    }

    private void LaunchBall() 
    {
        Vector3 acceleration = new Vector3(0, Input.acceleration.y, 0);
        //Vector3 acceleration = Input.acceleration;
        if (acceleration.sqrMagnitude >= forceMultiplier) 
        {
            Vector3 movingDir = _power * new Vector3(0, 0, acceleration.y);
            _rb.AddForce(movingDir, ForceMode.Force);
            moving = true;
        }
    }
}
