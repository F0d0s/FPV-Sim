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
    //Input and throttle power
    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    [SerializeField] private float throttlePower = 20;
    
    //Other
    private Rigidbody _rb;
    public InputActions inputActions;

    //Rate values
    [Header("Pitch rates")]
    [SerializeField]private float maxRatePitch = 720;
    [SerializeField]private float centerSensitivityPitch = 150;
    [SerializeField]private float expoPitch = 0.1f;
    [Header("Yaw rates")]
    [SerializeField]private float maxRateYaw = 720;
    [SerializeField]private float centerSensitivityYaw = 150;
    [SerializeField]private float expoYaw = 0.1f;
    [Header("Roll rates")]
    [SerializeField]private float maxRateRoll = 720;
    [SerializeField]private float centerSensitivityRoll = 150;
    [SerializeField]private float expoRoll = 0.1f;

    private float outputPitch;
    private float outputYaw;
    private float outputRoll;
    
    //"Ground effect" values and things
    private float groundEffect;
    private float groundEffectDistance;
    private RaycastHit groundEffectHit;
    private bool nearGround;
    
    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Drone.Enable();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputGather();
        nearGround = Physics.BoxCast(_rb.transform.position, new Vector3(1.5f, 0.25f, 1.5f), -_rb.transform.up, out groundEffectHit, Quaternion.identity, 1);
        if (nearGround)
        {
            groundEffectDistance = Mathf.Clamp(groundEffectHit.distance, 0f, 1f);
            groundEffect = 2f - groundEffectDistance;
        }
        else if (nearGround == false)
        {
            groundEffect = 1;
        }
        
        outputPitch = GetBetaflightActualRate(maxRatePitch, centerSensitivityPitch, expoPitch, pitch);
        outputYaw = GetBetaflightActualRate(maxRateYaw, centerSensitivityYaw, expoYaw, yaw);
        outputRoll = GetBetaflightActualRate(maxRateRoll, centerSensitivityRoll, expoRoll, roll);
    }

    private void FixedUpdate()
    {
        _rb.AddRelativeForce(0,throttle * throttlePower * groundEffect, 0);
        _rb.AddRelativeTorque(
            outputPitch * 6.75f * Time.deltaTime,
            outputYaw * 6.75f * Time.deltaTime,
            outputRoll * 6.75f * Time.deltaTime);
    }

    private void InputGather()
    {
        roll = inputActions.Drone.Roll.ReadValue<float>();
        pitch = inputActions.Drone.Pitch.ReadValue<float>();
        yaw = inputActions.Drone.Yaw.ReadValue<float>();
        throttle = inputActions.Drone.Throttle.ReadValue<float>();
    }
    
    public float GetBetaflightActualRate(double maxRate, double centerSensitivity, double expo, double input)
    {
        double absInput = Math.Abs(input);
        double inputSign = Math.Sign(input);
        double expoFactor = absInput * (Math.Pow(absInput, 5) * expo + absInput * (1 - expo));
        double rateCurve = (centerSensitivity * absInput) + ((maxRate - centerSensitivity) * expoFactor);
        return (float)(rateCurve * inputSign);
    }
}
