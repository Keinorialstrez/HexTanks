using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetric
{
    public const float outerRadius = 5f;

    public const float innerRadius = outerRadius * 0.866025404f;

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f  * outerRadius),
        new Vector3(0f, 0f, outerRadius)

    };

}
