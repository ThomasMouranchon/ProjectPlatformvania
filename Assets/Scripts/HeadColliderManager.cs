using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderManager : MonoBehaviour
{
    public CharacterManager characterManager;

    public float horizontalForce = 1500f;
    public float verticalForce = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Untagged")// && other.gameObject.layer == "Ground")
        {
            //Vector3 hitDirection = other.transform.position = transform.position;
            //hitDirection = hitDirection.normalized;

            /*if (collidingTimer >= 0 && collidingTimer < 120)
            {
                collidingTimer++;*/

            Vector3 direction = (other.transform.position - transform.position).normalized;
            //characterManager.Knockback((other.transform.position - this.transform.position).normalized);
            characterManager.Knockback(direction, horizontalForce, verticalForce);
            //other.GetComponent<Rigidbody>().AddForce(direction * characterManager.knockBackForce);
            /*}
            else
            {
                hitbox.enabled = true;
                collidingTimer = 0;
            }*/
        }
    }
}
