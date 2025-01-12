using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    private static DamageDetector instance = null;
    public static DamageDetector Instance => instance;

    public List<GameObject> ennemisDetectes = new List<GameObject>();

    private CharacterManager characterManager;

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

    private void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage")
        {
            ennemisDetectes.Add(other.gameObject);
            UpdateDamageBool();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Damage")
        {
            ennemisDetectes.Remove(other.gameObject);
            UpdateDamageBool();
        }
    }

    public void RemoveDamage(GameObject target)
    {
        ennemisDetectes.Remove(target);
        UpdateDamageBool();
    }

    public void UpdateDamageBool()
    {
        StartCoroutine(UpdateDamageBoolCoroutine());
    }

    IEnumerator UpdateDamageBoolCoroutine()
    {
        yield return new WaitForEndOfFrame();

        bool ennemiPresent = false;
        foreach (GameObject ennemi in ennemisDetectes)
        {
            if (ennemi != null && ennemi.activeSelf)
            {
                ennemiPresent = true;
                break;
            }
        }
        characterManager.soulAnim.SetBool("isCloseToDamage", ennemiPresent);
    }
}