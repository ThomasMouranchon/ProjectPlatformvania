using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCLongTalk : MonoBehaviour
{
    public int charactersPerLine;
    [TextArea(3, 3)]
    public string[] originalTexts;
    [Space(10)]

    public int currentActiveText;
    public bool startedTalk, endedTalk, endedCurrentTalk;
    private TalkAction talkAction;
    private LongTalkHandler longTalkHandler;

    // Start is called before the first frame update
    void Start()
    {
        currentActiveText = 0;
        startedTalk = false;
        endedTalk = false;

        endedCurrentTalk = true;

        /*for (int i = 0; i < originalTexts.Length; i++)
        {
            originalTexts[i] = FormatString(originalTexts[i]);
        }*/

        longTalkHandler = LongTalkHandler.Instance;
    }
    public string FormatString(string input)
    {
        string[] words = input.Split(' ');
        string result = "";
        string line = "";

        foreach (string word in words)
        {
            if ((line.Length + word.Length + 1) > charactersPerLine)
            {
                result += line.TrimEnd() + "<br>";
                line = word + " ";
            }
            else
            {
                line += word + " ";
            }
        }

        result += line.TrimEnd();
        return result;
    }

    public void LongTalkFunction()
    {
        longTalkHandler.talkBubbleAnim.SetBool("play", true);
        currentActiveText = 0;
        longTalkHandler.talkBubbleRandomizer.GenerateRandomImage();
    }

    public void NextLongTalk()
    {
        if (!startedTalk)
        {
            longTalkHandler.talkBubbleRandomizer.GenerateRandomImage();
        }

        longTalkHandler.validateIcon.SetBool("play", false);

        longTalkHandler.talkBubble.text = "";

        if (currentActiveText >= originalTexts.Length)
        {
            StartCoroutine(EndTalkCoroutine());
        }
        else
        {
            longTalkHandler.talkBubbleAnim.SetBool("play", true);
            endedCurrentTalk = false;

            if (startedTalk)
            {
                longTalkHandler.talkBubbleAnim.CrossFade("LongTalkBubble_Change", 0);
            }

            StartCoroutine(AnimateTalkCoroutine());
        }

        startedTalk = true;
    }

    private IEnumerator AnimateTalkCoroutine()
    {
        talkAction = TalkAction.Instance;
        
        for (int i = 0; i < originalTexts[currentActiveText].Length; i++)
        {
            longTalkHandler.talkBubble.text += originalTexts[currentActiveText][i];

            if (!talkAction.isFastForward)
            {
                yield return new WaitForFixedUpdate();
            }
        }
        longTalkHandler.validateIcon.SetBool("play", true);
        endedCurrentTalk = true;
        currentActiveText++;
    }

    public void EndTalk()
    {
        StartCoroutine(EndTalkCoroutine());
        /*
         * longTalkHandler.validateIcon.SetBool("play", false);
        endedTalk = true;
        currentActiveText = 0;
        longTalkHandler.talkBubbleAnim.SetBool("play", false);

        longTalkHandler.talkBubble.text = "";

        endedTalk = false;
        startedTalk = false;
        */
    }
    private IEnumerator EndTalkCoroutine()
    {
        longTalkHandler.validateIcon.SetBool("play", false);
        endedTalk = true;
        startedTalk = false;
        currentActiveText = 0;

        longTalkHandler.talkBubble.text = "";
        longTalkHandler.talkBubbleAnim.SetBool("play", false);

        //yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(0.1f);
        endedTalk = false;

    }
}
