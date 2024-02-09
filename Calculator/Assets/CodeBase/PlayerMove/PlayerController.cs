using UnityEngine;
using UniRx;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    private const float MaxVerticalRotation = 80f;
    private const float MinVerticalRotation = -70f;
    private const int MaxSpeed = 12;
    
    private Rigidbody _rigidbody;
    private float _currentRotationX = 0f;

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void Start()
    {
        Observable
            .EveryUpdate()
            .Where(_ => IsMoving())
            .Subscribe(_ => Move());

        Observable
            .EveryUpdate()
            .Where(_ => IsCameraRotating())
            .Subscribe(_ => Rotate());
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _currentRotationX -= mouseY * _rotationSpeed;
        _currentRotationX = Mathf.Clamp(_currentRotationX, MinVerticalRotation, MaxVerticalRotation);

        float rotationY = transform.localEulerAngles.y + mouseX * _rotationSpeed;

        transform.localEulerAngles = new Vector3(_currentRotationX, rotationY, 0f);

    }

    private void Move()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveVector = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveInput * _moveSpeed;

        if (_rigidbody.velocity.magnitude < MaxSpeed)
            _rigidbody.AddForce(moveVector, ForceMode.Acceleration);
    }

    private bool IsCameraRotating() => 
        Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0;

    private bool IsMoving() => 
        Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
}
