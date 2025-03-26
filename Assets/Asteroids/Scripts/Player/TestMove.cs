using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float drag = 0.01f;
    [SerializeField] private float acceleration = 5f;

    private Vector3 _velocity = Vector3.zero;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    
    public void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _velocity += transform.forward * (acceleration * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            _velocity += -transform.forward * (acceleration * Time.deltaTime);
        }
        
        _velocity += -_velocity.normalized * drag * Time.deltaTime;
        
        _velocity = Vector2.ClampMagnitude(_velocity, maxSpeed);
        transform.position += _velocity * Time.deltaTime;
    }

    public void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        Vector3 rotation = new Vector3(-mouseY, mouseX, 0f) * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotation, Space.Self);
    }
}