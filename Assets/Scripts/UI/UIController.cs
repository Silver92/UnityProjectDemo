using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Public Fieild
    [Header("Main Panel UI")]
    [Tooltip("Visual button in the main screen")]
    public GameObject visualButton;
    [Tooltip("Setting button in the main screen")]
    public GameObject settingButton;
    [Tooltip("The place to initiate the text flow")]
    public Transform threadTextParent;
    [Tooltip("The text display template")]
    public GameObject threadTextTemplate;
    [Tooltip("The three dots animation part")]
    public GameObject threadAnimation;

    [Header("Main Button Panels")]
    [Tooltip("The visual panel after clicking the visual button")]
    public GameObject visualPanel;
    [Tooltip("The setting panel after clicking the setting button")]
    public GameObject settingPanel;
    
    [Header("Subscreen in the Visual Panel")]
    [Tooltip("All the sub panels in the visual panel")]
    public GameObject[] subPanels;
    [Tooltip("The renderers to change the materials and shaders")]
    public Renderer[] renderers;
    [Tooltip("Heart materials to be changed by the toggles")]
    public Material[] heartMaterials;
    
    [Header("Subscreens in the Setting Panel")]
    [Tooltip("The parent of all the sub buttons in the panel to control the display")]
    public GameObject settingButtons;
    [Tooltip("The sub panel to display the language sub section")]
    public GameObject languagePanel;
    [Tooltip("The sub panel to display the references sub section")]
    public GameObject referencesPanel;

    [Header("Fading Mask")]
    [Tooltip("A mask to fadin at the start of the app")]
    public CanvasGroup fadingMask;
    [Range(0.5f, 3f)]
    [Tooltip("The setting time to fade in the main screen")]
    public float fadingTime = 2f;
    #endregion

    #region Private Field
    /// <summary>
    /// Memorize the current coroutine on running.
    /// </summary>
    private Coroutine _currentCoroutine;
    #endregion

    #region Events
    /// <summary>
    /// Ask for updating the selected language.
    /// </summary>
    /// <param name="language">All the selected languate data.</param>
    public delegate void OnLanguageSeletedAsked(SystemLanguage language);
    public OnLanguageSeletedAsked LanguageSeletedAsked;
    /// <summary>
    /// Ask for the text to display in the current language.
    /// </summary>
    /// <param name="key">Key for the specific text.</param>
    /// <returns></returns>
    public delegate string OnDisplayTextAsked(string key);
    public OnDisplayTextAsked displayTextAsked;
    /// <summary>
    /// Ask to enable the AR tracking process.
    /// </summary>
    public delegate void OnEnableARTrackingAsked();
    public OnEnableARTrackingAsked enableARTrackingAsked;
    /// <summary>
    /// Ask to disenable the AR tracking procress.
    /// </summary>
    public delegate void OnDisableARTrackingAsked();
    public OnDisableARTrackingAsked disableARTrackingAsked;
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------
    /// <summary>
    /// Initialize all the default settings. Display the right UI.
    /// </summary>
    private void Start()
    {
    
        MainScreenInit(true);
        VisualPanelInit();
        SettingPanelInit();
        
        threadAnimation.SetActive(false);
        fadingMask.gameObject.SetActive(true);
        fadingMask.alpha = 1;
        CameraFadein();
        
    }

    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Mains the menu buttons init.
    /// </summary>
    /// <param name="state"> true = activate visual and setting buttons</param>
    private void MainScreenInit(bool state)
    {
        visualButton.SetActive(state);
        settingButton.SetActive(state);

        if (state)
        {
            enableARTrackingAsked();
        }
        else
        {
            DeleteCurrentThreadTexts();
            disableARTrackingAsked();
        }
        
    }

    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Set up the defualt visual panel display.
    /// </summary>
    private void VisualPanelInit()
    {
        visualPanel.SetActive(false);
        for (int i = 0; i < subPanels.Length; i++) 
        {
            subPanels[i].SetActive(false);
        }
        subPanels[0].SetActive(true);
    }

    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Set up the settign panel display.
    /// </summary>
    private void SettingPanelInit()
    {
        settingPanel.SetActive(false);
        settingButtons.SetActive(true);
        languagePanel.SetActive(false);
        referencesPanel.SetActive(false);
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Main Screen Functions
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Behaviours on visual button clicked.
    /// </summary>
    public void VisualButtonClicked()
    {
        MainScreenInit(false);
        visualPanel.SetActive(true);
    }

    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Behaviours on setting button clicked.
    /// </summary>
    public void SettingButtonClicked()
    {
        MainScreenInit(false);
        settingPanel.SetActive(true);
    }

    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Display the thread text. Use the coroutine to set up the waiting period.
    /// </summary>
    public void DisplayThreadText(ARMessages aRMessages)
    {
        _currentCoroutine = StartCoroutine(ShowText(aRMessages));
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Coroutine to run the text display flow.
    /// </summary>
    /// <param name="aRMessages">Data of the thread texts (time + key)</param>
    /// <returns></returns>
    private IEnumerator ShowText (ARMessages aRMessages) 
    {
        threadAnimation.SetActive(true);
        
        int currentAnimationIndex = 
            threadAnimation.transform.GetSiblingIndex();

        for (int i = 0; i < aRMessages.localizationSeries.
            localizationTexts.Length; i++)
        {
            // Perform the delay.
            yield return new WaitForSeconds(aRMessages.localizationSeries.localizationTexts[i].delayTime);
            // Generate the text frame.
            GameObject text = Instantiate
                (threadTextTemplate, threadTextParent);
            // Call the right text.
            text.GetComponentInChildren<Text>().text = 
                displayTextAsked(aRMessages.localizationSeries.localizationTexts[i].key);
            //// Move the animation down.
            //threadAnimation.transform.
            //    SetSiblingIndex(++currentAnimationIndex);
        }

        threadAnimation.SetActive(false);
    }
    
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Delete the previous texts before update the new thread.
    /// </summary>
    public void DeleteCurrentThreadTexts()
    {   
        // If the previous threa is still running, stop it.
        if (_currentCoroutine != null) 
        {
            StopCoroutine(_currentCoroutine);
            threadAnimation.SetActive(false);
        }
        
        // Destroy all the text templates.
        for (int i = 0; i < threadTextParent.childCount; i++) 
        {
            Destroy(threadTextParent.GetChild(i).gameObject);
        }
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Visual Panel Functions
    //--------------------------------------------------------------------------

    /// <summary>
    /// To close the visual screen and open the main screen buttons.
    /// </summary>
    public void VisualReturnClicked()
    {
        visualPanel.SetActive(false);
        MainScreenInit(true);
    }
    
    //--------------------------------------------------------------------------
    
    /// <summary>
    /// To display the clicked the screen and close all the others.
    /// </summary>
    /// <param name="index">The screen index in the array.</param>
    public void MenuButtonsClicked(int index)
    {
        for (int i = 0; i < subPanels.Length; i++) 
        {
            if (subPanels[i].activeSelf) 
            {
                subPanels[i].SetActive(false);
                break;
            }
        }
        subPanels[index].SetActive(true);
    }
    
    //--------------------------------------------------------------------------
    
    /// <summary>
    /// Change the current heart model's texture. To be able to see the effects
    /// here I use the material for the changes. In the future if I get the 
    /// appropriate textures I wil change that.
    /// </summary>
    /// <param name="index">Different texture indexs in the array.</param>
    public void HeartTogglesClicked(int index) 
    {
        renderers[(int)GlobalEnum.BodyType.Heart].sharedMaterial = 
            heartMaterials[index];
    }
    
    //--------------------------------------------------------------------------
    
    /// <summary>
    /// Change the material's shader with the slider.
    /// </summary>
    /// <param name="value">Control coefficient.</param>
    public void HeartSeverity(float value) 
    {
        renderers[(int)GlobalEnum.BodyType.Heart].material.
            SetFloat("_Metallic", value);
    }

    //--------------------------------------------------------------------------

    /// <summary>
    /// Change the material's shader with the slider.
    /// </summary>
    /// <param name="value">Control coefficient.</param>
    public void HandAge(float value) 
    {
        renderers[(int)GlobalEnum.BodyType.Hand].material.
            SetFloat("_Metallic", value);
    }

    //--------------------------------------------------------------------------

    /// <summary>
    /// Change the material's shader with the slider.
    /// </summary>
    /// <param name="value">Control coefficient.</param>
    public void HeadSlider(float value) 
    {
        renderers[(int)GlobalEnum.BodyType.Head].material.
            SetFloat("_Metallic", value);
    }

    //--------------------------------------------------------------------------

    /// <summary>
    /// Change the material's shader with the slider.
    /// </summary>
    /// <param name="value">Control coefficient.</param>
    public void EyeSlider(float value) 
    {
        renderers[(int)GlobalEnum.BodyType.Eye].material.
            SetFloat("_Metallic", value);
    }

    //--------------------------------------------------------------------------
    #endregion

    #region Setting Panel Functions
    //-------------------------------------------------------------------------

    /// <summary>
    /// Close the setting screen and open the language seletion screen.
    /// </summary>
    public void LanguageButtonClicked()
    {
        settingButtons.SetActive(false);
        languagePanel.SetActive(true);
    }
    
    //-------------------------------------------------------------------------

    /// <summary>
    /// Point to the website.
    /// </summary>
    public void TermOfUseClicked()
    {
        Application.
        OpenURL("https://www.persistant.fr/");
    }
    
    //-------------------------------------------------------------------------

    /// <summary>
    /// Close the setting screen and open the reference screen.
    /// </summary>
    public void ReferencesClicked()
    {
        settingButtons.SetActive(false);
        referencesPanel.SetActive(true);
    }
    
    //-------------------------------------------------------------------------

    /// <summary>
    /// Close the setting screen and display the two main menu buttons.
    /// </summary>
    public void SettingReturnClicked()
    {
        settingPanel.SetActive(false);
        MainScreenInit(true);
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Language Panel Functions
    //-------------------------------------------------------------------------

    /// <summary>
    /// Send the selected language choosing requirement and close the language
    /// screen, open the setting screen.
    /// </summary>
    /// <param name="language"></param>
    public void LocalizeLanguage(SystemLanguage language)
    {
        PlayerPrefs.SetString("Language", language.ToString());
        LanguageSeletedAsked(language);
        languagePanel.SetActive(false);
        settingButtons.SetActive(true);
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Close the language screen and open the setting screen.
    /// </summary>
    public void LanguageReturnClicked()
    {
        languagePanel.SetActive(false);
        settingButtons.SetActive(true);
    }

    //-------------------------------------------------------------------------
    #endregion

    #region References Panel Functions
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Close the reference screen and open the setting screen.
    /// </summary>
    public void ReferencesReturnClicked()
    {
        referencesPanel.SetActive(false);
        settingButtons.SetActive(true);
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Visual Effects Region
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Call a coroutine to fade in the camera at start.
    /// </summary>
    private void CameraFadein() 
    {
        StartCoroutine(Fade(1 ,0));
    }
    
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// The coroutine to perform the fading effect with the canvas group.
    /// </summary>
    /// <param name="start">Start alpha.</param>
    /// <param name="end">End alpha.</param>
    /// <returns></returns>
    private IEnumerator Fade(float start, float end) 
    {
        float elaspedTime = 0f;
        while (elaspedTime <= fadingTime) 
        {
            elaspedTime += Time.deltaTime;
            fadingMask.alpha = Mathf.
                Lerp(start, end, elaspedTime / fadingTime);
            yield return null;
        }
        fadingMask.alpha = end;
    }
    
    //-------------------------------------------------------------------------
    #endregion

}
