using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Move_PhantomDash : MonoBehaviour
{
    private static Move_PhantomDash instance = null;
    public static Move_PhantomDash Instance => instance;


    public bool canUsePhantomDash, isPhantomDashing;
    public int phantomDashTimerLimit = 120;
    private int phantomDashTimer;
    [Space(30)]

    public GaugeHandler phantomDashGauge;
    public ParticleSystem phantomDashIsReadyEffect;
    private Animator phantomDashGaugeAnimator;
    [Space(30)]

    public ParticleSystem[] phantomDashEffects;

    private ItemsHandler itemsHandler;
    private CharacterManager characterManager;

    private void Awake()
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
    }

    private void Start()
    {
        itemsHandler = ItemsHandler.Instance;
        characterManager = CharacterManager.Instance;

        phantomDashTimer = phantomDashTimerLimit;

        phantomDashGaugeAnimator = phantomDashGauge.gameObject.GetComponent<Animator>();
        phantomDashGauge.SetMaxValue(phantomDashTimerLimit);
    }

    private void FixedUpdate()
    {
        if (phantomDashTimer < phantomDashTimerLimit)
        {
            phantomDashTimer++;
            canUsePhantomDash = false;
        }
        else
        {
            if (canUsePhantomDash && phantomDashTimer == phantomDashTimerLimit)
            {
                phantomDashTimer++;
                //phantomDashGauge.MaxValueEffect(phantomDashIsReadyEffect);
            }
            canUsePhantomDash = true;
            isPhantomDashing = false;
        }

        if (!characterManager.isDashing)
        {
            isPhantomDashing = false;
        }

        phantomDashGauge.SetValue(phantomDashTimer);

        UpdateAnimator();
    }

    public void UpdateAnimator()
    {
        if (itemsHandler.enablePhantomDash)
        {
            phantomDashGaugeAnimator.SetBool("play", !canUsePhantomDash);
        }
        else
        {
            phantomDashGaugeAnimator.SetBool("play", false);
        }
    }

    public void PhantomDash()
    {
        if (canUsePhantomDash && itemsHandler.enablePhantomDash)
        {
            phantomDashTimer = 0;
            isPhantomDashing = true;

            foreach (ParticleSystem effect in phantomDashEffects)
            {
                Instantiate(effect, transform.position, transform.rotation);
            }
        }
    }
}