using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.81f;
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private float verticalVelocity;
    private float xRotation = 0f;
    public bool inputLocked = false;

    private Vector2 moveInput;
    private Vector2 lookInput;

    void Update()
    {
        // Read new Input System values directly (no Input Actions asset needed for this quick version)
        moveInput = new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
        );
        lookInput = Mouse.current.delta.ReadValue();

        HandleLook();
        HandleMove();
    }

    void HandleLook()
    {
        if (inputLocked) return;

        float mouseX = lookInput.x * mouseSensitivity * 0.1f;
        float mouseY = lookInput.y * mouseSensitivity * 0.1f;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMove()
    {
        if (inputLocked) return;

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;
        controller.Move(move * speed * Time.deltaTime + Vector3.up * verticalVelocity * Time.deltaTime);
    }
}