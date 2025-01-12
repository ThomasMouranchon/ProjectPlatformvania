using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBoolValue : MonoBehaviour
{
    //public string variableNameToChange;
    public bool newValue;
    //public CharacterManager characterManager;

    // Start is called before the first frame update
    void Start()
    {
        newValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.)
        /*if (other.layer == "Ground" | other.layer == "Platform") */newValue = true;
        //ChangeBool();
    }

    public void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.)
        /*if (other.layer == "Ground" | other.layer == "Platform") */newValue = false;
        //ChangeBool();
    }
    /*
    public void ChangeBool()
    {
        characterManager.variableNameToChange = newValue;
    }*/
}
