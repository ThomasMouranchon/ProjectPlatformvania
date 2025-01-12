using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainComponents : MonoBehaviour
{
    public static MainComponents Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
