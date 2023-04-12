using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;


public class DroneMovement : MonoBehaviour
{
    private Rigidbody _rb;
    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    [SerializeField] private float tSens = 20;
    [SerializeField] private float ySens = 20;
    [SerializeField] private float rSens = 20;
    [SerializeField] private float pSens = 20;

    public InputActions inputActions;

    private Quaternion droneQuaternion;
    
    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Drone.Enable();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputGather();
    }

    private void FixedUpdate()
    {
        droneQuaternion = Quaternion.Euler(pitch * pSens * Time.deltaTime, yaw * ySens * Time.deltaTime, roll * rSens * Time.deltaTime);
        _rb.AddRelativeForce(0,throttle * tSens, 0);
        _rb.MoveRotation(_rb.rotation * droneQuaternion);
    }

    private void InputGather()
    {
        roll = inputActions.Drone.Roll.ReadValue<float>();
        pitch = inputActions.Drone.Pitch.ReadValue<float>();
        yaw = inputActions.Drone.Yaw.ReadValue<float>();
        throttle = inputActions.Drone.Throttle.ReadValue<float>();
    }
}
