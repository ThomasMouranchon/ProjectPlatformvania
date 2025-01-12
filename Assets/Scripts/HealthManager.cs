using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Text;
using static UnityEngine.Rendering.DebugUI;

public enum MaskColor
{
    White,
    Red,
    Yellow,
    Blue
}
//public static color colorOptions;

public class HealthManager : MonoBehaviour
{
    private static HealthManager instance = null;
    public static HealthManager Instance => instance;

    private CharacterManager characterManager;
    public GameObject playerObject;
    private GameManager gameManager;
    public PauseManager pauseManagerScript;
    private GameLoader levelLoader;
    private OptionsValues optionsValues;
    private SaveManager saveManager;
    //public UIHurtVfx hurtVfx;

    public bool isRespawning;
    public Vector3 respawnPosition;
    public Quaternion respawnRotation;
    [Space(10)]

    public float waitForRespawnTimer;
    [Space(10)]

    public GameObject deathEffect;
    [Space(10)]

    public Animator redScreenAnim;
    public float damageAnimationDuration;
    [Space(10)]
    public float waitForFadeTimer;
    [Space(10)]
    public bool isHurt = false;

    private GameObject cameraTarget;

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

    void Start()
    {
        characterManager = CharacterManager.Instance;
        gameManager = GameManager.Instance;
        saveManager = SaveManager.Instance;

        respawnPosition = characterManager.transform.position + new Vector3(0, 10, 0);
        respawnRotation = characterManager.transform.rotation;
                
        levelLoader = GameLoader.Instance;
        optionsValues = ScriptLocations.Instance.optionsValues;
        cameraTarget = characterManager.cameraTarget;
    }

    public void HurtPlayer()
    {
        characterManager.HurtEffect();
        StartCoroutine(DamageCo());
        Respawn();
    }

    public IEnumerator DamageCo()
    {
        isHurt = true;

        redScreenAnim.CrossFade("Background_HurtDeathStart", 0);
        
        yield return new WaitForSeconds(damageAnimationDuration);

        redScreenAnim.CrossFade("Background_HurtDeathEnd", 0);

        isHurt = false;
    }

    public void Respawn()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnCo());
        }
    }

    public IEnumerator RespawnCo()
    {
        characterManager.diveTrailEffect1.Stop();
        characterManager.diveTrailEffect2.Stop();
        characterManager.diveTrailEffect3.Stop();
        pauseManagerScript.canBePaused = false;

        Time.timeScale = 0;

        characterManager.DeathEffect();

        CameraFreeLookController.Instance.SwitchFixedCamera(false, true);

        yield return new WaitForSecondsRealtime(damageAnimationDuration);

        Time.timeScale = 1;

        isRespawning = true;
        characterManager.gameObject.SetActive(false);

        Instantiate(deathEffect, characterManager.transform.position, characterManager.transform.rotation);

        yield return new WaitForSeconds(waitForRespawnTimer);

        levelLoader.LoadFromDeath(waitForFadeTimer / 1.5f);

        yield return new WaitForSeconds(waitForFadeTimer / 2);

        cameraTarget.transform.position = respawnPosition;
        cameraTarget.transform.rotation = respawnRotation;

        isRespawning = false;

        characterManager.gameObject.SetActive(true);
        characterManager.additionalGravity = 0;
        characterManager.ResetState();

        characterManager.transform.position = respawnPosition;
        characterManager.transform.rotation = respawnRotation;

        characterManager.StartGame(1);

        yield return new WaitForSecondsRealtime(1);

        pauseManagerScript.canBePaused = true;
    }

    public void HideUI(bool state)
    {
        if (CameraFreeLookController.Instance.fixedCamera) CameraFreeLookController.Instance.SwitchUIFixedCamera(!state);
    }

    public void SetSpawnPoint(Vector3 newPosition, Quaternion newRotation)
    {
        respawnPosition = newPosition;
        respawnRotation = newRotation;
    }
}