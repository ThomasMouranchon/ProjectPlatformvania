using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetecter : MonoBehaviour
{
    private static GroundDetecter instance = null;
    public static GroundDetecter Instance => instance;

    public bool isOnGround;
    CharacterManager characterManager;

    void Awake()
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
        if ((characterManager.groundMask.value & (1 << other.gameObject.layer)) != 0)
        {
            isOnGround = true;
        }
        else
        {
            isOnGround = false;
        }
    }
}
