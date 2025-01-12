using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_CameraAim : MonoBehaviour
{
    private static Move_CameraAim instance = null;
    public static Move_CameraAim Instance => instance;

    private Vector2 screenCenterPoint;
    private Ray ray;
    [Tooltip("Mask used for aiming")]
    public LayerMask projectileDetectionMask;

    [Tooltip("Where the fireball is aiming")]
    private Vector3 projectileDirection;
    private float aimPositionValue;
    public GameObject raycastTPSTarget;

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
    
    public void CameraAim()
    {
        screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 99999f, projectileDetectionMask))
        {
            projectileDirection = raycastHit.point;

            aimPositionValue = ray.GetPoint(75).y - ray.GetPoint(50).y;

            switch (aimPositionValue)
            {
                default:
                    raycastTPSTarget.transform.position = new Vector3(0f, 12, 0);
                    break;
                case > 0 and <= 0.1f:
                    raycastTPSTarget.transform.position = new Vector3(0f, 12, 0);
                    break;
                case > 0.1f and <= 0.115f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 115, 0);
                    break;
                case > 0.115f and <= 0.15f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 90, 0);
                    break;
                case > 0.15f and <= 0.2f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 70, 0);
                    break;
                case > 0.2f and <= 0.3f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 45, 0);
                    break;
                case > 0.3f and <= 0.4f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 35, 0);
                    break;
                case > 0.4f and <= 0.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 30, 0);
                    break;
                case > 0.5f and <= 0.6f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 25, 0);
                    break;
                case > 0.6f and <= 0.7f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 20 + aimPositionValue, 0);
                    break;
                case > 0.7f and <= 0.85f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 17 + aimPositionValue, 0);
                    break;
                case > 0.85f and <= 1:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 14 + aimPositionValue, 0);
                    break;
                case > 1 and <= 1.2f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 14 - aimPositionValue, 0);
                    break;
                case > 1.2f and <= 1.35f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 12 - aimPositionValue, 0);
                    break;
                case > 1.35f and <= 1.6f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 9 + aimPositionValue, 0);
                    break;
                case > 1.6f and <= 2:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 8 + aimPositionValue, 0);
                    break;
                case > 2 and <= 2.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 7, 0);
                    break;
                case > 2.5f and <= 3:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 6.5f, 0);
                    break;
                case > 3 and <= 3.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 6, 0);
                    break;
                case > 3.5f and <= 4:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5.5f, 0);
                    break;
                case > 4 and <= 5:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5, 0);
                    break;
                case > 5 and <= 6:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.5f, 0);
                    break;
                case > 6 and <= 7:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.25f, 0);
                    break;
                case > 7 and <= 8:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4, 0);
                    break;
                case > 8 and <= 14:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 3.75f, 0);
                    break;
                case > 14 and <= 16:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4, 0);
                    break;
                case > 16 and <= 18:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.25f, 0);
                    break;
                case > 18 and <= 20:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.75f, 0);
                    break;
                case > 20 and <= 20.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5, 0);
                    break;
                case > 20.5f and <= 21:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5.25f, 0);
                    break;
                case > 21 and <= 21.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5.75f, 0);
                    break;
                case > 21.5f and <= 22:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 6, 0);
                    break;
                case > 22 and <= 22.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 6.5f, 0);
                    break;
                case > 22.5f and <= 23:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 7, 0);
                    break;
                case > 23 and <= 23.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 8, 0);
                    break;
                case > 23.5f and <= 24:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 9, 0);
                    break;
                case > 24:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 11, 0);
                    break;
                case < -0.25f and >= -0.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, 11, 0);
                    break;
                case < -0.5f and >= -1:
                    raycastTPSTarget.transform.position = new Vector3(0f, 10, 0);
                    break;
                case < -1 and >= -2:
                    raycastTPSTarget.transform.position = new Vector3(0f, 10 + aimPositionValue, 0);
                    break;
                case < -2 and >= -3:
                    raycastTPSTarget.transform.position = new Vector3(0f, 8 + aimPositionValue, 0);
                    break;
                case < -3 and >= -3.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, 6 + aimPositionValue, 0);
                    break;
                case < -3.5f and >= -4:
                    raycastTPSTarget.transform.position = new Vector3(0f, 5.5f + aimPositionValue, 0);
                    break;
                case < -4 and >= -4.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * -0.5f, 0);
                    break;
                case < -4.5f and >= -5:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * -0.05f, 0);
                    break;
                case < -5 and >= -6:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 0.5f, 0);
                    break;
                case < -6 and >= -7:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 0.75f, 0);
                    break;
                case < -7 and >= -8:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 1, 0);
                    break;
                case < -8 and >= -9:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 1.25f, 0);
                    break;
                case < -9 and >= -10.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 1.5f, 0);
                    break;
                case < -10.5f and >= -12.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 1.75f, 0);
                    break;
                case < -12.5f and >= -14:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 2, 0);
                    break;
                case < -14 and >= -16:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 2.25f, 0);
                    break;
                case < -16 and >= -17:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 2.5f, 0);
                    break;
                case < -17 and >= -18:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 2.65f, 0);
                    break;
                case < -18 and >= -19.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 3, 0);
                    break;
                case < -19.5f and >= -20.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 3.25f, 0);
                    break;
                case < -20.5f and >= -21:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 3.5f, 0);
                    break;
                case < -21 and >= -21.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 3.75f, 0);
                    break;
                case < -21.5f and >= -22:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.25f, 0);
                    break;
                case < -22 and >= -22.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 4.75f, 0);
                    break;
                case < -22.5f and >= -23:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5, 0);
                    break;
                case < -23 and >= -23.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 5.5f, 0);
                    break;
                case < -23.5f and >= -24:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 6.5f, 0);
                    break;
                case < -24 and >= -24.5f:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 8, 0);
                    break;
                case < -24.5f and >= -26:
                    raycastTPSTarget.transform.position = new Vector3(0f, aimPositionValue * 12, 0);
                    break;

            }
        }
    }
}
