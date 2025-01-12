using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Particles : MonoBehaviour
{
    public ParticleSystem[] particleEffects;

    public void InstantiateParticles()
    {
        foreach (ParticleSystem effect in particleEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
    }
}