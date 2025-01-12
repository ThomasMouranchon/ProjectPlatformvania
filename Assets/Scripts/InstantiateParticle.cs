using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InstantiateParticle : MonoBehaviour
{
    public ParticleSystem particleEffect;
    public int particleTimer = 2;
    private int originalParticleTimer;
    public float beforeParticleTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        originalParticleTimer = particleTimer;
        particleTimer = 0;
    }
    
    public void InstantiateParticleEffect()
    {
        StartCoroutine(InstantiateParticleCoroutine());
    }

    private IEnumerator InstantiateParticleCoroutine()
    {
        yield return new WaitForSecondsRealtime(beforeParticleTimer);

        while (Time.timeScale == 0)
        {
            if (particleTimer < originalParticleTimer) particleTimer++;
            else
            {
                Instantiate<ParticleSystem>(particleEffect, this.transform.position, this.transform.rotation);
                particleTimer = 0;
            }
            yield return new WaitForSecondsRealtime(0.016f);
        }
        yield return null;
    }
}
