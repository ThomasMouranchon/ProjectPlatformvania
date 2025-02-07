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

    private bool validate, click, back, backHold;
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

    void Update()
    {
        validate = inputReader.validate;
        click = inputReader.click;
        back = inputReader.back;
        backHold = inputReader.backHold;
    }

    private void FixedUpdate()
    {
        if (isActive && (click | validate | backHold))
        {
            if (afterValidateTimer == 0)
            {
                afterValidateTimer++;
                nextText = true;
            }

            if (backHold) isFastForward = true;
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