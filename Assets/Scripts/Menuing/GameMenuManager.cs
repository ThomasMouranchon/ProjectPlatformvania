using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEditor;
using UnityEngine.UI;
using System;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
    public GameObject defaultPanel;
    public GameObject credits;
    [Space(5)]
    public GameObject items, itemsBackground;
    public Animator itemsButtonsAnim;
    [Space(5)]
    public GameObject costumes, costumesBackground;
    public Animator costumesButtonsAnim;
    [Space(5)]
    public GameObject memories, memoriesBackground;
    public Animator memoriesButtonsAnim;
    //private MMFeedbacks memoriesFeedback, memoriesBackFeedback;
    [Space(5)]
    public GameObject options, optionsBackground;
    public Animator optionsButtonsAnim;
    public List<MainComponents> check;
    public string sceneToLoadMenu;
    public GameObject navigationIcon;
    private Animator navigationIconAnim;
    public Animator mainPanelBgColorAnim;
    [Space(10)]
    public InputReader inputReader;
    public PauseManager pauseManager;
    public OptionsValues optionValues;
    public UIPauseCharacterHandler pauseCharacterHandler;
    public ChangeOptionNavigation changeOptionNavigation;
    public ChangeItemNavigation changeItemNavigation;
    private GameLoader levelLoader;
    [Space(10)]
    public GameObject pauseMenuButton, itemsFirstButton, costumesFirstButton, memoriesFirstButton, optionsFirstButton;
    
    [Space(10)]
    public GameObject itemsButton, costumesButton, memoriesButton, optionsButton;

    [Space(10)]
    public Button goBackButton;

    void Start()
    {
        defaultPanel.SetActive(true);
        credits.SetActive(false);

        DisableSubMenus();
        
        levelLoader = GameLoader.Instance;

        navigationIconAnim = navigationIcon.GetComponent<Animator>();
        changeOptionNavigation = GetComponent<ChangeOptionNavigation>();
        changeItemNavigation = GetComponent<ChangeItemNavigation>();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseMenuButton);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisableSubMenus()
    {
        items.SetActive(false);
        itemsBackground.SetActive(false);

        costumes.SetActive(false);
        costumesBackground.SetActive(false);
        
        memories.SetActive(false);
        memoriesBackground.SetActive(false);
        
        options.SetActive(false);
        optionsBackground.SetActive(false);

        mainPanelBgColorAnim.SetInteger("panel", 0);
    }

    public void DisableSubMenusFeedbacks()
    {
        if (items.activeSelf)
        {
            AnimatorStateInfo currentAnimation = itemsButtonsAnim.GetCurrentAnimatorStateInfo(0);
            if (currentAnimation.IsName("ItemsPanelButtons_idleOn") | (currentAnimation.normalizedTime > 0.9f && currentAnimation.IsName("ItemsPanelButtons_Open")))
            {
                itemsButtonsAnim.CrossFade("ItemsPanelButtons_Close", 0);
            }
            else
            {
                itemsButtonsAnim.CrossFade("ItemsPanelButtons_idleOff", 0);
            }
        }

        if (costumes.activeSelf)
        {
            AnimatorStateInfo currentAnimation = costumesButtonsAnim.GetCurrentAnimatorStateInfo(0);
            if (currentAnimation.IsName("CostumesPanelButtons_idleOn") | (currentAnimation.normalizedTime > 0.9f && currentAnimation.IsName("CostumesPanelButtons_Open")))
            {
                costumesButtonsAnim.CrossFade("CostumesPanelButtons_Close", 0);
            }
            else
            {
                costumesButtonsAnim.CrossFade("CostumesPanelButtons_idleOff", 0);
            }
        }

        if (memories.activeSelf)
        {
            AnimatorStateInfo currentAnimation = memoriesButtonsAnim.GetCurrentAnimatorStateInfo(0);
            if (currentAnimation.IsName("MemoriesPanelButtons_idleOn") | (currentAnimation.normalizedTime > 0.9f && currentAnimation.IsName("MemoriesPanelButtons_Open")))
            {
                memoriesButtonsAnim.CrossFade("MemoriesPanelButtons_Close", 0);
            }
            else
            {
                memoriesButtonsAnim.CrossFade("MemoriesPanelButtons_idleOff", 0);
            }
        }

        if (options.activeSelf)
        {
            AnimatorStateInfo currentAnimation = optionsButtonsAnim.GetCurrentAnimatorStateInfo(0);
            if (currentAnimation.IsName("OptionsPanelButtons_idleOn") | (currentAnimation.normalizedTime > 0.9f && currentAnimation.IsName("OptionsPanelButtons_Open")))
            {
                optionsButtonsAnim.CrossFade("OptionsPanelButtons_Close", 0);
            }
            else
            {
                optionsButtonsAnim.CrossFade("OptionsPanelButtons_idleOff", 0);
            }
        }
    }

    public void LoadMainMenuScene()
    {
        inputReader.ChangeInputState(false);
        EventSystem.current.SetSelectedGameObject(null);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseManager.canBePaused = false;
        levelLoader.LoadLevel(sceneToLoadMenu);
        //SceneManager.LoadScene(sceneToLoadMenu);
    }

    public IEnumerator ShowCreditsCoroutine()
    {
        Scene scene = SceneManager.GetActiveScene();

        defaultPanel.SetActive(false);

        navigationIcon.SetActive(false);
        credits.SetActive(true);

        yield return new WaitForSecondsRealtime(2);

        credits.SetActive(false);
        navigationIcon.SetActive(true);
    }

    public void SetActiveItems(bool state)
    {
        items.SetActive(state);
        itemsBackground.SetActive(state);

        costumes.SetActive(!state);
        costumesBackground.SetActive(!state);
        memories.SetActive(!state);
        memoriesBackground.SetActive(!state);
        options.SetActive(!state);
        optionsBackground.SetActive(!state);

        pauseCharacterHandler.UpdatePauseMenu(1);
        mainPanelBgColorAnim.SetInteger("panel", 1);
        pauseManager.currentMenuText.text = itemsButton.GetComponentInChildren<TMP_Text>().text;

        EventSystem.current.SetSelectedGameObject(null);
        if (state)
        {
            EventSystem.current.SetSelectedGameObject(itemsFirstButton);
            itemsButtonsAnim.CrossFade("ItemsPanelButtons_Open", 0);
            navigationIconAnim.CrossFade("NavigationIcon_ItemsAnimation", 0);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(pauseManager.SetActiveMainPauseMenu);
            pauseManager.isOnMainPanel = false;
            pauseManager.afterBackTimer = 1;
            //itemsFirstButton.GetComponent<ChangeSubMenu>().ShowGameObject();
            changeItemNavigation.ChangeItemNav(true);
            //StartCoroutine(SetActiveItemsCoroutine());
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseMenuButton);
        }
    }

    private IEnumerator SetActiveItemsCoroutine()
    {
        AnimatorStateInfo currentAnimation = itemsButtonsAnim.GetCurrentAnimatorStateInfo(0);
        EventSystem.current.SetSelectedGameObject(null);

        while (currentAnimation.normalizedTime < 0.9f && currentAnimation.IsName("ItemsPanelButtons_Open"))
        {
            yield return null;
        }
        EventSystem.current.SetSelectedGameObject(itemsFirstButton);

    }

    public void SetActiveCostumes(bool state)
    {
        costumes.SetActive(state);
        costumesBackground.SetActive(state);

        items.SetActive(!state);
        itemsBackground.SetActive(!state);
        memories.SetActive(!state);
        memoriesBackground.SetActive(!state);
        options.SetActive(!state);
        optionsBackground.SetActive(!state);

        pauseCharacterHandler.UpdatePauseMenu(2);
        mainPanelBgColorAnim.SetInteger("panel", 2);
        pauseManager.currentMenuText.text = costumesButton.GetComponentInChildren<TMP_Text>().text;

        EventSystem.current.SetSelectedGameObject(null);
        if (state)
        {
            EventSystem.current.SetSelectedGameObject(costumesFirstButton);
            costumesButtonsAnim.CrossFade("CostumesPanelButtons_Open", 0);
            navigationIconAnim.CrossFade("NavigationIcon_CostumesAnimation", 0);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(pauseManager.SetActiveMainPauseMenu);
            pauseManager.isOnMainPanel = false;
            //CostumeHandler.Instance.PauseUICostumeSwitch(true);
            pauseManager.afterBackTimer = 1;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseMenuButton);
        }
    }

    public void SetActiveMemories(bool state)
    {
        memories.SetActive(state);
        memoriesBackground.SetActive(state);

        items.SetActive(!state);
        itemsBackground.SetActive(!state);
        costumes.SetActive(!state);
        costumesBackground.SetActive(!state);
        options.SetActive(!state);
        optionsBackground.SetActive(!state);

        pauseCharacterHandler.UpdatePauseMenu(3);
        mainPanelBgColorAnim.SetInteger("panel", 3);
        pauseManager.currentMenuText.text = memoriesButton.GetComponentInChildren<TMP_Text>().text;

        EventSystem.current.SetSelectedGameObject(null);
        if (state)
        {
            EventSystem.current.SetSelectedGameObject(memoriesFirstButton);
            memoriesButtonsAnim.CrossFade("MemoriesPanelButtons_Open", 0);
            navigationIconAnim.CrossFade("NavigationIcon_MemoriesAnimation", 0);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(pauseManager.SetActiveMainPauseMenu);
            pauseManager.isOnMainPanel = false;
            pauseManager.afterBackTimer = 1;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseMenuButton);
        }
    }

    public void SetActiveOptions(bool state)
    {
        options.SetActive(state);
        optionsBackground.SetActive(state);

        items.SetActive(!state);
        itemsBackground.SetActive(!state);
        costumes.SetActive(!state);
        costumesBackground.SetActive(!state);
        memories.SetActive(!state);
        memoriesBackground.SetActive(!state);

        pauseCharacterHandler.UpdatePauseMenu(4);
        mainPanelBgColorAnim.SetInteger("panel", 4);
        pauseManager.currentMenuText.text = optionsButton.GetComponentInChildren<TMP_Text>().text;

        InputReader.Instance.ReloadMovementActions();

        EventSystem.current.SetSelectedGameObject(null);
        if (state)
        {
            EventSystem.current.SetSelectedGameObject(optionsFirstButton);
            optionsButtonsAnim.CrossFade("OptionsPanelButtons_Open", 0);
            navigationIconAnim.CrossFade("NavigationIcon_OptionsAnimation", 0);
            goBackButton.onClick.RemoveAllListeners();
            goBackButton.onClick.AddListener(pauseManager.SetActiveMainPauseMenu);
            pauseManager.isOnMainPanel = false;
            pauseManager.afterBackTimer = 1;

            optionsFirstButton.GetComponent<ChangeSubMenu>().ShowGameObject();
            changeOptionNavigation.ChangeOptionNav(0);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(pauseMenuButton);
            optionValues.SaveConfiguration();
        }
    }

    public void SaveSaveFile()
    {        
        SaveManager.Instance.Save(SaveManager.Instance.currentSaveFile);
    }
}