using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnPause : MonoBehaviour
{
    private Vector3 worldPosition;
    public Camera characterCamera;

    public ParticleSystem mouseEffect;
    public Transform canvas;
    //public GameObject go;
    //public TrailRenderer mouseTrailEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Plane objPlane = new Plane(characterCamera.transform.forward * -1, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y));

        Ray mRay = characterCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100));

        float rayDistance = 0.1f;
        if (objPlane.Raycast(mRay, out rayDistance))
        {
            //this.transform.position =  new Vector3(mRay.GetPoint(rayDistance).x, mRay.GetPoint(rayDistance).y, canvas.position.z - 35);
            this.transform.position = mRay.GetPoint(rayDistance);
            //this.transform.position.z = canvas.position.z - 1;
        }

        if (Time.timeScale == 0)
        {
            //mouseEffect.Play();
            //go.SetActive(true);
            Instantiate<ParticleSystem>(mouseEffect, this.transform.position, this.transform.rotation);
            //mouseTrailEffect.time = 0.1f;
            //mouseTrailEffect.emitting = true;
        }
        /*else
        {
            go.SetActive(false);

            //mouseEffect.Stop();
            //mouseTrailEffect.time = 0;
            //mouseTrailEffect.emitting = false;
        }*/
    }
}
