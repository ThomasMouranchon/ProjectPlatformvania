using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_StartRailMovement : MonoBehaviour
{
    private bool railMovementIsActive;
    [SerializeField] private RailMovement railMovement;

    private void Awake()
    {
        if (railMovement == null) railMovement = GetComponent<RailMovement>();

        railMovementIsActive = false;

        railMovement.enabled = railMovementIsActive;
    }

    public void StartRailMovement()
    {
        if (!railMovementIsActive)
        {
            railMovementIsActive = true;
            railMovement.enabled = railMovementIsActive;
        }        
    }
}
