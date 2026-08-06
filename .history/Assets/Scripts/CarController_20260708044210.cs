using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float turnSpeed = 60f;

    void Update()
    {
        if (Keyboard.current == null) return;

        float move = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
        float turn = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        // only steer while actually moving, like a real car turning while driving
        if (Mathf.Abs(move) > 0f)
            transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime * Mathf.Sign(move));

        transform.position += transform.forward * move * moveSpeed * Time.deltaTime;
    }
}
