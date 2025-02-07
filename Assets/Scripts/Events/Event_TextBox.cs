using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class Event_TextBox : MonoBehaviour
{
    [TextArea(3, 3)]
    //public string[] originalTexts;
    public LocalizeStringEvent[] originalTexts;
    [Space(10)]

    public int currentActiveText;
    public bool startedTextBox;
    public bool startedText, endedText, endedCurrentText;

    private EventTextAction textAction;
    private LongTalkHandler longTalkHandler;

    void Start()
    {
        endedText = false;
        textAction = EventTextAction.Instance;
        ResetTextBox();
    }

    private void ResetTextBox()
    {
        currentActiveText = 0;
        startedTextBox = false;
        startedText = false;
        endedCurrentText = true;
    }

    public void StartTextBox()
    {
        textAction = EventTextAction.Instance;
        longTalkHandler = LongTalkHandler.Instance;

        if (!startedTextBox)
        {
            textAction.StartText();
            longTalkHandler.GenerateRandomImage();
            endedCurrentText = true;
            startedText = false;
            startedTextBox = true;
            longTalkHandler.talkBubbleAnim.SetBool("play", true);
            currentActiveText = 0;
            NextTextBox();
        }
    }

    private void FixedUpdate()
    {
        if (textAction.isActive)
        {
            if (textAction.nextText && endedCurrentText)
            {
                NextTextBox();
            }
        }
    }

    public void NextTextBox()
    {
        longTalkHandler.validateIcon.SetBool("play", false);

        longTalkHandler.talkBubble.text = "";

        if (currentActiveText >= originalTexts.Length)
        {
            StartCoroutine(EndTextCoroutine());
        }
        else
        {
            longTalkHandler.talkBubbleAnim.SetBool("play", true);
            endedCurrentText = false;

            if (startedText)
            {
                longTalkHandler.CrossfadeBubbles();
            }

            StartCoroutine(AnimateTextCoroutine());
        }
        startedText = true;
    }

    private IEnumerator AnimateTextCoroutine()
    {
        for (int i = 0; i < originalTexts[currentActiveText].StringReference.GetLocalizedString().ToString().Length; i++)
        {
            longTalkHandler.talkBubble.text += originalTexts[currentActiveText].StringReference.GetLocalizedString().ToString()[i];

            if (!textAction.isFastForward)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        longTalkHandler.validateIcon.SetBool("play", true);
        endedCurrentText = true;

        textAction.NextText();
        currentActiveText++;
    }

    public void EndText()
    {
        StartCoroutine(EndTextCoroutine());
    }
    private IEnumerator EndTextCoroutine()
    {
        longTalkHandler.validateIcon.SetBool("play", false);
        endedText = true;
        startedTextBox = false;
        startedText = false;
        endedCurrentText = false;
        currentActiveText = 0;

        textAction.EndText();

        longTalkHandler.talkBubble.text = "";
        longTalkHandler.talkBubbleAnim.SetBool("play", false);

        yield return new WaitForFixedUpdate();

        endedText = false;
    }
}