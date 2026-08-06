using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Speed")]
    public float moveSpeed = 8f;
    public float reverseSpeed = 4f;
    public float acceleration = 20f;
    public float braking = 30f;
    public float deceleration = 12f;

    [Header("Steering")]
    public float turnSpeed = 60f;
    [Range(0f, 1f)] public float minTurnAtMaxSpeed = 0.5f;

    [Header("Handbrake / Drift")]
    public float handbrakeDeceleration = 60f;
    [Range(0f, 1f)] public float driftMinSpeedRatio = 0.5f;
    public float driftTurnMultiplier = 1.6f;
    public float driftGrip = 2f;
    public float normalGrip = 20f;

    [Header("Tire Wear")]
    public float distanceKm;
    public float kmBeforeTireWarning = 50f;
    [Range(0f, 1f)] public float wornSpeedMultiplier = 0.5f;

    public bool TireWarning => distanceKm >= kmBeforeTireWarning;
    [Header("UI Input (Mobile)")]
    private bool uiAccelerate, uiBrake, uiLeft, uiRight;
    Rigidbody rb;
    float currentSpeed;
    Vector3 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveDirection = transform.forward;
    }

    // physics-driven movement (Rigidbody.MovePosition/MoveRotation in FixedUpdate) instead of
    // editing transform directly, so PhysX can actually resolve collisions against obstacles
    void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float move = (Keyboard.current.wKey.isPressed ? 1f : 0f) - (Keyboard.current.sKey.isPressed ? 1f : 0f);
        float turn = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);
        bool handbrake = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        bool isDrifting = handbrake && Mathf.Abs(currentSpeed) > moveSpeed * driftMinSpeedRatio;

        UpdateSpeed(move, handbrake);
        UpdateSteering(turn, isDrifting);

        // moveDirection lags behind the car's facing while drifting, so the body slides sideways
        // instead of tracking straight along transform.forward like normal grippy driving
        float grip = isDrifting ? driftGrip : normalGrip;
        moveDirection = Vector3.Slerp(moveDirection, transform.forward, grip * Time.fixedDeltaTime).normalized;

        rb.MovePosition(rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime);

        // 1 Unity unit == 1 meter, so meters travelled / 1000 == km
        distanceKm += Mathf.Abs(currentSpeed) * Time.fixedDeltaTime / 1000f;
    }

    public void ResetTrip() => distanceKm = 0f;

    void UpdateSpeed(float move, bool handbrake)
    {
        float effectiveMoveSpeed = TireWarning ? moveSpeed * wornSpeedMultiplier : moveSpeed;
        float targetSpeed = handbrake ? 0f : move > 0f ? effectiveMoveSpeed : move < 0f ? -reverseSpeed : 0f;

        // pressing the opposite direction of current motion brakes instead of smoothly accelerating
        bool isBraking = move * currentSpeed < 0f;
        float rate = handbrake ? handbrakeDeceleration : isBraking ? braking : move != 0f ? acceleration : deceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);
    }

    void UpdateSteering(float turn, bool isDrifting)
    {
        if (turn == 0f) return;

        // full steering authority at low speed, tapering off near top speed for stability
        float speedRatio = Mathf.Clamp01(Mathf.Abs(currentSpeed) / moveSpeed);
        float turnAmount = Mathf.Lerp(1f, minTurnAtMaxSpeed, speedRatio);
        float driftMultiplier = isDrifting ? driftTurnMultiplier : 1f;

        Quaternion deltaRotation = Quaternion.Euler(0f, turn * turnSpeed * turnAmount * driftMultiplier * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    // هذه الدوال هي "البوابات" التي سيستخدمها الـ UI لإرسال الأوامر للسيارة
    public void SetAccelerate(bool pressed) => uiAccelerate = pressed;
    public void SetBrake(bool pressed) => uiBrake = pressed;
    public void SetLeft(bool pressed) => uiLeft = pressed;
    public void SetRight(bool pressed) => uiRight = pressed;
}
