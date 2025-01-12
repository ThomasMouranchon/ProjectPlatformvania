using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_UI : MonoBehaviour
{
    public bool hideUI;

    public void ShowUI()
    {
        HealthManager.Instance.HideUI(hideUI);
    }
}