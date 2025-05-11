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
    public bool eventIsHappening;
    private bool eventHappened = false;
    public bool reverseWhenReused = false;
    [Space(30)]

    public GameObject[] eventList;
    [Space(30)]

    public GameObject[] reversedEventList;

    private Component targetedEvent;

    private CharacterManager characterManager;
    private SaveManager saveManager;

    private void Start()
    {
        saveManager = SaveManager.Instance;

        eventIsHappening = false;

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
        if ((!eventHappened | !oneTimeEvent) && !eventIsHappening)
        {
            StartCoroutine(CallEventListCoroutine(false));
        }
    }

    IEnumerator CallEventListCoroutine(bool loadOnStart)
    {
        if (!characterManager) characterManager = CharacterManager.Instance;

        eventIsHappening = true;

        GameObject[] calledEvents = eventList;
        if (eventHappened && reverseWhenReused) calledEvents = reversedEventList;

        for (int i = 0; i < calledEvents.Length; i++)
        {
            if (eventIsHappening)
            {
                targetedEvent = calledEvents[i].GetComponent<MonoBehaviour>();
                switch (targetedEvent)
                {
                    case Event_Wait event_Wait:
                        if (!loadOnStart && (!eventHappened | !oneTimeEvent))
                        {
                            if (event_Wait.waitingTime == 0) yield return new WaitForFixedUpdate();
                            else yield return new WaitForSeconds(event_Wait.waitingTime);
                        }
                        break;
                    case Event_LockControls event_LockControls:
                        if (!loadOnStart) event_LockControls.LockControls();
                        break;
                    case Event_UnlockControls event_UnlockControls:
                        if (!loadOnStart) event_UnlockControls.UnlockControls();
                        break;
                    case Event_UI event_UI:
                        if (!loadOnStart) event_UI.ShowUI();
                        break;
                    case Event_TransformGO event_TransformGO:
                        if (!(loadOnStart && event_TransformGO.target.GetComponent<CharacterManager>()))
                        {
                            event_TransformGO.TransformGameObject(loadOnStart);
                        }
                        break;
                    case Event_StopTransformGO event_StopTransformGO:
                        if (!loadOnStart) event_StopTransformGO.StopTransformGO();
                        break;
                    case Event_Particles event_Particles:
                        if (!loadOnStart) event_Particles.InstantiateParticles();
                        break;
                    case Event_ClearParticles event_ClearParticles:
                        if (!loadOnStart) event_ClearParticles.ClearParticles();
                        break;
                    case Event_Cameras event_Cameras:
                        if (!loadOnStart) event_Cameras.SwitchToNextCamera();
                        break;
                    case Event_TextBox event_TextBox:
                        if (!loadOnStart)
                        {
                            event_TextBox.StartTextBox();
                            while (!event_TextBox.endedText)
                            {
                                yield return new WaitForFixedUpdate();
                            }
                        }
                        break;
                    case Event_Destroy event_Destroy:
                        event_Destroy.DestroyFunction(loadOnStart);
                        break;
                    case Event_SaveValue event_SaveValue:
                        if (!loadOnStart) event_SaveValue.SaveValue();
                        break;
                    case Event_ChangeGravity event_ChangeGravity:
                        event_ChangeGravity.ChangeGravity();
                        break;
                    case Event_BossEnum event_BossEnum:
                        if (!loadOnStart) event_BossEnum.ChangeBossEnum();
                        break;
                    case Event_RigidbodyConstraints event_RigidbodyConstraints:
                        if (!loadOnStart) event_RigidbodyConstraints.SwitchRigidbodyConstraints();
                        break;
                    case Event_UnlockItem event_UnlockItem:
                        if (!loadOnStart) event_UnlockItem.UnlockItem();
                        break;
                    case Event_StartRailMovement event_StartRailMovement:
                        if (!loadOnStart) event_StartRailMovement.StartRailMovement();
                        break;
                    case PlayAnimation playAnimation:
                        if (!(loadOnStart && calledEvents[i].GetComponent<PlayerAnimatorTarget>()))
                        {
                            playAnimation.CallAnimation();
                        }
                        break;
                    case FixedCamera fixedCamera:
                        if (!loadOnStart) fixedCamera.enabled = !fixedCamera.enabled;
                        break;
                    case SkipCutsceneUI skipCutsceneUI:
                        if (!loadOnStart) skipCutsceneUI.canSkipCutscene = !skipCutsceneUI.canSkipCutscene;
                        break;
                    default:
                        calledEvents[i].SetActive(!calledEvents[i].activeSelf);
                        break;
                }

                yield return null;
            }
            else
            {
                StopEvent();
                break;
            }
        }

        EventEnd();
    }

    public void StopEvent()
    {
        StopAllCoroutines();
        EventEnd();
    }

    private void EventEnd()
    {
        eventHappened = true;
        eventIsHappening = false;
        if (savedEvent)
        {
            saveManager.activatedEvents[savedEventValue] = eventHappened;
        }
    }

    private void OnDestroy()
    {
        EventEnd();
    }

    private void OnDisable()
    {
        EventEnd();
    }
}