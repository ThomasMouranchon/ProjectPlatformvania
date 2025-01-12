using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCounter : MonoBehaviour
{
    public ParticleSystem particle;
    public int particleTimer;

    void Update()
    {
        particleTimer++;
        if (particleTimer > 5)
        {
            Instantiate(particle, transform.position, transform.rotation);
            particleTimer = 0;
        }
    }
}
