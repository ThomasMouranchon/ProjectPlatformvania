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

    public CharacterManager characterManager;

    void Start()
    {
        characterManager = CharacterManager.Instance;
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
        int rand = Random.Range(0, groundEvents.Length);

        switch (positionEnum)
        {
            default:
                /*if (CheckEventLists(groundEvents))
                {*/
                    groundEvents[rand].CallEventList();
                    Debug.Log("aaa");
                //}
                break;
            case Boss_Position.OnWall:
                /*if (CheckEventLists(wallEvents))
                {*/
                    rand = Random.Range(0, wallEvents.Length);
                    wallEvents[rand].CallEventList();
                //}
                break;
            case Boss_Position.OnCeiling:
                /*if (CheckEventLists(ceilingEvents))
                {*/
                    rand = Random.Range(0, ceilingEvents.Length);
                    ceilingEvents[rand].CallEventList();
                //}
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
            if (eventList.eventisHappening) return false;
        }
        return true;
    }
}
