using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoulEyesHandler : MonoBehaviour
{
    public AnimUpdater animUpdater;
    private Animator eyeAnim;
    public int blinkProbability = 120, lookAroundProbability = 360;
    public CharacterManager characterManager;
    [HideInInspector]
    public bool scaledTime;

    private void Awake()
    {
        eyeAnim = animUpdater.eyeAnim;
        scaledTime = true;
    }

    private void Update()
    {
        if (!scaledTime)
        {
            if (Random.Range(0, blinkProbability * 2) == 1) StartCoroutine(blinkCoroutine());
            else if (Random.Range(0, lookAroundProbability * 2) == 1 && characterManager.isGrounded &&
                (characterManager.idleValue < 2 | (characterManager.idleValue >= 3 && characterManager.idleValue < 11))) StartCoroutine(lookAroundCoroutine());
        }
    }

    void FixedUpdate()
    {
        if (scaledTime)
        {
            if (Random.Range(0, blinkProbability) == 1) StartCoroutine(blinkCoroutine());
            else if (Random.Range(0, lookAroundProbability) == 1 && characterManager.isGrounded &&
                (characterManager.idleValue < 2 | (characterManager.idleValue >= 3 && characterManager.idleValue < 11))) StartCoroutine(lookAroundCoroutine());
        }
    }

    public void ChangeEyeColor(int transformationValue)
    {
        eyeAnim.SetInteger("transformationValue", transformationValue);
    }

    IEnumerator blinkCoroutine()
    {
        eyeAnim.SetBool("blink", true);

        if (scaledTime) yield return new WaitForSeconds(0.2f);
        else yield return new WaitForSecondsRealtime(0.4f);

        eyeAnim.SetBool("blink", false);
    }

    IEnumerator lookAroundCoroutine()
    {
        int rand;
        rand = Random.Range(1, 6);

        eyeAnim.SetInteger("lookAround", rand);

        if (scaledTime)
        {
            if (rand == 1 | rand == 3 | rand == 4 | rand == 5) yield return new WaitForSeconds(1.5f);
            else if (rand == 2) yield return new WaitForSeconds(2);
            else yield return new WaitForSeconds(1);
        }
        else
        {
            if (rand == 1 | rand == 3 | rand == 4 | rand == 5) yield return new WaitForSecondsRealtime(3);
            else if (rand == 2) yield return new WaitForSecondsRealtime(4);
            else yield return new WaitForSecondsRealtime(2);
        }

        eyeAnim.SetInteger("lookAround", 0);
    }
}
