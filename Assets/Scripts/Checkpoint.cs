using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private HealthManager healthManager;
    private SaveManager saveManager;

    public GameObject rendererOn, rendererOff;

    public int value = -1;
    public SaveManager.zones location;

    private void Start()
    {
        healthManager = HealthManager.Instance;
    }

    public void CheckpointOn()
    {
        Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

        foreach (Checkpoint cp in checkpoints)
        {
            cp.CheckpointOff();
        }

        if (rendererOn) rendererOn.SetActive(true);
        if (rendererOff) rendererOff.SetActive(false);

        if (value != -1) AddTeleporter(value);
    }

    public void CheckpointOff()
    {
        if (rendererOn) rendererOn.SetActive(false);
        if (rendererOff) rendererOff.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            healthManager.SetSpawnPoint(transform.position + Vector3.up * 3, transform.rotation);
            CheckpointOn();
        }
    }

    private void AddTeleporter(int value)
    {
        SaveManager.Instance.lastTeleportPoint = value;
        SaveManager.Instance.activatedTeleportations[value] = true;
        
        SaveManager.Instance.activatedTeleportationsPositionsX[value] = transform.position.x;
        SaveManager.Instance.activatedTeleportationsPositionsY[value] = transform.position.y;
        SaveManager.Instance.activatedTeleportationsPositionsZ[value] = transform.position.z;

        SaveManager.Instance.activatedTeleportationsRotationsX[value] = transform.rotation.x;
        SaveManager.Instance.activatedTeleportationsRotationsY[value] = transform.rotation.y;
        SaveManager.Instance.activatedTeleportationsRotationsZ[value] = transform.rotation.z;
        SaveManager.Instance.activatedTeleportationsRotationsW[value] = transform.rotation.w;

        SaveManager.Instance.activatedTeleportationsZone[value] = location.ToString();
    }
}