using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHandler : MonoBehaviour
{
    private static BombHandler instance = null;
    public static BombHandler Instance => instance;

    public int bombTimerLimit = 600;
    public int currentBombTimer;

    public ParticleSystem[] explosionEffects;
    public GameObject explosionHitbox;

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
        currentBombTimer = 0;
    }

    private void FixedUpdate()
    {
        if (currentBombTimer < bombTimerLimit) currentBombTimer++;
        else Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Yoyo"))
        {
            gameObject.transform.position = YoyoHandler.Instance.transform.position;
        }
    }

    void OnDestroy()
    {
        foreach (ParticleSystem effect in explosionEffects)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }
        Instantiate(explosionHitbox, transform.position, Quaternion.identity);
    }
}