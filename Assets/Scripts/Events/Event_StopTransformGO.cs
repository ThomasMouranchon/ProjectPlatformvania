using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_StopTransformGO : MonoBehaviour
{
    public Event_TransformGO event_TransformGO;

    public void StopTransformGO()
    {
        event_TransformGO.startTrPosition = false;
        event_TransformGO.startTrRotation = false;
        event_TransformGO.startTrScale = false;
    }
}
