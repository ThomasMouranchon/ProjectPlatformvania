using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchAnimation : MonoBehaviour
{
    public GameObject soul, soulVfx;
    [Space(10)]
    public GameObject kite;

    public void DoStretch(string coroutineName)
    {
        StartCoroutine(coroutineName);
    }

    IEnumerator StretchJumpAnimation()
    {
        void DoPositiveStuff()
        {
            //soul.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale += new Vector3(-0.03f, 0.05f, -0.03f);
            //kite.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
            kite.transform.position += new Vector3(0, 0.15f, 0);
        }

        void DoNegativeStuff()
        {
            //soul.transform.localScale -= new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale -= new Vector3(-0.03f, 0.05f, -0.03f);

            kite.transform.position -= new Vector3(0, 0.15f, 0);
        }

        for (int i = 0; i < 10; i++)
        {
            DoPositiveStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 10; i++)
        {
            DoNegativeStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator StretchBounceAnimation()
    {
        void DoPositiveStuff()
        {
            //soul.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale += new Vector3(-0.03f, 0.05f, -0.03f);
            //kite.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
            kite.transform.position += new Vector3(0, 0.15f, 0);
        }

        void DoNegativeStuff()
        {
            //soul.transform.localScale -= new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale -= new Vector3(-0.03f, 0.05f, -0.03f);

            kite.transform.position -= new Vector3(0, 0.15f, 0);
        }

        for (int i = 0; i < 8; i++)
        {
            DoPositiveStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 8; i++)
        {
            DoNegativeStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
    }
    
    
    IEnumerator StretchLedgeJumpAnimation()
    {
        void DoPositiveStuff()
        {
            //soul.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale += new Vector3(-0.01f, 0.05f, -0.01f);
        }

        void DoNegativeStuff()
        {
            //soul.transform.localScale -= new Vector3(-0.01f, 0.05f, -0.01f);
            soulVfx.transform.localScale -= new Vector3(-0.01f, 0.05f, -0.01f);
        }

        for (int i = 0; i < 10; i++)
        {
            DoPositiveStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 10; i++)
        {
            DoNegativeStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator StretchLandAnimation()
    {
        void DoPositiveStuff()
        {
            //soul.transform.localScale += new Vector3(-0.01f, 0.04f, -0.01f);
            soulVfx.transform.localScale += new Vector3(-0.01f, 0.04f, -0.01f);
        }

        void DoNegativeStuff()
        {
            //soul.transform.localScale -= new Vector3(-0.01f, 0.04f, -0.01f);
            soulVfx.transform.localScale -= new Vector3(-0.01f, 0.04f, -0.01f);
        }

        for (int i = 0; i < 3; i++)
        {
            DoNegativeStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
        for (int i = 0; i < 3; i++)
        {
            DoPositiveStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator StretchDiveLandAnimation()
    {
        void DoPositiveStuff()
        {
            //soul.transform.localScale += new Vector3(-0.01f, 0.03f, -0.01f);
            soulVfx.transform.localScale += new Vector3(-0.01f, 0.03f, -0.01f);
        }

        void DoNegativeStuff()
        {
            //soul.transform.localScale -= new Vector3(-0.01f, 0.03f, -0.01f);
            soulVfx.transform.localScale -= new Vector3(-0.01f, 0.03f, -0.01f);
        }

        for (int i = 0; i < 16; i++)
        {
            DoNegativeStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.0125f);
        }
        for (int i = 0; i < 16; i++)
        {
            DoPositiveStuff();
            yield return new WaitForFixedUpdate();
            //yield return new WaitForSeconds(0.0125f);
        }
    }

    IEnumerator StretchStartDive()
    {
        void DoPositiveStuff()
        {
            soul.transform.localScale += new Vector3(-0.02f, 0.06f, -0.02f);
        }

        for (int i = 0; i < 8; i++)
        {
            DoPositiveStuff();
            yield return new WaitForSeconds(0.025f);
        }
    }
    IEnumerator StretchStopDive()
    {
        void DoNegativeStuff()
        {
            soul.transform.localScale -= new Vector3(-0.02f, 0.06f, -0.02f);
        }

        for (int i = 0; i < 8; i++)
        {
            DoNegativeStuff();
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator StretchDashAnimation()
    {
        soul.transform.localScale += new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale += new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale += new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale += new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        /*characterModel.transform.localScale += new Vector3(0, 0.08f, 0);
        yield return new WaitForSeconds(0.05f);*/
        soul.transform.localScale -= new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale -= new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale -= new Vector3(0.08f, -0.02f, 0.08f);
        yield return new WaitForSeconds(0.05f);
        soul.transform.localScale -= new Vector3(0.08f, -0.02f, 0.08f);
        /*yield return new WaitForSeconds(0.05f);
        characterModel.transform.localScale -= new Vector3(0, 0.08f, 0);*/
    }
}
