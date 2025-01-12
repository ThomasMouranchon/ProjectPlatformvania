using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPowerHit : MonoBehaviour
{
    private CharacterManager characterManager;

    public ParticleSystem[] fireBallHitEffects;
    public ParticleSystem[] electroBallHitEffects;
    public ParticleSystem[] waterBallHitEffects;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    public void MaskPowerHitEffect(GameObject hitObject, Vector3 enemyPosition)
    {
        Vector3 tr = hitObject.transform.position;

        if (hitObject.tag == "LingeringHitbox" && hitObject.name.Contains("Teleportation"))
        {
            tr = enemyPosition;
        }

        if (hitObject.name.Contains("Fire"))
        {
            Instantiate<ParticleSystem>(fireBallHitEffects[0], tr, characterManager.transform.rotation);

            if (hitObject.name.Contains("Charged")) Instantiate<ParticleSystem>(fireBallHitEffects[2], tr, characterManager.transform.rotation);
            else Instantiate<ParticleSystem>(fireBallHitEffects[1], tr, characterManager.transform.rotation);
        }
        else if (hitObject.name.Contains("Elec"))
        {
            Instantiate<ParticleSystem>(electroBallHitEffects[0], tr, characterManager.transform.rotation);

            if (hitObject.name.Contains("Charged")) Instantiate<ParticleSystem>(electroBallHitEffects[2], tr, characterManager.transform.rotation);
            else Instantiate<ParticleSystem>(electroBallHitEffects[1], tr, characterManager.transform.rotation);
        }
        else if (hitObject.name.Contains("Water"))
        {
            Instantiate<ParticleSystem>(waterBallHitEffects[0], tr, characterManager.transform.rotation);

            if (hitObject.name.Contains("Charged")) Instantiate<ParticleSystem>(waterBallHitEffects[2], tr, characterManager.transform.rotation);
            else Instantiate<ParticleSystem>(waterBallHitEffects[1], tr, characterManager.transform.rotation);
        }

        if (characterManager.cameraShake) characterManager.impulseSource.GenerateImpulse(0.5f);
    }
}
