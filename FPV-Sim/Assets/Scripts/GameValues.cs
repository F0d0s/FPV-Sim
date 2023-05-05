using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameValues : MonoBehaviour
{
    [Header("Pitch rates")]
    public static float MaxRatePitch = 720;
    public static float CenterSensitivityPitch = 150;
    public static float ExpoPitch = 0.1f;
    [Header("Yaw rates")]
    public static float MaxRateYaw = 720;
    public static float CenterSensitivityYaw = 150;
    public static float ExpoYaw = 0.1f;
    [Header("Roll rates")]
    public static float MaxRateRoll = 720;
    public static float CenterSensitivityRoll = 150;
    public static float ExpoRoll = 0.1f;

    public static float DronePower = 40;

    public static float Volume = 0;
    
    public static float CamAngle = 10;


}
