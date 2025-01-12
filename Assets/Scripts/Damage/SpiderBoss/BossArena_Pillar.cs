using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArena_Pillar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            if (collision.gameObject.GetComponent<BossHandler>())
            {
                collision.gameObject.GetComponent<BossHandler>().HitPillar();
            }
        }
    }
}
