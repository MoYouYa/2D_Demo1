using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomData
{
#if UNITY_EDITOR
    public static float FLOAT_PERCISION = 1e-5f;
#else
    public static float FLOAT_PERCISION = 1e-2f;
#endif
}
