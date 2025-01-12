using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ChangeGravity : MonoBehaviour
{
    public bool switchMoonGravity, switchInvertedGravity;

    public void ChangeGravity()
    {
        GravityHandler.Instance.ChangeGravity(switchMoonGravity, switchInvertedGravity);
    }
}