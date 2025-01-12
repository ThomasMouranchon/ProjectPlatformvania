using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    /*public GameObject ui;
    public string uiName;*/
    private TutorialUIDictionary tutoUiDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        tutoUiDictionary = FindObjectOfType<TutorialUIDictionary>();
        //ui = GameObject.FindWithTag("UITips");
        //ui = GameObject.Find(uiName);
        //ui = FindObjectWithName<uiName>();
        //ui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutoUiDictionary.FindTutorialUI(this.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tutoUiDictionary.FindTutorialUI(this.name);
        }
    }
}
