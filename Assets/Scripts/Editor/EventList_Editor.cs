using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EventList))]
public class EventList_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EventList eventList = (EventList)target;

        if (GUILayout.Button("Call EventList"))
        {
            eventList.CallEventList();
        }
    }
}
