using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Event_TransformGO;

public class Event_TransformGO : MonoBehaviour
{
    public enum RotationType
    {
        Follow,
        RotateTowards
    }

    [HideInInspector] public bool startTrPosition, startTrRotation, startTrScale;

    public GameObject target;

    private Vector3 targetStartPosition;
    private Quaternion targetStartRotation;
    private Vector3 targetStartScale;

    public GameObject targetEnd;
    public Vector3 positionSpeed, rotationSpeed, scaleSpeed;
    [Space(10)]

    public RotationType rotationType;
    public bool endWhenFinished = true;

    private float tolerance = 0.01f;

    public void TransformGameObject(bool instant)
    {
        targetStartPosition = target.transform.position;
        targetStartRotation = target.transform.rotation;
        targetStartScale = target.transform.localScale;
        if (instant)
        {
            target.transform.position = targetEnd.transform.position;
            target.transform.rotation = Quaternion.Euler(targetEnd.transform.eulerAngles);
            target.transform.localScale = targetEnd.transform.localScale;
        }
        else
        {
            target.transform.position = targetStartPosition;
            target.transform.rotation = targetStartRotation;
            target.transform.localScale = targetStartScale;

            startTrPosition = true;
            startTrRotation = true;
            startTrScale = true;
        }
    }

    void FixedUpdate()
    {
        if (startTrPosition)
        {
            target.transform.position = Vector3.MoveTowards(
                target.transform.position,
                targetEnd.transform.position,
                positionSpeed.magnitude * Time.deltaTime
            );

            if (Vector3.Distance(target.transform.position, targetEnd.transform.position) < tolerance && endWhenFinished) startTrPosition = false;
        }

        if (startTrRotation)
        {
            Quaternion targetQuaternion = Quaternion.Euler(targetEnd.transform.eulerAngles);
            
            switch (rotationType)
            {
                case RotationType.Follow:
                    targetQuaternion = Quaternion.Euler(targetEnd.transform.eulerAngles);
                    target.transform.rotation = Quaternion.RotateTowards(
                        target.transform.rotation,
                        targetQuaternion,
                        rotationSpeed.magnitude * Time.deltaTime
                    );
                    break;
                case RotationType.RotateTowards:
                    Vector3 direction = (targetEnd.transform.position - target.transform.position).normalized;
                    targetQuaternion = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                    target.transform.rotation = Quaternion.Slerp(target.transform.rotation, targetQuaternion, Time.deltaTime * rotationSpeed.magnitude);
                    break;
            }

            if (Quaternion.Angle(target.transform.rotation, targetQuaternion) < tolerance && endWhenFinished) startTrRotation = false;

        }

        if (startTrScale)
        {
            target.transform.localScale = Vector3.MoveTowards(
                target.transform.localScale,
                targetEnd.transform.localScale,
                scaleSpeed.magnitude * Time.deltaTime
            );

            if (Vector3.Distance(target.transform.localScale, targetEnd.transform.localScale) < tolerance && endWhenFinished) startTrScale = false;
        }
    }
}