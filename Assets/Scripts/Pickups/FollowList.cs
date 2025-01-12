using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowList : MonoBehaviour
{
    private static FollowList instance = null;
    public static FollowList Instance => instance;

    public List<GameObject> followPlayerList;// = new List<GameObject>();
    public Collider characterManagerCollider;
    public float speed = 22;
    private int increaseSpeedTimer;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }

    void FixedUpdate()
    {
        AdjustFollowersBehavior();
    }

    public void AdjustFollowersBehavior()
    {
        /*if (followPlayerList.Count > 1)
        {
            for (int i = 0; i < (followPlayerList.Count - 1); i++)
            {
                if (followPlayerList[i] == null)
                {
                    followPlayerList[i] = followPlayerList[i + 1];
                    followPlayerList.Remove(followPlayerList[i + 1]);
                }
            }
        }
        if (followPlayerList != null) FollowInOrder();*/

        if (followPlayerList != null && followPlayerList.Count > 0)
        {
            for (int i = 0; i < (followPlayerList.Count - 1); i++)
            {
                if (followPlayerList[i] == null)
                {
                    followPlayerList[i] = followPlayerList[i + 1];
                    followPlayerList.Remove(followPlayerList[i + 1]);
                }
            }
            FollowInOrder();
            //if (followPlayerList.Count == 1) followPlayerList.RemoveAt(0);
        }
        else followPlayerList.Clear();

        /*if (followPlayerList != null && followPlayerList.Count > 0)
        {
            List<GameObject> nonNullElements = new List<GameObject>();

            // Collectez les �l�ments non nuls dans une nouvelle liste
            foreach (var player in followPlayerList)
            {
                if (player != null)
                {
                    nonNullElements.Add(player);
                }
            }

            // Mettez � jour la liste d'origine avec les �l�ments non nuls
            followPlayerList = nonNullElements;

            FollowInOrder();
        }
        else
        {
            // G�rez le cas o� la liste est nulle ou vide.
            followPlayerList.Clear();
        }*/
    }

    public void FollowInOrder()
    {
        if (followPlayerList != null && followPlayerList.Count > 0 && followPlayerList[0] != null)
        {
            followPlayerList[0].transform.position = Vector3.MoveTowards(followPlayerList[0].transform.position, characterManagerCollider.transform.position, speed * Time.deltaTime);
            //followPlayerList[0].transform.position = Vector3.MoveTowards(followPlayerList[0].transform.position, characterManagerCollider.transform.position/*new Vector3(characterManagerCollider.transform.position.x, characterManagerCollider.transform.position.y - 2, characterManagerCollider.transform.position.z)*/, speed * Time.deltaTime);
            //followPlayerList[1].transform.position = Vector3.MoveTowards(followPlayerList[1].transform.position, followPlayerList[0].transform.position/*new Vector3(characterManagerCollider.transform.position.x, characterManagerCollider.transform.position.y - 2, characterManagerCollider.transform.position.z)*/, speed * Time.deltaTime / 1.5f);
            for (int i = 0; i < followPlayerList.Count; i++)
            {
                if (i > 0) followPlayerList[i].transform.position = Vector3.MoveTowards(followPlayerList[i].transform.position, followPlayerList[i - 1].transform.position, speed * Time.deltaTime / i);
                //else followPlayerList.RemoveAt(0);
                //followPlayerList.Remove(followPlayerList[i+1];
            }
        }
    }

    public void AddFollower(GameObject newFollower)
    {
        followPlayerList.Add(newFollower);
        AdjustFollowersBehavior();
    }
}
