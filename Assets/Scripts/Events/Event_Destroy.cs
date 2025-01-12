using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event_Destroy : MonoBehaviour
{
    public PlayAnimation playAnimation;
    [Space(10)]

    public Collider collider;
    [Space(10)]

    public ParticleSystem[] destroyEffects;
    public float delayBeforeDestroy;

    public void DestroyFunction(bool instant)
    {
        if (instant) Destroy(gameObject);
        else StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine()
    {
        if (playAnimation) playAnimation.CallAnimation();

        foreach (ParticleSystem effect in destroyEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }

        if (collider) collider.enabled = false;

        yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);
    }
}