using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeHandler : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public void MaxValueEffect(ParticleSystem ps)
    {
        ps.Play();
    }
}