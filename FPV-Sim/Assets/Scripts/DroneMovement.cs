using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class DroneMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float throttle;
    private float roll;
    private float tStrength = 20;

    public InputActions inputActions;
    // Start is called before the first frame update
    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Drone.Enable();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roll = inputActions.Drone.Roll.ReadValue<float>();
        throttle = inputActions.Drone.Throttle.ReadValue<float>();
        
        
        
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(0,throttle * tStrength, 0);
    }

    private void Throttle()
    {
        
    }
}
