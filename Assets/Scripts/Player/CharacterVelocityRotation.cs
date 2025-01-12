using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVelocityRotation : MonoBehaviour
{
    private Rigidbody rigidbody;
    [HideInInspector] public Quaternion currentRotation;
    public GameObject characterModel;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        characterModel = CharacterManager.Instance.characterModel;
    }

    void FixedUpdate()
    {
        characterModel.transform.rotation = currentRotation;

        if (rigidbody.velocity != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(rigidbody.velocity);
            currentRotation = rotation;
        }
    }
}
