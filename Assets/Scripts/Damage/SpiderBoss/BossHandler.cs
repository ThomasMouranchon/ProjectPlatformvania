using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandler : MonoBehaviour
{
    public enum Boss_Position
    {
        OnGround,
        OnWall,
        OnCeiling
    }

    public enum Boss_Health
    {
        Full,
        Damaged,
        Dead
    }

    public enum Boss_Animation
    {
        Neutral,
        Move,
        Attack,
        Exposed
    }

    public Boss_Position positionEnum;
    public Boss_Health healthEnum;
    public Boss_Animation animationEnum;
    [Space(10)]

    public Animator animator;
    [Space(10)]

    public int health;
    public float walkSpeed, runSpeed;
    public float rotationSpeed;
    [Space(10)]

    public GameObject projectile;
    public float projectileSpeed;
    [Space(10)]

    public EventList startEvent;
    public EventList[] groundEvents;
    public EventList[] wallEvents;
    public EventList[] ceilingEvents;
    public EventList damagedEvent;
    public EventList deathEvent;

    private int nextGroundAttack, nextWallAttack, nextCeilingAttack;

    public CharacterManager characterManager;

    void Start()
    {
        characterManager = CharacterManager.Instance;
        healthEnum = Boss_Health.Full;
        startEvent.CallEventList();
    }

    void FixedUpdate()
    {
        if (characterManager == null)
        {
            animationEnum = Boss_Animation.Neutral;
        }
        else
        {
            //bool callEvent = CheckEventLists(groundEvents);

            //if (callEvent) callEvent = CheckEventLists(wallEvents);

            //if (callEvent) callEvent = CheckEventLists(ceilingEvents);
            
            if (CheckEventLists(groundEvents) && CheckEventLists(wallEvents) && 
                CheckEventLists(ceilingEvents) && animationEnum == Boss_Animation.Neutral)
            {
                //animationEnum = Boss_Animation.Move;
                CallAction();
            }
            else
            {
                UpdateState();
            }
        }

        //if (animationEnum = SpiderBoss_Animation.Neutral)
    }

    void CallAction()
    {
        switch (positionEnum)
        {
            default:
                groundEvents[nextGroundAttack].CallEventList();

                if (nextGroundAttack == groundEvents.Length - 1) nextGroundAttack = 0;
                else nextGroundAttack++;

                break;

            case Boss_Position.OnWall:
                wallEvents[nextWallAttack].CallEventList();

                if (nextWallAttack == wallEvents.Length - 1) nextWallAttack = 0;
                else nextWallAttack++;

                break;

            case Boss_Position.OnCeiling:
                ceilingEvents[nextCeilingAttack].CallEventList();

                if (nextCeilingAttack == ceilingEvents.Length - 1) nextCeilingAttack = 0;
                else nextCeilingAttack++;

                break;
        }
    }

    void UpdateState()
    {
        //switch

    }

    public void HitPillar()
    {
        if (animationEnum == Boss_Animation.Attack)
        {
            animationEnum = Boss_Animation.Exposed;
        }
        else
        {
            animationEnum = Boss_Animation.Neutral;
        }
    }

    private bool CheckEventLists(EventList[] events)
    {
        foreach (EventList eventList in events)
        {
            if (eventList.eventIsHappening) return false;
        }
        return true;
    }

    public void Damaged(int damageValue)
    {
        health -= damageValue;

        if (health <= 0)
        {
            healthEnum = Boss_Health.Dead;
            deathEvent.CallEventList();
        }
        else
        {
            healthEnum = Boss_Health.Damaged;
            damagedEvent.CallEventList();
        }
    }
}
