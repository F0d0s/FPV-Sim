using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class ThrottleProcessor : InputProcessor<float>
{
#if UNITY_EDITOR
    static ThrottleProcessor()
    {
        Initialize();
    }
#endif

    [RuntimeInitializeOnLoadMethod]
    static void Initialize()
    {
        InputSystem.RegisterProcessor<ThrottleProcessor>();
    }

    [Tooltip("Changing TX value to positive")]
    

    public override float Process(float value, InputControl control)
    {
        
        return (value + 1) / 2;
        
    }
}
