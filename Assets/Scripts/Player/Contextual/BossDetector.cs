using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDetector : MonoBehaviour
{
    private static BossDetector instance = null;
    public static BossDetector Instance => instance;

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
        characterManager = CharacterManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            UpdateBossBool(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boss")
        {
            UpdateBossBool(false);
        }
    }

    public void UpdateBossBool(bool state)
    {
        characterManager.soulAnim.SetBool("isCloseToBoss", state);
    }
}
