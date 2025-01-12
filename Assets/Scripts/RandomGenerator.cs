using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    private static RandomGenerator instance = null;
    public static RandomGenerator Instance => instance;

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

    public int RandomGeneratorFunction(int min, int max)
    {
        return Random.Range(min, max);
    }
}
