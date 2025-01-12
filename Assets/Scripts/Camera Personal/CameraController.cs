using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PhysicsBasedCharacterController
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;

        public Vector3 offset;

        public bool useOffsetValue;

        public float rotateSpeedHorizontal;
        public float rotateSpeedVertical;

        public Transform pivot;

        public float minViewAngle;
        public float maxViewAngle;

        public bool invertY;
        //public CameraManager cameraManagerScript;

        private Vector2 cameraInput;
        public InputReader input;

        // Start is called before the first frame update
        void Start()
        {
            if (!useOffsetValue) offset = target.position - transform.position;

            pivot.transform.position = target.transform.position;
            //pivot.transform.parent = target.transform;
            pivot.transform.parent = null;

            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            cameraInput = input.cameraInput;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            pivot.transform.position = target.transform.position;

            //Get the X position of the mouse & rotate the target
            //float horizontal = Mathf.Atan(cameraInput.x * rotateSpeedHorizontal);
            //float horizontal = cameraInput.x * rotateSpeedHorizontal * 0.05f;
            //float horizontal = Input.GetAxis("Mouse X") * rotateSpeedHorizontal;

            //Get the Y position of the mouse & rotate the pivot
            //float vertical = Mathf.Atan(cameraInput.y * rotateSpeedVertical);
            //float vertical = cameraInput.y * rotateSpeedVertical * 0.05f;
            //float vertical = Input.GetAxis("Mouse Y") * rotateSpeedVertical;

            float horizontal = 0;
            float vertical = 0;

            /*if (input.isMouseAndKeyboard)
            {*/
            horizontal = cameraInput.x * rotateSpeedHorizontal;// * 0.2f;
            vertical = cameraInput.y * rotateSpeedVertical;// * 0.2f;
            /*}
            else
            {
                horizontal = cameraInput.x * rotateSpeedHorizontal * 0.3f;
                vertical = cameraInput.y * rotateSpeedVertical * 0.3f;
            }*/

            pivot.Rotate(0, horizontal, 0);
            if (invertY) pivot.Rotate(vertical, 0, 0);
            else pivot.Rotate(-vertical, 0, 0);


            //Move the camera based on the current rotation of the target & the original offset
            float desiredYAngle = pivot.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            transform.position = target.position - (rotation * offset);

            // Limite haut/bas de la rotation de la caméra
            if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f) pivot.rotation = Quaternion.Euler(maxViewAngle, desiredYAngle, 0);

            if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle) pivot.rotation = Quaternion.Euler(360f + minViewAngle, desiredYAngle, 0);

            //transform.position = target.position - offset;

            if (transform.position.y < target.position.y) transform.position = new Vector3(transform.position.x, target.position.y - .5f, transform.position.z);

            transform.LookAt(target);
        }
    }
}