using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutsceneUI : MonoBehaviour
{
    private static SkipCutsceneUI instance = null;
    public static SkipCutsceneUI Instance => instance;


    [HideInInspector] public bool canSkipCutscene;
    public bool isFastForward;
    private InputReader inputReader;
    public Animator skipInterfaceAnim;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
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
    }

    private void FixedUpdate()
    {
        if (canSkipCutscene)
        {
            isFastForward = inputReader.backHold;
            if (Time.timeScale > 0 && Time.timeScale < 3) Time.timeScale += 0.2f;
        }
        else
        {
            isFastForward = false;
            if (Time.timeScale > 1) Time.timeScale -= 0.2f;
        }

        skipInterfaceAnim.SetBool("active", isFastForward);
    }
}