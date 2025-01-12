using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LongTalkHandler : MonoBehaviour
{
    public static LongTalkHandler instance = null;
    public static LongTalkHandler Instance => instance;

    public TMP_Text talkBubble;
    public Animator talkBubbleAnim, validateIcon;
    public UIUseRandomImage talkBubbleRandomizer;

    void Awake()
    {
        instance = this;

        validateIcon.SetBool("play", false);
    }
}
