using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calculate
{
    private static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // return = (1 - t)3 P0 + 3(1 - t)2 tP1 + 3(1 - t) t2 P2 + t3 P3
        //             uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
