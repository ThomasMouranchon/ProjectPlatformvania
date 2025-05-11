using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTextAction : MonoBehaviour
{
    private static EventTextAction instance = null;
    public static EventTextAction Instance => instance;

    public bool isActive, nextText;
    [Space(5)]

    public Animator fastForwardAnim;
    public bool isFastForward;
    public int afterValidateTimer;

    private InputReader inputReader;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        inputReader = InputReader.Instance;
        EndText();
    }

    private void FixedUpdate()
    {
        if (isActive && (inputReader.click | inputReader.validate | inputReader.backHold))
        {
            if (afterValidateTimer == 0)
            {
                afterValidateTimer++;
                nextText = true;
            }

            if (inputReader.backHold) isFastForward = true;
            else isFastForward = false;
        }
        else
        {
            isFastForward = false;
        }

        //fastForwardAnim.SetBool("active", isFastForward | TalkAction.Instance.isFastForward);
        fastForwardAnim.SetBool("active", isFastForward);

        if (afterValidateTimer > 0 && afterValidateTimer < 15) afterValidateTimer++;
        else
        {
            afterValidateTimer = 0;
            nextText = false;
        }
    }

    public void StartText()
    {
        if (!isActive)
        {
            isActive = true;

            nextText = true;
        }
    }

    public void NextText()
    {
        nextText = false;
        afterValidateTimer = 1;
    }

    public void EndText()
    {
        NextText();
        isActive = false;
    }
}