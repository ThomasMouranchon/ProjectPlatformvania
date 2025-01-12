using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOptionVSync : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        /*if (QualitySettings.vSyncCount == 1) target.SetActive(false);
        else target.SetActive(true);*/
    }

    public void ShowOptionsVSync()
    {
        //target.SetActive(!target.activeSelf);
        if (QualitySettings.vSyncCount == 1) target.SetActive(true);
        else target.SetActive(false);
    }
}
