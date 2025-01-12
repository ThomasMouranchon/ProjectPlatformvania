using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonVFXs : MonoBehaviour
{
    public MMFeedbacks ButtonActivationFeedback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClickEffects()
    {
        ButtonActivationFeedback.PlayFeedbacks();
    }
}
