using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public class WaterBall : MonoBehaviour
{
    public SphereCollider m_Collider;
    private float initialRadius;
    [Space(10)]

    public GameObject[] beforeDestroyEffects;
    [Space(10)]

    public GameObject[] destroyEffects;
    [Space(10)]
    public ParticleSystem ashEffect;
    public GameObject lingeringHitbox;

    private int particleTimer; // For fire particle effect
    private int activateGravityTimer;
    public Rigidbody rigidbody;
    private Vector3 gravity = Vector3.zero;
    private bool isCharged;
    private bool isDestroyed;
    public GameObject meshObject;

    private CharacterManager characterManager;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
        rigidbody = GetComponent<Rigidbody>();
        initialRadius = m_Collider.radius;
        if (this.GetComponent("ChargedFireBall") == null) isCharged = false;
        else isCharged = true;
        isDestroyed = false;
        //m_Collider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!isDestroyed)
        {
            particleTimer++;
            if (particleTimer > 15)
            {
                Instantiate(ashEffect, m_Collider.transform.position, m_Collider.transform.rotation);
                particleTimer = 0;
            }

            rigidbody.AddForce(gravity);

            if (activateGravityTimer >= 1 && activateGravityTimer < 10)
            {
                if (Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) >= 24) rigidbody.AddForce(Vector3.up * -4500);
                else if (Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) >= 20) rigidbody.AddForce(Vector3.up * -1200);
                else if (Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) >= 8) rigidbody.AddForce(Vector3.up * -400);
                else if (Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) <= -5) rigidbody.AddForce(Vector3.up * 0);
                else rigidbody.AddForce(Vector3.up * -75);
            }
            else if (activateGravityTimer >= 10 && activateGravityTimer < 20 && Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) >= 13)
            {
                rigidbody.AddForce(Vector3.up * -400);
            }
            else if (activateGravityTimer >= 20 && activateGravityTimer < 30)
            {
                rigidbody.AddForce(Vector3.up * -200);
            }
            else if (activateGravityTimer >= 30)
            {
                rigidbody.AddForce(Vector3.up * -400);
            }

            if (isCharged && activateGravityTimer >= 60) DestroyInstantly();
            else if (activateGravityTimer >= 90) DestroyInstantly();
            activateGravityTimer++;
        }
    }

    private IEnumerator WaitSeconds(float _time)
    {
        yield return new WaitForSeconds(_time);
        GameObject.Destroy(this.gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Enemy" | other.collider.tag == "Default" | other.collider.tag == "Untagged" | other.collider.tag == "Ground" | other.collider.tag == "NPC")
        {
            DestroyWithDelay();
        }
    }
    public void DestroyInstantly()
    {
        isDestroyed = true;

        foreach (GameObject effect in destroyEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
        
        Instantiate(lingeringHitbox, transform.position, transform.rotation);

        Destroy(this.gameObject);
    }

    public void DestroyWithDelay()
    {
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        m_Collider.enabled = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        isDestroyed = true;
        meshObject.SetActive(false);

        foreach (GameObject effect in beforeDestroyEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }

        yield return new WaitForSeconds(1);

        DestroyInstantly();
    }

    private void CheckVariant()
    {
        if (this.GetComponent("ChargedFireBall") == null) DestroyInstantly();
    }
}