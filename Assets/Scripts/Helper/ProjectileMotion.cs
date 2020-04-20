using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    Vector3 lastPos;
    Vector3 impulse;
    float gravity;

    public bool operateProjectileMotion = true;

    public void Initialize(Vector3 pos, float gravity)
    {
        transform.position = pos;
        lastPos = transform.position;
        this.gravity = gravity;
    }


    private void FixedUpdate()
    {
        if (operateProjectileMotion)
        {
            float dt = Time.fixedDeltaTime;
            Vector3 accel = -gravity * Vector3.up;

            Vector3 curPos = transform.position;
            Vector3 newPos = curPos + (curPos - lastPos) + impulse * dt + accel * dt * dt;

            lastPos = curPos;
            transform.position = newPos;
            transform.forward = newPos - lastPos;

            impulse = Vector3.zero;
        }
    }

    public void AddImpulse(Vector3 impulse)
    {
        this.impulse = impulse;

    }
}
