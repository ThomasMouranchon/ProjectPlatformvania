using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

public class ChangeInputIcon : MonoBehaviour
{
    public GameObject[] controllerIcons;
    public GameObject defaultControllerIcon, keyboardIcon;

    public void UpdateIcon(InputDevice device)
    {
        if (device is Gamepad gamepad)
        {
            if (gamepad is DualShockGamepad)
            {
                UpdateActiveIcon(0, true);
            }
            else if (gamepad is XInputController)
            {
                UpdateActiveIcon(1, true);
            }
            else if (gamepad is SwitchProControllerHID)
            {
                UpdateActiveIcon(2, true);
            }
            else
            {
                UpdateActiveIcon(50, true);
            }
        }
        else
        {
            UpdateActiveIcon(0, false);
        }
    }

    private void UpdateActiveIcon(int target, bool isController)
    {
        keyboardIcon.SetActive(!isController);

        bool defaultIcon = true;

        for (int i = 0; i < controllerIcons.Length; i++)
        {
            if (i == target)
            {
                controllerIcons[i].SetActive(isController);
                defaultIcon = false;
            }
            else controllerIcons[i].SetActive(false);
        }

        if (defaultIcon) defaultControllerIcon.SetActive(isController);
    }
}
