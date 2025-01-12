using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarVFXChanger : MonoBehaviour
{
    public Color spriteColor;
    public float alphaMultiplier;
    public Quaternion rotationMultiplier;
    //public int[] thresholdList;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
        
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);

        if (dist > 600) ChangePlayerAlpha(dist * 0.00006f * alphaMultiplier);
        else if (dist > 500) ChangePlayerAlpha(dist * 0.00005f * alphaMultiplier);
        else if (dist > 400) ChangePlayerAlpha(dist * 0.00004f * alphaMultiplier);
        else if (dist > 300) ChangePlayerAlpha(dist * 0.00003f * alphaMultiplier);
        else if (dist > 200) ChangePlayerAlpha(dist * 0.00002f * alphaMultiplier);
        else if (dist > 100) ChangePlayerAlpha(dist * 0.000015f * alphaMultiplier);
        else ChangePlayerAlpha(dist * 0.00001f * alphaMultiplier);
        //else ChangePlayerAlpha(0);
    }

    private void ChangePlayerAlpha(float value)
    {
        spriteColor.a = value;
        GetComponent<SpriteRenderer>().color = spriteColor;
        //kiteWoodTransparent.SetFloat("_Alpha", value);
    }
}
