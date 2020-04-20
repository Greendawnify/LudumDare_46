using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBox : MonoBehaviour
{
    Rigidbody rb;
    ProjectileMotion motion;
    [SerializeField] GameObject shieldObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        motion = GetComponent<ProjectileMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;
        //rb.isKinematic = true;

        shieldObj.SetActive(true);
        motion.operateProjectileMotion = false;

    }
}
