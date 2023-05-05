using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
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
    private float arm;
    [SerializeField] private float throttlePower = 40;
    
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
    
    //Animations
    [SerializeField]private Animator FL_prop;
    [SerializeField]private Animator FR_prop;
    [SerializeField]private Animator BL_prop;
    [SerializeField]private Animator BR_prop;
    private float flProp;
    private float frProp;
    private float blProp;
    private float brProp;
    
    //Sounds
    [SerializeField] AudioSource droneAudio;
    private bool isPlaying = false;
    
    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Drone.Enable();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    void Update()
    {
        UpdateValues();
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
        if (arm == 1)
        {
        flProp = arm + throttle + Mathf.Clamp(roll, -1, 0) * -1 + Mathf.Clamp(pitch, -1, 0) * -1 + Mathf.Clamp(yaw, -1, 0) * -1;
        frProp = arm + throttle + Mathf.Clamp(roll, 0, 1) + Mathf.Clamp(pitch, -1, 0) * -1  + Mathf.Clamp(yaw, 0, 1);
        blProp = arm + throttle + Mathf.Clamp(roll, -1, 0) * -1 + Mathf.Clamp(pitch, 0, 1) + Mathf.Clamp(yaw, 0, 1);
        brProp = arm + throttle + Mathf.Clamp(roll, 0, 1) + Mathf.Clamp(pitch, 0, 1) + Mathf.Clamp(yaw, -1, 0) * -1;
        }
        else if (arm == 0)
        {
            flProp = 0;
            frProp = 0;
            blProp = 0;
            brProp = 0;
        }
            
        FL_prop.SetFloat("MovementFL", flProp);
        FR_prop.SetFloat("MovementFR", frProp);
        BL_prop.SetFloat("MovementBL", blProp);
        BR_prop.SetFloat("MovementBR", brProp);

        if (arm == 1)
        {
            if (isPlaying == false)
            {
                droneAudio.Play();
            }
            isPlaying = true;
        }
        else if (arm == 0)
        {
            if (isPlaying == true)
            {
                droneAudio.Stop();
            }
            isPlaying = false;
        }

        if (Time.timeScale == 0)
        {
            droneAudio.Stop();
            isPlaying = false;
        }

        droneAudio.pitch = 1 + (arm + throttle + MathF.Abs(roll) + MathF.Abs(pitch) + MathF.Abs(yaw)) * 0.5f;
    }

    private void FixedUpdate()
    {
        if (arm == 1)
        {
            _rb.AddRelativeForce(0,throttle * throttlePower * groundEffect, 0);
            _rb.AddRelativeTorque(
                outputPitch * 6.75f * Time.deltaTime,
                outputYaw * 6.75f * Time.deltaTime,
                outputRoll * 6.75f * Time.deltaTime);
        }
    }

    private void InputGather()
    {
        roll = inputActions.Drone.Roll.ReadValue<float>();
        pitch = inputActions.Drone.Pitch.ReadValue<float>();
        yaw = inputActions.Drone.Yaw.ReadValue<float>();
        throttle = inputActions.Drone.Throttle.ReadValue<float>();
        arm = Mathf.Clamp(inputActions.Drone.Arm.ReadValue<float>(), 0, 1);
    }
    
    public float GetBetaflightActualRate(double maxRate, double centerSensitivity, double expo, double input)
    {
        double absInput = Math.Abs(input);
        double inputSign = Math.Sign(input);
        double expoFactor = absInput * (Math.Pow(absInput, 5) * expo + absInput * (1 - expo));
        double rateCurve = (centerSensitivity * absInput) + ((maxRate - centerSensitivity) * expoFactor);
        return (float)(rateCurve * inputSign);
    }

    public void UpdateValues()
    {
        maxRatePitch = GameValues.MaxRatePitch;
        centerSensitivityPitch = GameValues.CenterSensitivityPitch;
        expoPitch = GameValues.ExpoPitch;
        maxRateRoll = GameValues.MaxRateRoll;
        centerSensitivityRoll = GameValues.CenterSensitivityRoll;
        expoRoll = GameValues.ExpoRoll;
        maxRateYaw = GameValues.MaxRateYaw;
        centerSensitivityYaw = GameValues.CenterSensitivityYaw;
        expoYaw = GameValues.ExpoYaw;
        throttlePower = GameValues.DronePower;
    }
}
