using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BallasticMovement
{

    public static bool solveBallisticArc(Vector3 proj_pos, float lateral_speed, Vector3 target_pos, float max_height, out Vector3 fire_velocity, out float gravity)
    {

        fire_velocity = Vector3.zero;
        gravity = float.NaN;

        Vector3 diff = target_pos - proj_pos;
        Vector3 diffXZ = new Vector3(diff.x, 0f, diff.z);
        float lateralDist = diffXZ.magnitude;

        if (lateralDist == 0)
        {
            Debug.Log("later distance was zero");
            return false;
        }

        float time = lateralDist / lateral_speed;

        fire_velocity = diffXZ.normalized * lateral_speed;

        float a = proj_pos.y;       // initial
        float b = max_height;       // peak
        float c = target_pos.y;     // final

        gravity = -4 * (a - 2 * b + c) / (time * time);
        fire_velocity.y = -(3 * a - 4 * b + c) / time;


        return true;

    }
}
