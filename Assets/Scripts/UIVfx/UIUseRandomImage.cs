using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUseRandomImage : MonoBehaviour
{
    public Sprite[] imagesList;

    void Start()
    {
        GenerateRandomImage();
    }

    public void GenerateRandomImage()
    {
        if (imagesList.Length > 0)
        {
            int randomIndex = Random.Range(0, imagesList.Length);

            Sprite randomImage = imagesList[randomIndex];

            GetComponent<Image>().sprite = randomImage;
        }
    }
}
