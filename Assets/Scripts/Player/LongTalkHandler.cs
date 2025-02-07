using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LongTalkHandler : MonoBehaviour
{
    public static LongTalkHandler instance = null;
    public static LongTalkHandler Instance => instance;

    public TMP_Text talkBubble;
    public Animator talkBubbleAnim;
    public UIUseRandomImage talkBubbleRandomizer;
    [Space(10)]

    public Animator validateIcon;
    [Space(10)]

    public Animator speedUpAnim;

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

        validateIcon.SetBool("play", false);
        talkBubbleAnim.SetBool("main", true);
    }

    public void CrossfadeBubbles()
    {
        talkBubbleAnim.CrossFade("LongTalkBubble_Change", 0, 0);
    }

    public void GenerateRandomImage()
    {
        talkBubbleRandomizer.GenerateRandomImage();
    }
}
