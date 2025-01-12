using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private EventList eventList;

    private void Awake()
    {
        eventList = gameObject.GetComponent<EventList>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            eventList.CallEventList();
        }
    }
}