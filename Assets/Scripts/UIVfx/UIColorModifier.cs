using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorModifier : MonoBehaviour
{
    public Image energyGaugeImage, energyGaugeFaceImage;
    //public RawImage menuImage;
    [Space(10)]
    public SkinnedMeshRenderer soulRenderer;
    [Space(10)]
    public Animator soulAnim, mainPanelBgColorAnim;
    [Space(10)]
    public Material blueSoulMaterial, yellowSoulMaterial, redSoulMaterial, whiteSoulMaterial, darkSoulMaterial;
    [Space(10)]
    public Color colorBlueGauge, colorYellowGauge, colorRedGauge, colorDarkGauge, colorWhiteGauge, colorEmptyGauge;
    [Space(10)]
    public Color colorBlueMenu, colorYellowMenu, colorRedMenu, colorDarkMenu, colorWhiteMenu;
    private bool hasLowHealth;
    [Space(10)]
    public Color colorBlueUi, colorYellowUi, colorRedUi, colorDarkUi, colorWhiteUi;
    public UIPauseCharacterHandler characterHandler;

    public void UpdateColor(int colorValue)
    {
        switch (colorValue)
        {
            case 5:
                energyGaugeImage.color = colorBlueGauge;
                energyGaugeFaceImage.color = colorBlueGauge;
                //menuImage.color = colorBlueMenu;
                //characterHandler.starUiImage.color = colorBlueUi;

                soulRenderer.material = blueSoulMaterial;

                break;
            case 4:
                energyGaugeImage.color = colorYellowGauge;
                energyGaugeFaceImage.color = colorYellowGauge;
                //menuImage.color = colorYellowMenu;
                //characterHandler.starUiImage.color = colorYellowUi;

                soulRenderer.material = yellowSoulMaterial;

                break;
            case 3:
                energyGaugeImage.color = colorRedGauge;
                energyGaugeFaceImage.color = colorRedGauge;
                //menuImage.color = colorRedMenu;
                //characterHandler.starUiImage.color = colorRedUi;

                soulRenderer.material = redSoulMaterial;

                break;
            case 1:
                energyGaugeImage.color = colorDarkGauge;
                energyGaugeFaceImage.color = colorDarkGauge;
                //menuImage.color = colorDarkMenu;
                //characterHandler.starUiImage.color = colorDarkUi;

                soulRenderer.material = darkSoulMaterial;

                hasLowHealth = true;

                break;
            case -1:
                energyGaugeImage.color = colorEmptyGauge;
                energyGaugeFaceImage.color = colorEmptyGauge;

                break;
            default:
                energyGaugeImage.color = colorWhiteGauge;
                energyGaugeFaceImage.color = colorWhiteGauge;
                //menuImage.color = colorWhiteMenu;
                //characterHandler.starUiImage.color = colorWhiteUi;

                soulRenderer.material = whiteSoulMaterial;

                break;
        }

        if (colorValue != 1) hasLowHealth = false;

        characterHandler.hasLowHealth = hasLowHealth;
        soulAnim.SetBool("hasLowHealth", hasLowHealth);
        mainPanelBgColorAnim.SetInteger("color", colorValue);
    }
}
