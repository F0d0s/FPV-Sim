using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class ArmProcessor : InputProcessor<float>
{
    
    
#if UNITY_EDITOR
    static ArmProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<ArmProcessor>();
    }

    [Tooltip("Making a button toggle")]
    
    bool armed;
    public override float Process(float value, InputControl control)
    {
        switch ((int)value, armed)
        {
            case (1, false):
                armed = true;
                return 1;
            case (1, true):
                armed = false;
                return 0;
            case(0, true):
                return 1;
            case(0, false):
                return 0;
        }

        return 0;
    }
}

