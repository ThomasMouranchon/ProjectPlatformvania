using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerTransformTarget;

public class RailMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [Space(10)]

    [Header("Basic Movement")]
    public Transform[] positionsList;
    public int positionTarget;
    public float movementSpeed, stepSpeed, steps = 18;
    private float currentSpeed;
    [Space(10)]

    [Header("Initiate Movement")]
    public bool canMove = true;
    private bool startImmediately;
    [Space(10)]

    [Header("Waiting")]
    public float waitingTimeBetweenPositions;
    public bool isWaiting;
    [Space(10)]

    [Header("One-way rail")]
    public float waitingTimeBeforeRespawn = 1;
    public bool isRespawning;
    public ParticleSystem respawnEffect;
    public GameObject goToRender;

    private void Start()
    {
        if (target == null) target = gameObject;

        if (positionsList.Length == 0) canMove = false;
        if (canMove) target.transform.position = positionsList[0].position;
        isWaiting = false;
        startImmediately = canMove;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 targetPosition = positionsList[positionTarget].position;
            float distance = Vector3.Distance(target.transform.position, targetPosition);

            if (distance < 1)
            {
                currentSpeed = 0;

                if (positionTarget + 1 != positionsList.Length) positionTarget++;
                else
                {
                    positionTarget = 0;
                    if (positionsList[positionsList.Length - 1] != positionsList[0]) StartCoroutine(WaitBeforeRespawn());
                }

                if (waitingTimeBetweenPositions > 0) StartCoroutine(WaitBetweenPositions());
            }
            else if (!isWaiting)
            {
                if (distance < stepSpeed * steps)
                {
                    if (currentSpeed > 0.01f) currentSpeed -= stepSpeed;
                }
                else if (currentSpeed < movementSpeed) currentSpeed += stepSpeed;

                target.transform.position = Vector3.MoveTowards(target.transform.position,
                    positionsList[positionTarget].position,
                    currentSpeed * Time.deltaTime);
            }
        }
    }

    IEnumerator WaitBetweenPositions()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitingTimeBetweenPositions);
        isWaiting = false;
    }

    IEnumerator WaitBeforeRespawn()
    {
        canMove = false;

        yield return new WaitForSeconds(waitingTimeBeforeRespawn);

        goToRender.SetActive(false);
        Instantiate(respawnEffect, positionsList[positionsList.Length - 1]);

        yield return new WaitForSeconds(waitingTimeBeforeRespawn);

        target.transform.position = positionsList[0].position;
        positionTarget = 1;
        goToRender.SetActive(true);
        Instantiate(respawnEffect, positionsList[0]);

        canMove = startImmediately;
    }
}