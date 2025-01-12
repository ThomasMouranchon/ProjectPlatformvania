using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrampoline : MonoBehaviour
{
    [Header("Trampoline properties")]
    public float bounceStrength = 80;
    public NPCIdleHandler npcIdleHandler;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterManager characterManager = other.GetComponent<CharacterManager>();

            if ((!characterManager.isGrounded && characterManager.airbornedTimer > 10) | (characterManager.afterLandingTimer > 0 && characterManager.afterLandingTimer < 5))
            {
                characterManager.Bounce(bounceStrength);
                characterManager.JumpHitEffect(this.transform);
                npcIdleHandler.HurtFunction();
            }
        }
    }
}
