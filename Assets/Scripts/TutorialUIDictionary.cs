using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIDictionary : MonoBehaviour
{
    public Dictionary<string, GameObject> tutorialUiDatabase = new Dictionary<string, GameObject>();

    //public GameObject UIModifiers;
    public GameObject[] colliderList;

    // Start is called before the first frame update
    void Awake()
    {
        //colliderList = UIModifiers.FindGameObjectsWithTag("UITips");
        foreach (GameObject gameObj in colliderList)
        {
            tutorialUiDatabase.Add(gameObj.name, gameObj);
        }
        /*foreach (GameObject gameObj in UIModifiers)
        {
            tutorialUIDatabase.Add(gameObj.name, gameObj);
        }*/
    }

    public void FindTutorialUI(string name)
    {
        tutorialUiDatabase[name].SetActive(!tutorialUiDatabase[name].activeSelf);
    }
}
