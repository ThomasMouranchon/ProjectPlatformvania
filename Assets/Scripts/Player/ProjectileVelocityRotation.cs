using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileVelocityRotation : MonoBehaviour
{
    private Rigidbody rigidbody;
    [HideInInspector] public Quaternion currentRotation;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.rotation = currentRotation;

        if (rigidbody.velocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rigidbody.velocity);
            currentRotation = rotation;
        }
    }
}
