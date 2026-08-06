<<<<<<< HEAD
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

using UnityEngine;
using UnityEngine.InputSystem;
>>>>>>> 807c1c224ecc82ae33ec7ceffa78d8d1d9784596

public class CarController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float turnSpeed = 60f;
<<<<<<< HEAD
    public enum Axel
    {
      Front,
    Rear  
    }
    [Serializable]
    public struct wheel
    {
        public GameObject WheelModel;
        public WheelCollider wheelcollider; 
        public Axel axel;
    }
    public float maxAcceleration = 30.0f;
    public float breakAcceleration = 50.0f;
    public List<wheel> wheels;
    float moveInput;
    float turnInput;
   
    private Rigidbody rb;
      void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void NewUpdate()
    {
        getInputs();
    }
    void LateUpdate()
    {
        Move();
    }
    void getInputs()
    {
        moveInput = Input.GetAxis("Vertical");
    }
    void Move()
{
    foreach (var wheel in wheels)
    {
        wheel.wheelcollider.motorTorque = moveInput * maxAcceleration;

        if (wheel.axel == Axel.Front)
        {
            wheel.wheelcollider.steerAngle = turnInput * 30f; 
        }
    }
}
=======
>>>>>>> 807c1c224ecc82ae33ec7ceffa78d8d1d9784596

    void Update()
    {
        if (Keyboard.current == null) return;
<<<<<<< HEAD
        turnInput = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);
        float move = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
        float turn = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        
=======

        float move = (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0);
        float turn = (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0);

        // only steer while actually moving, like a real car turning while driving
>>>>>>> 807c1c224ecc82ae33ec7ceffa78d8d1d9784596
        if (Mathf.Abs(move) > 0f)
            transform.Rotate(Vector3.up, turn * turnSpeed * Time.deltaTime * Mathf.Sign(move));

        transform.position += transform.forward * move * moveSpeed * Time.deltaTime;
<<<<<<< HEAD
         Vector3 moveVector = transform.forward * move * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
       Move(); 
    }
    

=======
    }
>>>>>>> 807c1c224ecc82ae33ec7ceffa78d8d1d9784596
}
