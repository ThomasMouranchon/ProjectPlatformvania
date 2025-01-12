using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;
    public NoiseSettingsPropertyAttribute cinemachineMultiChaPerlin;
    /*CinemachineComposer comp0;
    CinemachineComposer comp1;
    CinemachineComposer comp2;*/

    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook = GetComponent<CinemachineFreeLook>();//.GetComponent<CinemachineFramingTransposer>();

        //cinemachineMultiChaPerlin = m_vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();

        //cinemachineMultiChaPerlin = cinemachineFreeLook.GetComponent<CinemachineBasicMultiChannelPerlin>();

        //comp0 = cinemachineFreeLook.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        /*comp1 = cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        comp2 = cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera()
    {
        StartCoroutine(_ProcessShake());
    }

    IEnumerator _ProcessShake()
    {
        float shakeIntensity = 20;
        float shakeTiming = 1;

        Noise(1, shakeIntensity);
        Debug.Log("hello");
        yield return new WaitForSeconds(shakeTiming);
        Noise(0, 0);
    }

    private void Noise(float amplitudeGain, float frequencyGain)
    {
        //cinemachineFreeLook.GetComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitudeGain;

        //cinemachineMultiChaPerlin.m_AmplitudeGain = amplitudeGain;

        /*cinemachineFreeLook.middleRig.Noise.m_AmplitudeGain = amplitudeGain;
        cinemachineFreeLook.bottomRig.Noise.m_AmplitudeGain = amplitudeGain;*/

        //cinemachineMultiChaPerlin.m_FrequencyGain = frequencyGain;

        /*cinemachineFreeLook.middleRig.Noise.m_FrequencyGain = frequencyGain;
        cinemachineFreeLook.bottomRig.Noise.m_FrequencyGain = frequencyGain;*/

    }
}
