using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Normal,
    Confirm,
    GroundPound,
    Kick,
    Fire,
    Electricity,
    Water,
    Projectile
}

public enum ButtonType
{
    DestroyOnHit,
    Spammable,
    OnOff
}

public class InteractableButton : MonoBehaviour
{
    public Collider buttonCollider;
    [Space(10)]
    public Animator buttonAnim;
    public bool isInteractable = true;
    [Space(5)]
    public InteractionType interactionType;
    [Space(5)]
    public ButtonType buttonType;
    [Space(5)]

    public ParticleSystem[] hitEffects;
    [Space(10)]

    public PlayAnimation playAnimation;
    public Event_Destroy event_Destroy;
    [Space(50)]

    public EventList eventList;
    [Space(50)]

    public float timerUntilHitsReset = 120;
    private float timerUntilHitsResetLimit;
    public int amountOfHitsTaken;
    private int amountOfHitsLimit;

    void Start()
    {
        amountOfHitsLimit = amountOfHitsTaken;
        amountOfHitsTaken = 0;

        timerUntilHitsResetLimit = timerUntilHitsReset;
        timerUntilHitsReset = 0;
    }

    void FixedUpdate()
    {
        if (amountOfHitsTaken > 0 && amountOfHitsTaken < amountOfHitsLimit && timerUntilHitsReset < timerUntilHitsResetLimit)
        {
            timerUntilHitsReset++;
        }
        else if (amountOfHitsTaken < amountOfHitsLimit)
        {
            amountOfHitsTaken = 0;
            timerUntilHitsReset = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isInteractable)
        {
            switch (interactionType)
            {
                case InteractionType.Normal:
                    if (other.collider.tag != "Player")
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.GroundPound:
                    if (CharacterManager.Instance.afterDiveLandingTimer > 0 &&
                     CharacterManager.Instance.afterDiveLandingTimer < 5)
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.Kick:
                    if (other.collider.tag == "Counter")
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.Fire: //Layer Fireball
                    if (other.gameObject.layer == 28)
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.Electricity: //Layer Electroball
                    if (other.gameObject.layer == 29)
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.Water: //Layer Waterball
                    if (other.gameObject.layer == 30)
                    {
                        CollisionEffect();
                    }
                    break;
                case InteractionType.Projectile:
                    if (other.gameObject.layer >= 28 | other.collider.tag == "GrabbableItem")
                    {
                        CollisionEffect();
                    }
                    break;
            }
        }
    }

    public void CollisionEffect()
    {
        playAnimation.CallAnimation();
        foreach (ParticleSystem effect in hitEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }

        switch (buttonType)
        {
            case ButtonType.DestroyOnHit:
                event_Destroy.DestroyFunction(false);
                break;
            case ButtonType.Spammable:
                amountOfHitsTaken++;
                timerUntilHitsReset = 0;
                isInteractable = (amountOfHitsTaken >= amountOfHitsLimit);
                break;
            case ButtonType.OnOff:
                isInteractable = false;
                break;
        }

        if (buttonType != ButtonType.Spammable | amountOfHitsTaken >= amountOfHitsLimit)
        {
            eventList.CallEventList();

            buttonCollider.enabled = isInteractable;

            //buttonAnim.SetBool("isInteractable", isInteractable);
        }
    }
}