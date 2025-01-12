using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NavigationIcon : MonoBehaviour
{
    public EventSystem eventSystem;
    private RectTransform rectTransform;
    private RectTransform targetRectTransform;
    private RawImage rawImage;
    public float navSpeed = 1;
    public float rotationSpeed;

    private bool invertRotation;

    private Animator selectedAnimator;
    private GameObject selectedObject;

    private GameObject goBackButton;
    public bool hideInPlay;
    private HealthManager healthManager;

    [Space(10)]
    public Vector2 togglePosition;
    public Vector2 sliderPosition;
    public Vector2 sliderAudioPosition;
    public Vector2 dropdownPosition;

    [Space(10)]
    public Animator highlightAnim;
    private InputReader inputReader;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = this.GetComponent<RectTransform>();
        rawImage = this.GetComponent<RawImage>();

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "MainMenu")
        {
            healthManager = HealthManager.Instance;
        }
        inputReader = InputReader.Instance;

        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
        {
            if (button.gameObject.name != "GoBackButton") button.onClick.AddListener(ValidateNavigationIconAnimation);
        }

        Toggle[] toggles = FindObjectsOfType<Toggle>(true);
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener((bool isOn) => ValidateNavigationIconAnimation());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (hideInPlay && Time.timeScale > 0 | (scene.name != "MainMenu" && healthManager.isRespawning)) rawImage.enabled = false;
        else rawImage.enabled = true;

        if (rectTransform.eulerAngles.z > 30) invertRotation = true;
        else if (rectTransform.eulerAngles.z < 20) invertRotation = false;

        if (invertRotation)
        {
            rectTransform.Rotate(new Vector3(0, 0, -rotationSpeed * Time.unscaledDeltaTime));
            //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -animationSpeed));
        }
        else
        {
            rectTransform.Rotate(new Vector3(0, 0, rotationSpeed * Time.unscaledDeltaTime));
            //eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, animationSpeed));
        }

        if (eventSystem.currentSelectedGameObject != goBackButton)
        {
            if (eventSystem.currentSelectedGameObject)
            {
                targetRectTransform = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();

                Toggle toggle = eventSystem.currentSelectedGameObject.GetComponentInChildren<Toggle>();
                Slider slider = eventSystem.currentSelectedGameObject.GetComponentInChildren<Slider>();

                if (toggle != null)
                {
                    targetRectTransform = toggle.GetComponent<RectTransform>();
                }
                else if (slider != null)
                {
                    targetRectTransform = slider.transform.parent.GetComponent<RectTransform>();
                }

                ChangeString changeString = targetRectTransform.gameObject.GetComponent<ChangeString>();

                if (changeString)
                {
                    if (changeString.effectOnHover)
                    {
                        if (changeString.enabled)
                        {
                            changeString.ShowString();
                        }
                        else
                        {
                            changeString.ShowHiddenString();
                        }
                    }
                }

                ChangeStringList changeStringList = targetRectTransform.gameObject.GetComponent<ChangeStringList>();

                if (changeStringList)
                {
                    if (changeStringList.effectOnHover)
                    {
                        changeStringList.SelectChangeString();
                    }
                }

                ChangeOptionNavigationButton changeOptionNavigationButton = targetRectTransform.gameObject.GetComponent<ChangeOptionNavigationButton>();

                if (changeOptionNavigationButton)
                {
                    changeOptionNavigationButton.ChangeOptionNav();
                }
            }
            //rectTransform.anchoredPosition = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;
            if (selectedObject != null)
            {
                if (selectedObject != eventSystem.currentSelectedGameObject)
                {
                    selectedAnimator.SetBool("isHighlighted", false);
                    //highlightAnim.CrossFade("NavigationIcon_Move", 0);
                }
                /*else
                {
                    highlightAnim.CrossFade("NavigationIcon_Idle", 0.05f);
                }*/
            }
            selectedObject = eventSystem.currentSelectedGameObject;
            if (selectedObject)
            {
                Toggle toggle = selectedObject.GetComponentInChildren<Toggle>();
                if (toggle != null && selectedObject.name != "AutoSaveButton")
                {
                    selectedAnimator = toggle.transform.Find("CheckboxBackground").GetComponent<Animator>();
                }
                else
                {
                    selectedAnimator = selectedObject.GetComponent<Animator>();
                }
            }
            
            selectedAnimator.SetBool("isHighlighted", true);

            if (eventSystem.currentSelectedGameObject)
            {
                Toggle toggle = targetRectTransform.GetComponentInChildren<Toggle>();
                Slider slider = eventSystem.currentSelectedGameObject.GetComponentInChildren<Slider>();
                TMP_Dropdown dropdown = eventSystem.currentSelectedGameObject.GetComponent<TMP_Dropdown>();
                if (toggle != null && selectedObject.name != "AutoSaveButton")
                {
                    rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetRectTransform.anchoredPosition + togglePosition, navSpeed * Time.unscaledDeltaTime);
                }
                else if (slider != null)
                {
                    RectTransform handleRect = slider.handleRect;
                    if (selectedObject.name == "AudioSlider")
                    {
                        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetRectTransform.anchoredPosition + sliderAudioPosition + handleRect.anchorMin * 10, navSpeed * Time.unscaledDeltaTime);
                    }
                    else
                    {
                        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetRectTransform.anchoredPosition + sliderPosition + handleRect.anchorMin * 10, navSpeed * Time.unscaledDeltaTime);
                    }
                }
                else if (dropdown != null)
                {
                    rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetRectTransform.anchoredPosition + dropdownPosition, navSpeed * Time.unscaledDeltaTime);
                }
                else
                {
                    rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, targetRectTransform.anchoredPosition, navSpeed * Time.unscaledDeltaTime);
                }
            }
        }
    }

    public void MoveCursorToSelectedItem()
    {
        StartCoroutine(MoveCursorToSelectedItemCo());

        //rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition, 100);

        //rectTransform.anchoredPosition = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition;

        /*Vector3 screenPosition = Camera.main.WorldToScreenPoint(eventSystem.currentSelectedGameObject.transform.position);

        this.transform.position = screenPosition;*/
    }

    IEnumerator MoveCursorToSelectedItemCo()
    {
        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition, 100);

        yield return new WaitForSecondsRealtime(0.3f);

        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, eventSystem.currentSelectedGameObject.GetComponent<RectTransform>().anchoredPosition, 100);
    }

    public void ValidateNavigationIconAnimation()
    {
        if (highlightAnim.isActiveAndEnabled)
        {
            Button button = targetRectTransform.GetComponent<Button>();
            if (button)
            {
                if (button.interactable)
                {
                    highlightAnim.CrossFade("NavigationIcon_Move", 0);
                }
            }
            else
            {
                highlightAnim.CrossFade("NavigationIcon_Move", 0);
            }
        }
    }
}
