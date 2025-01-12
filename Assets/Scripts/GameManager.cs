using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance => instance;

    private InputReader inputReader;
    [Space(10)]
    public GameObject cameraAimTarget;
    public GameObject inGameInterface;
    public ParticleSystem whiteEffect;
    public Transform UiVfxReferenceTr;
    [Space(10)]
    private HealthManager healthManager;
    private CharacterManager characterManager;
    private PauseManager pauseManager;
    private OptionsValues optionsValues;
    private ItemsHandler itemsHandler;
    private SaveManager saveManager;
    void Awake()
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

    void Start()
    {
        inputReader = InputReader.Instance;
        healthManager = HealthManager.Instance;
        characterManager = CharacterManager.Instance;
        pauseManager = PauseManager.Instance;
        optionsValues = FindObjectOfType<OptionsValues>();
        itemsHandler = ItemsHandler.Instance;
        saveManager = SaveManager.Instance;

        ActivateInGameInterface(optionsValues.visibleInterface);
    }

    public void ActivateAimTarget(bool targetIsActive)
    {
        cameraAimTarget.SetActive(targetIsActive);
    }

    public void ActivateInGameInterface(bool targetIsActive)
    {
        if (targetIsActive) inGameInterface.transform.localScale = new Vector3(1, 1, 1);
        else inGameInterface.transform.localScale = new Vector3(0, 0, 0);
    }
}
