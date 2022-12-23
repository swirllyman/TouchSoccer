using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static float GetPerc(float min, float max, float value)
    {
        return Mathf.Clamp01((value - min) / (max - min));
    }
}
