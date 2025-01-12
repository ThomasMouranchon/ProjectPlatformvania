using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoHandler : MonoBehaviour
{
    private static YoyoHandler instance = null;
    public static YoyoHandler Instance => instance;

    public SphereCollider m_Collider;
    private float initialRadius;

    public GameObject[] ashesEffects;
    [Space(10)]

    public GameObject[] startIdleEffects;
    [Space(10)]

    public GameObject[] startReturnEffects;
    [Space(10)]

    public GameObject[] caughtByPlayerEffects;
    [Space(10)]

    private int particleTimer; // For fire particle effect
    private int activateGravityTimer;
    private Rigidbody rigidbody;
    private Vector3 gravity = Vector3.zero;
    private bool isCharged;
    public Animator yoyoAnim;

    public bool isIdling, isReturningToPlayer;
    public int returnSpeed;

    private CharacterManager characterManager;

    public LineRenderer lineRenderer;

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
        rigidbody = GetComponent<Rigidbody>();
        initialRadius = m_Collider.radius;
        if (this.GetComponent("ChargedFireBall") == null) isCharged = false;
        else isCharged = true;

        isIdling = false;
        isReturningToPlayer = false;

        Move_Throws.Instance.yoyoIsActive = true;
        characterManager.soulAnim.SetBool("yoyoIsActive", true);

        Application.onBeforeRender += Move_Throws.Instance.UpdateLine;
    }

    private void FixedUpdate()
    {
        particleTimer++;
        if (particleTimer > 5)
        {
            foreach (GameObject effect in ashesEffects)
            {
                Instantiate(effect, transform.position, transform.rotation);
            }
            particleTimer = 0;
        }

        if (isReturningToPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (isIdling)
            {
                if (activateGravityTimer >= 120)
                {
                    StartMoveTowardsPlayer();
                }
            }
            else
            {
                if (isCharged && activateGravityTimer >= 60) StartIdle();
                else if (activateGravityTimer >= 60) StartIdle();
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

            activateGravityTimer++;
        }

        if (!isReturningToPlayer && (Mathf.Abs(this.transform.position.x - characterManager.transform.position.x) >= 75
            | Mathf.Abs(this.transform.position.y - characterManager.transform.position.y) >= 40
            | Mathf.Abs(this.transform.position.z - characterManager.transform.position.z) >= 75)) StartMoveTowardsPlayer();
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
            if (!isReturningToPlayer && !isIdling) StartIdle();
        }
        else if (other.collider.tag == "Player" && (isIdling | isReturningToPlayer) )
        {
            CaughtByPlayer();
        }
    }

    public void StartIdle()
    {
        foreach (GameObject effect in startIdleEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }

        isIdling = true;
        activateGravityTimer = 0;

        if (rigidbody != null)
        {
            rigidbody.velocity /= 2;
        }
        //rigidbody.useGravity = false;
        yoyoAnim.CrossFade("MaskThunder_Idle", 0);
    }

    public void StartMoveTowardsPlayer()
    {
        isReturningToPlayer = true;
        isIdling = false;
        rigidbody.velocity /= 2;
        rigidbody.useGravity = false;
        returnSpeed = 20;
        foreach (GameObject effect in startReturnEffects)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
        yoyoAnim.CrossFade("MaskThunder_Move", 0);
        characterManager.soulAnim.CrossFade("Player_YoyoChangeToReturn", 0, 8);
    }

    public void MoveTowardsPlayer()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, characterManager.transform.position, returnSpeed * Time.deltaTime);

        Vector3 direction = (characterManager.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10);
            
        if (characterManager.soulAnim.GetCurrentAnimatorStateInfo(8).IsName("Player_YoyoChangeToIdle"))
        {
            characterManager.soulAnim.CrossFade("Player_YoyoChangeToReturn", 0.05f, 8);
        }
        
        returnSpeed++;
    }

    public void CaughtByPlayer()
    {
        foreach (GameObject effect in caughtByPlayerEffects)
        {
            Instantiate(effect, Move_Throws.Instance.ropePointLeft.transform.position, transform.rotation);
        }
        characterManager.soulAnim.CrossFade("Player_YoyoCatch", 0, 8);
        Move_Throws.Instance.yoyoIsActive = false;
        characterManager.soulAnim.SetBool("yoyoIsActive", false);
        Destroy(gameObject);
    }

    private void CheckVariant()
    {
        if (this.GetComponent("ChargedFireBall") == null) StartIdle();
    }

    void OnDestroy()
    {
        Application.onBeforeRender -= Move_Throws.Instance.UpdateLine;
        instance = null;
    }
}