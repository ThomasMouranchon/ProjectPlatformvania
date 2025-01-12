using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    private static GravityHandler instance = null;
    public static GravityHandler Instance => instance;

    public bool invertedGravity;
    public float currentGravity, normalGravity, moonGravity;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    public void ChangeGravity(bool switchMoon, bool switchInverted)
    {
        if (switchMoon)
        {
            if (currentGravity == normalGravity) currentGravity = moonGravity;
            else currentGravity = normalGravity;
        }

        if (switchInverted)
        {
            invertedGravity = !invertedGravity;
        }
        /*
        if (invertedGravity) Physics.gravity.y = -currentGravity;
        else Physics.gravity.y = currentGravity;*/
    }
}