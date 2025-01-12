using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerScript : MonoBehaviour
{
    public float speed = 15.0f;
    public bool increaseSpeed;
    //public bool dropOnDeath = false;
    public TrailRenderer followingTrail;
    private int increaseSpeedTimer;
    public bool followPlayer;

    private Collider characterManagerCollider;
    //private RandomGenerator randomGeneratorScript;

    public ParticleSystem particlesIdle;
    public ParticleSystem particlesFollowing;
    private int particleTimer;
    private int particleTimerLimit;
    public bool emitParticleWhenIdle;

    private FollowList followListScript;

    void Awake()
    {
        followListScript = FollowList.Instance;
        //characterManagerCollider = followListScript.characterManagerCollider;
    }

    // Start is called before the first frame update
    void Start()
    {
        /*if (dropOnDeath)
        {
            FollowPlayer();
            /*followPlayer = true;
            followingTrail.enabled = true;
            followListScript.AddFollower(this.gameObject);*/
        /*}
        else
        {
            followPlayer = false;
            followingTrail.enabled = false;
        }*/
        //followingTrail = this.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followPlayer)
        {
            var step = speed * Time.deltaTime; // calculate distance to move
            this.transform.position = Vector3.MoveTowards(transform.position, new Vector3(characterManagerCollider.transform.position.x, characterManagerCollider.transform.position.y - 2, characterManagerCollider.transform.position.z), step);
            if (speed < 30)
            {
                if (increaseSpeed && increaseSpeedTimer < 30) increaseSpeedTimer++;
                else if (increaseSpeedTimer >= 30)
                {
                    speed *= 1.1f;
                    followingTrail.time *= 0.8f;
                    increaseSpeedTimer = 0;
                }
            }
            if (particleTimer <= particleTimerLimit) particleTimer++;
            else
            {
                Instantiate<ParticleSystem>(particlesFollowing, this.transform.position, this.transform.rotation);
                particleTimer = 0;
                //particleTimerLimit = randomGeneratorScript.RandomGeneratorFunction(1, 10);
                particleTimerLimit = Random.Range(1, 10);
                particlesIdle.Stop();
            }
        }
        else if (emitParticleWhenIdle && Vector3.Distance(Camera.main.transform.position, transform.position) < 200)
        {
            if (!particlesIdle.isPlaying) particlesIdle.Play();
        }
        else
        {
            particlesIdle.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            characterManagerCollider = other;
            //FollowPlayer();
            followPlayer = true;
            followingTrail.enabled = true;
            //followListScript.AddFollower(this.gameObject);
        }
    }

    /*public void FollowPlayer(Collider target)
    {
        var step = speed * Time.deltaTime; // calculate distance to move
        this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }*/

    public void FollowPlayer()
    {
        followPlayer = true;
        followingTrail.enabled = true;
        followListScript.AddFollower(this.gameObject);
        //followListScript.followPlayerList.Add(this.gameObject);
    }
}
