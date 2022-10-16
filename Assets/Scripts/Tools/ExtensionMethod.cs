using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;

    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorTarget = target.position - transform.position;
        vectorTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorTarget);

        return dot >= dotThreshold;
    }
}
