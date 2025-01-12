using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrampolinePlatform : MonoBehaviour
{
    [Header("Trampoline properties")]
    public float bounceStrength = 2.5f;

    public bool needToJump;
    public bool isKnockedDown;
    public bool hasSpikes;
    private HealthManager healthManager;

    void Start()
    {
        healthManager = HealthManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterManager characterManager = other.GetComponent<CharacterManager>();

            if ( ( ((characterManager.isOnTrampoline && !characterManager.isGrounded) | 
                (characterManager.afterLandingTimer > 0 && characterManager.afterLandingTimer < 5)) && !hasSpikes) |
                (!needToJump && isKnockedDown) )
            {
                characterManager.Bounce(bounceStrength);
                characterManager.JumpHitEffect(this.transform);
            }
        }
    }
}