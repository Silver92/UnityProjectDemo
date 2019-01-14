using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class LanguageSelection : UnityEvent<SystemLanguage> { }

[RequireComponent(typeof(Button))]
public class ClickableLanguage : MonoBehaviour
{
    #region Public Fields
    [Header("Language Selection")]
    [Tooltip("The selected language")]
    public SystemLanguage selectedLanguage = SystemLanguage.Unknown;
    [Tooltip("Evoke the language selected method to load language")]
    public LanguageSelection languageSelection;
    #endregion

    #region Private Region
    /// <summary>
    /// Get the button object in the game object to trigger the language event.
    /// </summary>
    private Button _button;
    #endregion

    #region Initialization
    /// <summary>
    /// Get the button component and add listener to the button.
    /// </summary>
    public void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => 
        languageSelection.Invoke(selectedLanguage));
    }
    #endregion



}