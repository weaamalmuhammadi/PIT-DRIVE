using UnityEngine;
using UnityEngine.InputSystem;

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

    float currentSpeed;

    void Update()
    {
        if (Keyboard.current == null) return;

        float move = (Keyboard.current.wKey.isPressed ? 1f : 0f) - (Keyboard.current.sKey.isPressed ? 1f : 0f);
        float turn = (Keyboard.current.dKey.isPressed ? 1f : 0f) - (Keyboard.current.aKey.isPressed ? 1f : 0f);

        UpdateSpeed(move);
        UpdateSteering(turn);

        transform.position -= transform.forward * currentSpeed * Time.deltaTime;
    }

    void UpdateSpeed(float move)
    {
        float targetSpeed = move > 0f ? moveSpeed : move < 0f ? -reverseSpeed : 0f;

        // pressing the opposite direction of current motion brakes instead of smoothly accelerating
        bool isBraking = move * currentSpeed < 0f;
        float rate = isBraking ? braking : move != 0f ? acceleration : deceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.deltaTime);
    }

    void UpdateSteering(float turn)
    {
        if (turn == 0f) return;

        // full steering authority at low speed, tapering off near top speed for stability
        float speedRatio = Mathf.Clamp01(Mathf.Abs(currentSpeed) / moveSpeed);
        float turnAmount = Mathf.Lerp(1f, minTurnAtMaxSpeed, speedRatio);

        transform.Rotate(Vector3.up, -turn * turnSpeed * turnAmount * Time.deltaTime);
    }
}
