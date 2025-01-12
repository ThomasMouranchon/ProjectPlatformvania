using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardDamage : MonoBehaviour
{
    public int damageToGive = 1;
    [Tooltip("To use only for bosses and certains enemies")]

    [Header("Knockback Specifics")]
    public bool applyKnockback;
    public float knockBackForceHorizontal = 1500f;
    public float knockBackForceVertical = 1000f;
    [Space(10)]

    private HealthManager healthManager;
    public TrampolinePlatform trampolinePlatform;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = HealthManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterManager characterManager = CharacterManager.Instance;

            healthManager.HurtPlayer();

            if (applyKnockback)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                characterManager.Knockback(direction, knockBackForceHorizontal, knockBackForceVertical);
            }
        }
    }
}
