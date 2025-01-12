using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerTransformTarget;

public class PlayerTransformTarget : MonoBehaviour
{
    public enum TargetedTransform
    {
        Start,
        End
    }

    public TargetedTransform targetedTransform;
    public Event_TransformGO event_TransformGO;

    void Start()
    {
        switch (targetedTransform)
        {
            default:
                event_TransformGO.target = CharacterManager.Instance.gameObject;
                break;
            case TargetedTransform.End:
                event_TransformGO.targetEnd = CharacterManager.Instance.gameObject;
                break;
        }
    }
}
