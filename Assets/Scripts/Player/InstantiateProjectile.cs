using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InstantiateProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateMaskPowerProjectile(GameObject fireBall, GameObject characterModel, GameObject raycastTPSTarget, float fireBallTPSSpeed)
    {
        /*if (additionnalHeight != 0)
        {
            Debug.Log(additionnalHeight);
            int addedHeight;
            if (additionnalHeight < -24.5f) addedHeight = 4;
            else if (additionnalHeight < -24f) addedHeight = 6;
            else addedHeight = 8;

            Instantiate<GameObject>(fireBall, this.transform.position + new Vector3(0, addedHeight, 0), characterModel.transform.rotation).GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, raycastTPSTarget.transform.position.y, fireBallTPSSpeed));
        }
        else
        {*/
            Instantiate<GameObject>(fireBall, this.transform.position, characterModel.transform.rotation).GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, 0, fireBallTPSSpeed));
        //}
    }
}
