using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteHandler : MonoBehaviour
{
    public GameObject kite;
    public SkinnedMeshRenderer[] kiteRenderer;
    public MeshRenderer[] kiteRendererStructure;
    [Space(10)]
    public Transform handsReference;
    [Space(10)]
    public TrailRenderer[] kiteTrails;

    private Vector3 kitePositionChange = new Vector3(0, 0, 0);
    private Vector3 kiteRotationChange = new Vector3(0, 0, 15);
    private Vector3 kiteScaleChange = new Vector3(0.2f, 0.2f, 0.2f);

    void Start()
    {
        CloseGliderInstantly();
    }

    private void UpdateKiteTrailsLength(float length)
    {
        for (int i = 0; i < kiteTrails.Length; i++)
        {
            kiteTrails[i].time = length;
        }
    }

    public void OpenGlider()
    {
        for (int i = 0; i < kiteRenderer.Length; i++)
        {
            kiteRenderer[i].enabled = true;
        }
        for (int i = 0; i < kiteRendererStructure.Length; i++)
        {
            kiteRendererStructure[i].enabled = true;
        }
        StartCoroutine(AnimOpenGliderCoroutine());
        for (int i = 0; i < kiteTrails.Length; i++)
        {
            kiteTrails[i].enabled = true;
        }
        StartCoroutine(OpenGliderCoroutine());
    }

    IEnumerator AnimOpenGliderCoroutine()
    {
        while (kite.transform.localScale.y < 1)
        {
            kite.transform.position += kitePositionChange;
            kite.transform.Rotate(kiteRotationChange);
            kite.transform.localScale += kiteScaleChange;
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    IEnumerator OpenGliderCoroutine()
    {
        for (float i = 0; i < 10; i++)
        {
            if (kiteRenderer[0].enabled) yield return new WaitForSeconds(0.1f);
            else yield break;
        }
        for (float i = 0; i < 0.4f; i = i + 0.02f)
        {
            UpdateKiteTrailsLength(i);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void CloseGlider()
    {
        StartCoroutine(CloseGliderCoroutine());
    }

    IEnumerator AnimCloseGliderCoroutine()
    {
        while (kite.transform.localScale.y > 0)
        {
            kite.transform.position -= kitePositionChange;
            kite.transform.Rotate(-kiteRotationChange);
            kite.transform.localScale -= kiteScaleChange;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator CloseGliderCoroutine()
    {
        StartCoroutine(AnimCloseGliderCoroutine());
        yield return new WaitForSeconds(0.3f);
        /*if (!kiteAnim.GetCurrentAnimatorStateInfo(0).IsName("Kite_IsOff") )
        {
            yield break;
        }*/
        for (int i = 0; i < kiteRenderer.Length; i++)
        {
            kiteRenderer[i].enabled = false;
        }
        for (int i = 0; i < kiteRendererStructure.Length; i++)
        {
            kiteRendererStructure[i].enabled = false;
        }

        for (int i = 0; i < kiteTrails.Length; i++)
        {
            kiteTrails[i].enabled = true;
        }
    }

    public void CloseGliderInstantly()
    {
        for (int i = 0; i < kiteRenderer.Length; i++)
        {
            kiteRenderer[i].enabled = false;
        }
        for (int i = 0; i < kiteRendererStructure.Length; i++)
        {
            kiteRendererStructure[i].enabled = false;
        }
    }
}
