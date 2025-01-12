using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventList : MonoBehaviour
{
    public bool savedEvent = false;
    public int savedEventValue = 0;
    [Space(30)]

    public bool loadSaveAtStart = false;
    public bool oneTimeEvent = false;
    [HideInInspector] public bool eventisHappening;
    private bool eventHappened = false;
    public bool reverseWhenReused = false;
    [Space(30)]

    public GameObject[] eventList;
    [Space(30)]

    public GameObject[] reversedEventList;

    private bool stopEvent;

    private CharacterManager characterManager;
    private SaveManager saveManager;

    private void Start()
    {
        saveManager = SaveManager.Instance;

        eventisHappening = false;

        if (savedEvent)
        {
            eventHappened = saveManager.activatedEvents[savedEventValue];
            if (loadSaveAtStart && eventHappened)
            {
                StartCoroutine(CallEventListCoroutine(true));
            }
        }
    }

    public void CallEventList()
    {
        // (!eventHappened && oneTimeEvent) | !oneTimeEvent
        if ((!eventHappened | !oneTimeEvent) && !eventisHappening)
        {
            StartCoroutine(CallEventListCoroutine(false));
        }
    }

    IEnumerator CallEventListCoroutine(bool loadOnStart)
    {
        if (!characterManager) characterManager = CharacterManager.Instance;

        eventisHappening = true;

        GameObject[] calledEvents = eventList;
        if (eventHappened && reverseWhenReused) calledEvents = reversedEventList;

        for (int i = 0; i < calledEvents.Length; i++)
        {
            Event_Wait event_Wait = calledEvents[i].GetComponent<Event_Wait>();
            Event_LockControls event_LockControls = calledEvents[i].GetComponent<Event_LockControls>();
            Event_UI event_UI = calledEvents[i].GetComponent<Event_UI>();
            Event_TransformGO event_TransformGO = calledEvents[i].GetComponent<Event_TransformGO>();
            Event_StopTransformGO event_StopTransformGO = calledEvents[i].GetComponent<Event_StopTransformGO>();
            Event_Particles event_Particles = calledEvents[i].GetComponent<Event_Particles>();
            Event_Destroy event_Destroy = calledEvents[i].GetComponent<Event_Destroy>();
            Event_SaveValue event_SaveValue = calledEvents[i].GetComponent<Event_SaveValue>();
            Event_ChangeGravity event_ChangeGravity = calledEvents[i].GetComponent<Event_ChangeGravity>();
            Event_BossEnum event_BossEnum = calledEvents[i].GetComponent<Event_BossEnum>();
            Event_RigidbodyConstraints event_RigidbodyConstraints = calledEvents[i].GetComponent<Event_RigidbodyConstraints>();
            NPCLongTalk npcLongTalk = calledEvents[i].GetComponent<NPCLongTalk>();
            PlayAnimation playAnimation = calledEvents[i].GetComponent<PlayAnimation>();
            FixedCamera fixedCamera = calledEvents[i].GetComponent<FixedCamera>();
            SkipCutsceneUI skipCutsceneUI = calledEvents[i].GetComponent<SkipCutsceneUI>();

            if (event_Wait)
            {
                if (!loadOnStart && (!eventHappened | !oneTimeEvent))
                {
                    if (event_Wait.waitingTime == 0) yield return new WaitForFixedUpdate();
                    else yield return new WaitForSeconds(event_Wait.waitingTime);
                }
            }
            else if (event_LockControls)
            {
                if (!loadOnStart) event_LockControls.LockControls();
            }
            else if (event_UI)
            {
                if (!loadOnStart) event_UI.ShowUI();
            }
            else if (event_TransformGO)
            {
                if (!(loadOnStart && event_TransformGO.target.GetComponent<CharacterManager>()))
                {
                    event_TransformGO.TransformGameObject(loadOnStart);
                }
            }
            else if (event_StopTransformGO)
            {
                event_StopTransformGO.StopTransformGO();
            }
            else if (event_Particles)
            {
                if (!loadOnStart) event_Particles.InstantiateParticles();
            }
            else if (event_Destroy)
            {
                event_Destroy.DestroyFunction(loadOnStart);
            }
            else if (event_SaveValue)
            {
                if (!loadOnStart) event_SaveValue.SaveValue();
            }
            else if (event_ChangeGravity)
            {
                event_ChangeGravity.ChangeGravity();
            }
            else if (event_BossEnum)
            {
                if (!loadOnStart) event_BossEnum.ChangeBossEnum();
            }
            else if (event_RigidbodyConstraints)
            {
                if (!loadOnStart) event_RigidbodyConstraints.SwitchRigidbodyConstraints();
            }
            else if (npcLongTalk)
            {
                if (!loadOnStart)
                {
                    npcLongTalk.NextLongTalk();
                    yield return new WaitForFixedUpdate();
                    while (!npcLongTalk.endedTalk)
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    npcLongTalk.endedTalk = false;
                }
            }
            else if (playAnimation)
            {
                if (!loadOnStart) playAnimation.CallAnimation();
            }
            else if (fixedCamera)
            {
                if (!loadOnStart) fixedCamera.enabled = !fixedCamera.enabled;
            }
            else if (skipCutsceneUI)
            {
                if (!loadOnStart) skipCutsceneUI.canSkipCutscene = !skipCutsceneUI.canSkipCutscene;
            }
            else
            {
                calledEvents[i].SetActive(!calledEvents[i].activeSelf);
            }

            yield return null;
        }

        eventHappened = true;
        eventisHappening = false;
        stopEvent = false;
        if (savedEvent)
        {
            saveManager.activatedEvents[savedEventValue] = eventHappened;
        }
    }
}