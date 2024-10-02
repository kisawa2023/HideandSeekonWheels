using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Настройки движения")]
    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float handbrakeForce = 5000f;
    public float maxSteerAngle = 30f;

    [Header("Коллайдеры колёс")]
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    [Header("Модели колёс")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private float horizontalInput;
    private float verticalInput;
    private bool isHandbraking;
    private float currentBrakeForce;
    private float currentSteerAngle;

    private void Start()
    {
        // Опускаем центр массы для устойчивости
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void Update()
    {
        GetInput();
        UpdateWheels();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        HandleBraking();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // A и D
        verticalInput = Input.GetAxis("Vertical");     // W и S
        isHandbraking = Input.GetKey(KeyCode.Space);   // Пробел
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void HandleBraking()
    {
        if (isHandbraking)
        {
            currentBrakeForce = handbrakeForce;
            ApplyBraking();
        }
        else
        {
            currentBrakeForce = 0f;
            rearLeftWheelCollider.brakeTorque = 0f;
            rearRightWheelCollider.brakeTorque = 0f;
        }
    }

    private void ApplyBraking()
    {
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPose(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPose(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 _pos;
        Quaternion _quat;
        wheelCollider.GetWorldPose(out _pos, out _quat);
        wheelTransform.position = _pos;
        wheelTransform.rotation = _quat;
    }
}
