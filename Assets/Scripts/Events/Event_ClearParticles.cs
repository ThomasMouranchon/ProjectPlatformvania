using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_ClearParticles : MonoBehaviour
{
    public void ClearParticles()
    {
        ParticleSystem[] particleSystems = FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem ps in particleSystems)
        {
            Destroy(ps.gameObject);
        }
    }
}