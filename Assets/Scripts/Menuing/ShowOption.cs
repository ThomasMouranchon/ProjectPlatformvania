using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOption : MonoBehaviour
{
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOptions()
    {
        target.SetActive(!target.activeSelf);
    }
}
