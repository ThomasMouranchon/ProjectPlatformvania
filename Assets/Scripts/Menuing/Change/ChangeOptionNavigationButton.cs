using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOptionNavigationButton : MonoBehaviour
{
    public ChangeOptionNavigation changeOptionNavigation;
    public ChangeSubMenu changeSubMenu;
    public int value;

    public void ChangeOptionNav()
    {
        changeOptionNavigation.ChangeOptionNav(value);
        changeSubMenu.ShowGameObject();
    }
}