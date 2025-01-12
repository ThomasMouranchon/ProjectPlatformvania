using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHandler : MonoBehaviour
{
    private static ItemsHandler instance = null;
    public static ItemsHandler Instance => instance;


    public bool enableDash, enableGlide, enableWallJump, enableYoyo, enableSpeedBoost, enableBomb, enablePhantomDash;
    [Space(30)]

    public Button dashButton, glideButton, wallJumpButton, yoyoButton, speedBoostButton, bombButton, phantomDashButton;
    [Space(10)]

    public Image dashImage, glideImage, wallJumpImage, yoyoImage, speedBoostImage, bombImage, phantomDashImage;
    [Space(30)]

    public Button[] mainMenuModifiableButtons;

    public Image[] mainMenuModifiableButtonsImages;
    public TMP_Text[] mainMenuModifiableButtonsTexts;
    private Image[] itemsButtonsImages;
    private ChangeString[] itemsButtonsChangeString;

    private SaveManager saveManager;

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
        saveManager = SaveManager.Instance;

        mainMenuModifiableButtonsImages = new Image[mainMenuModifiableButtons.Length];
        for (int i = 0; i < mainMenuModifiableButtons.Length; i++)
        {
            mainMenuModifiableButtonsImages[i] = mainMenuModifiableButtons[i].gameObject.GetComponent<Image>();
        }

        mainMenuModifiableButtonsTexts = new TMP_Text[mainMenuModifiableButtons.Length];
        for (int i = 0; i < mainMenuModifiableButtons.Length; i++)
        {
            mainMenuModifiableButtonsTexts[i] = mainMenuModifiableButtons[i].gameObject.GetComponentInChildren<TMP_Text>();
        }

        itemsButtonsImages = new Image[7];
        itemsButtonsImages[0] = dashButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[1] = glideButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[2] = wallJumpButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[3] = yoyoButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[4] = speedBoostButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[5] = bombButton.gameObject.GetComponent<Image>();
        itemsButtonsImages[6] = phantomDashButton.gameObject.GetComponent<Image>();

        itemsButtonsChangeString = new ChangeString[7];
        itemsButtonsChangeString[0] = dashButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[1] = glideButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[2] = wallJumpButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[3] = yoyoButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[4] = speedBoostButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[5] = bombButton.gameObject.GetComponent<ChangeString>();
        itemsButtonsChangeString[6] = phantomDashButton.gameObject.GetComponent<ChangeString>();

        EnableDash(saveManager.enableDash);
        EnableGlide(saveManager.enableGlide);
        EnableWallJump(saveManager.enableWallJump);
        EnableYoyo(saveManager.enableYoyo);
        EnableSpeedBoost(saveManager.enableSpeedBoost);
        EnableBomb(saveManager.enableBomb);
        EnablePhantomDash(saveManager.enablePhantomDash);
        SwitchDashMode();
    }

    public void EnableDash(bool enabled)
    {
        enableDash = enabled;
        saveManager.enableDash = enabled;

        EnableDashButton(enabled);
    }

    private void EnableDashButton(bool enabled)
    {
        dashButton.interactable = enabled;
        dashImage.enabled = enabled;
        itemsButtonsImages[0].raycastTarget = enabled;
        itemsButtonsChangeString[0].enabled = enabled;
    }

    public void EnableGlide(bool enabled)
    {
        enableGlide = enabled;
        saveManager.enableGlide = enabled;

        glideButton.interactable = enabled;
        glideImage.enabled = enabled;
        itemsButtonsImages[1].raycastTarget = enabled;
        itemsButtonsChangeString[1].enabled = enabled;
    }

    public void EnableWallJump(bool enabled)
    {
        enableWallJump = enabled;
        saveManager.enableWallJump = enabled;

        wallJumpButton.interactable = enabled;
        wallJumpImage.enabled = enabled;
        itemsButtonsImages[2].raycastTarget = enabled;
        //itemsButtonsChangeString[2].enabled = enabled;
    }

    public void EnableYoyo(bool enabled)
    {
        enableYoyo = enabled;
        saveManager.enableYoyo = enabled;

        yoyoButton.interactable = enabled;
        yoyoImage.enabled = enabled;
        itemsButtonsImages[3].raycastTarget = enabled;
        //itemsButtonsChangeString[3].enabled = enabled;
    }
    
    public void EnableSpeedBoost(bool enabled)
    {
        enableSpeedBoost = enabled;
        saveManager.enableSpeedBoost = enabled;

        speedBoostButton.interactable = enabled;
        speedBoostImage.enabled = enabled;
        itemsButtonsImages[4].raycastTarget = enabled;
        //itemsButtonsChangeString[4].enabled = enabled;
    }
    
    public void EnableBomb(bool enabled)
    {
        enableBomb = enabled;
        saveManager.enableBomb = enabled;

        bombButton.interactable = enabled;
        bombImage.enabled = enabled;
        itemsButtonsImages[5].raycastTarget = enabled;
        //itemsButtonsChangeString[5].enabled = enabled;
    }

    public void EnablePhantomDash(bool enabled)
    {
        enablePhantomDash = enabled;
        saveManager.enablePhantomDash = enabled;

        EnablePhantomDashButton(enabled);
    }
    private void EnablePhantomDashButton(bool enabled)
    {
        phantomDashButton.interactable = enabled;
        phantomDashImage.enabled = enabled;
        itemsButtonsImages[6].raycastTarget = enabled;
        //itemsButtonsChangeString[6].enabled = enabled;
    }

    private void SwitchDashMode()
    {
        EnableDashButton(enableDash && !enablePhantomDash);
        EnablePhantomDashButton(enablePhantomDash);
    }
}