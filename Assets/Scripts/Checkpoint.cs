using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsBasedCharacterController
{
    public class Checkpoint : MonoBehaviour
    {
        private HealthManager healthManagerScript;

        public Renderer theRend;

        public Material cpOff;
        public Material cpOn;

        // Start is called before the first frame update
        void Start()
        {
            healthManagerScript = HealthManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CheckpointOn()
        {
            Checkpoint[] checkpoints = FindObjectsOfType<Checkpoint>();

            foreach (Checkpoint cp in checkpoints)
            {
                cp.CheckpointOff();
            }
        
            theRend.material = cpOn;
        }

        public void CheckpointOff()
        {
            theRend.material = cpOff;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                healthManagerScript.SetSpawnPoint(transform.position + Vector3.up * 3, transform.rotation);
                CheckpointOn();
            }
        }
    }
}