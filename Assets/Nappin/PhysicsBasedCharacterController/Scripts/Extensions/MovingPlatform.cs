using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PhysicsBasedCharacterController
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        [Header("Platform movement")]
        public Vector3[] destinations;
        public float timeDelay;
        public float timeDelayBeginningEnd;
        public float platformSpeedDamp;
        public bool smoothMovement;
        [Space(10)]

        public bool canTranslate = true;


        [Header("Platform rotation")]
        public Vector3 rotationSpeed;
        [Space(10)]

        public bool canRotate = true;
        public bool canBeMoved = false;


        private Vector3 nextDestination;
        private int currentDestination = 0;
        private Vector3 velocity = Vector3.zero;
        private bool canMove = true;

        private List<Rigidbody> rigidbodies = new List<Rigidbody>();

        private Vector3 lastEulerAngles;
        private Vector3 lastPosition;
        private Transform tr;
        private Rigidbody rb;


        /**/


        private void Awake()
        {
            tr = this.GetComponent<Transform>();
            lastPosition = tr.position;
            lastEulerAngles = tr.eulerAngles;
            rb = this.GetComponent<Rigidbody>();

            if (canTranslate) tr.position = destinations[0];
            nextDestination = tr.position;
        }


        private void FixedUpdate()
        {
            UpdateDestination();
            UpdatePositionAndRotation();
            UpdateBodies();
        }


        #region Platform and Rigidbody

        private void UpdateDestination()
        {

            if (Vector3.Distance(tr.position, nextDestination) <= 0.01f)
            {
                rb.position = nextDestination;

                if ((currentDestination == 0 || currentDestination == destinations.Length - 1) && canMove) StartCoroutine(WaitTime(timeDelayBeginningEnd));
                else if (canMove) StartCoroutine(WaitTime(timeDelay));

                SetNextDestination();
            }
        }


        private void UpdatePositionAndRotation()
        {
            if (canMove)
            {
                if (canTranslate)
                {
                    if (smoothMovement) rb.position = Vector3.SmoothDamp(tr.position, nextDestination, ref velocity, platformSpeedDamp * Time.deltaTime);
                    else rb.position = Vector3.MoveTowards(tr.position, nextDestination, platformSpeedDamp * Time.deltaTime);
                }

                if (canRotate)
                {
                    if (!canBeMoved) tr.Rotate(rotationSpeed.x * Time.deltaTime, rotationSpeed.y * Time.deltaTime, rotationSpeed.z * Time.deltaTime);
                    else rb.AddTorque(new Vector3(rotationSpeed.x, rotationSpeed.y, rotationSpeed.z), ForceMode.Force);
                }
            }
        }


        private void UpdateBodies()
        {
            if (rigidbodies.Count > 0)
            {
                Vector3 velocity = tr.position - lastPosition;
                Vector3 angularVelocity = tr.eulerAngles - lastEulerAngles;

                for (int i = 0; i < rigidbodies.Count; i++)
                {
                    Rigidbody rb = rigidbodies[i];

                    if (angularVelocity.y > 0)
                    {
                        rb.transform.RotateAround(tr.position, Vector3.up, angularVelocity.y);
                        try { rb.GetComponent<CharacterManager>().targetAngle = rb.GetComponent<CharacterManager>().targetAngle + angularVelocity.y; }
                        catch { /* Debug.Log("There is no player on the platform") */ }
                    }

                    if (this.rb.velocity.magnitude > 0) rb.velocity += this.rb.velocity;

                    rb.position += velocity;
                }
            }

            lastPosition = tr.position;
            lastEulerAngles = tr.eulerAngles;
        }

        #endregion


        #region Handle list

        public void Add(Rigidbody _rb)
        {
            if (!rigidbodies.Contains(_rb)) rigidbodies.Add(_rb);
        }


        public void Remove(Rigidbody _rb)
        {
            if (rigidbodies.Contains(_rb)) rigidbodies.Remove(_rb);
        }

        #endregion


        #region Platform Handlers

        private void SetNextDestination()
        {
            currentDestination++;
            if (currentDestination > destinations.Length - 1) currentDestination = 0;

            nextDestination = destinations[currentDestination];
        }


        private IEnumerator WaitTime(float _time)
        {
            canMove = false;
            yield return new WaitForSeconds(_time);
            canMove = true;
        }

        #endregion
    }
}