using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStringList : MonoBehaviour
{
    public int value;
    public bool effectOnHover;
    public ChangeString[] changeStringList;

    public void SelectChangeString()
    {
        if (changeStringList[value].enabled)
        {
            changeStringList[value].ShowString();
        }
        else
        {
            changeStringList[value].ShowHiddenString();
        }
    }
}