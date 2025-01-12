using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollBarNavigation : MonoBehaviour
{
    public Scrollbar scrollbar;
    private int numberOfSteps;
    private float stepLegnth;
    // Start is called before the first frame update
    void Start()
    {
        numberOfSteps = scrollbar.numberOfSteps;
        stepLegnth = 1 / numberOfSteps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scroll()
    {
        //if (EventSystem.currentSelectedGameObject == )
    }
}
