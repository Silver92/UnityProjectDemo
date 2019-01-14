using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalizationManager : MonoBehaviour
{
    #region Public Field
    [Tooltip("References of all the language files to check.")]
    public LanguageProfile[] languageProfiles;
    #endregion

    #region Private Field
    /// <summary>
    /// Dictionary to store the current dynamic language data.
    /// </summary>
    private Dictionary<string, string> _localizedText;
    /// <summary>
    /// All the static texts references in the app for dynamic changes.
    /// </summary>
    private List<LocalizationText> _texts = new List<LocalizationText>();
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------

    /// <summary>
    /// Get all the text fields in the applicaiotn loading.
    /// </summary>
    private void Awake()
    {
        GetFieldsToLocalize();
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Set up the default system language.
    /// </summary>
    private void Start()
    {
        LanguageInit();
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Check if the user has selected a language preference.
    /// </summary>
    private void LanguageInit()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            Debug.Log("Found previous saved language: " +
                PlayerPrefs.GetString("Language"));

            LanguageProfile languageProfile = 
                Array.Find(languageProfiles, 
                l => l.language.ToString() == 
                PlayerPrefs.GetString("Language"));

            LoadLocalizedText(languageProfile.language);
        }
        else
        {
            LoadLocalizedText(Application.systemLanguage);
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Load static texts.
    /// </summary>
    private void GetFieldsToLocalize()
    {
        GameObject[] gameObjects = SceneManager.
            GetActiveScene().GetRootGameObjects();

        foreach(GameObject gameObject in gameObjects)
        {
            _texts.AddRange(gameObject.
                GetComponentsInChildren<LocalizationText>(true));
        }
        
        Debug.Log("Current Static Text: " + _texts.Count);

    }

    //-------------------------------------------------------------------------
    #endregion

    #region Localizing Management
    //--------------------------------------------------------------------------

    /// <summary>
    /// Check if the system contains the language profile and load data.
    /// </summary>
    /// <param name="language"></param>
    public void LoadLocalizedText(SystemLanguage language =
         SystemLanguage.Unknown)
    {
        if (language == SystemLanguage.Unknown)
        {
            return;
        }

        LanguageProfile profile = Array.Find(languageProfiles, 
            x => x.language.Equals(language));

        if (profile != null)
        {
            LoadLocalizedData(profile.textFile);
        }
        else
        {
            Debug.LogErrorFormat("Language profile not found: " +
                "{0}", language);
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Load the selected language.
    /// </summary>
    /// <param name="languageFile">File saving the data for the selected 
    /// language</param>
    public void LoadLocalizedData(TextAsset languageFile)
    {

        _localizedText = new Dictionary<string, string> ();

        if (languageFile.text != "")
        {
            string dataAsJson = languageFile.text;
            LocalizationData loadedData = JsonUtility.
                FromJson<LocalizationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                _localizedText.Add(loadedData.items[i].key,
                    loadedData.items[i].value);
            }

            for (int i = 0; i < _texts.Count; i++)
            {
                _texts[i].text.text = GetLocalizedValue(_texts[i].key);
            }

            Debug.Log("Data loaded, dictionary contains: " +
                _localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Cannot find file!");
        }
    }
    
    //--------------------------------------------------------------------------
    
    /// <summary>
    /// Dynamically get the text of the selected language.
    /// </summary>
    /// <param name="key">The key sent to call the corresponding text.</param>
    /// <returns></returns>
    public string GetLocalizedValue(string key)
    {
        string result = "Localized text not found";
        if (_localizedText.ContainsKey (key)) 
        {
            result = _localizedText[key];
        }

        return result;

    }
    
    //--------------------------------------------------------------------------
    #endregion
}
