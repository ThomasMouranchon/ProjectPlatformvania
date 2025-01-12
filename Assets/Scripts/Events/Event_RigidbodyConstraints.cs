using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Event_RigidbodyConstraints : MonoBehaviour
{
    public Rigidbody rigidbody;
    public bool3 freezePosition;
    public bool3 freezeRotation;

    public void SwitchRigidbodyConstraints()
    {
        RigidbodyConstraints newConstraints = RigidbodyConstraints.None;

        // Ajouter les contraintes en fonction des booléens
        if (freezePosition.x) newConstraints |= RigidbodyConstraints.FreezePositionX;
        if (freezePosition.y) newConstraints |= RigidbodyConstraints.FreezePositionY;
        if (freezePosition.z) newConstraints |= RigidbodyConstraints.FreezePositionZ;

        if (freezeRotation.x) newConstraints |= RigidbodyConstraints.FreezeRotationX;
        if (freezeRotation.y) newConstraints |= RigidbodyConstraints.FreezeRotationY;
        if (freezeRotation.z) newConstraints |= RigidbodyConstraints.FreezeRotationZ;

        rigidbody.constraints = newConstraints;
    }
}
