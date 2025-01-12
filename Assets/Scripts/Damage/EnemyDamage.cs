using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damageToGive = 1;
    //private bool isColliding = false;
    /*private int collidingTimer;
    Collider hitbox;*/
    [Tooltip("To use only for bosses and certains enemies")]

    [Header("Knockback Specifics")]
    public bool applyKnockback;
    public float knockBackForceHorizontal = 1500f;
    public float knockBackForceVertical = 1000f;
    [Space(10)]

    private HealthManager healthManager;
    public TrampolinePlatform trampolinePlatform;

    void Start()
    {
        healthManager = HealthManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterManager characterManager = other.GetComponent<CharacterManager>();
            TalkAction talkAction = characterManager.talkAction;

            if (trampolinePlatform)
            {
                if (!((!(characterManager.isGrounded) || (characterManager.afterLandingTimer > 0 && characterManager.afterLandingTimer < 5)) &&
                    !trampolinePlatform.hasSpikes) || (!trampolinePlatform.needToJump && trampolinePlatform.isKnockedDown))
                
                if (!((!characterManager.isGrounded || (characterManager.afterLandingTimer > 0 && characterManager.afterLandingTimer < 5)) && !trampolinePlatform.hasSpikes
                        || (!trampolinePlatform.needToJump && trampolinePlatform.isKnockedDown)))
                
                {
                    Move_Throws move_Throws = Move_Throws.Instance;
                    Move_PhantomDash move_PhantomDash = Move_PhantomDash.Instance;
                    if (!talkAction.isTalking && !move_PhantomDash.isPhantomDashing && (!move_Throws.isGettingDragged | move_Throws.isHoldingToRope))
                    {
                        characterManager.healthManager.HurtPlayer();
                    }
                }
            }
            else
            {
                if (!(!characterManager.isGrounded || (characterManager.afterLandingTimer > 0 && characterManager.afterLandingTimer < 5)))
                {
                    Move_Throws move_Throws = Move_Throws.Instance;
                    if (!talkAction.isTalking && (!move_Throws.isGettingDragged | move_Throws.isHoldingToRope))
                    {
                        characterManager.healthManager.HurtPlayer();
                    }
                }
            }

            if (applyKnockback)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                characterManager.Knockback(direction, knockBackForceHorizontal, knockBackForceVertical);
            }
        }
    }
}
