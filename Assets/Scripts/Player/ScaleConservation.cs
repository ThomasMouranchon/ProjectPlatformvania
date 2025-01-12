using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleConservation : MonoBehaviour
{
    private Vector3 originalScale, globalScale;

    void Awake()
    {
        originalScale = transform.lossyScale;
    }

    public void ChangeParent(Collision collision)
    {
        globalScale = transform.lossyScale;
        transform.SetParent(collision.transform);
    }

    private void FixedUpdate()
    {
        if (transform.parent != null)
        {
            globalScale = transform.lossyScale;
            Vector3 parentScale = transform.parent ? transform.parent.lossyScale : Vector3.one;
            transform.localScale = new Vector3(
                globalScale.x / parentScale.x,
                globalScale.y / parentScale.y,
                globalScale.z / parentScale.z
            );
        }
    }
}
