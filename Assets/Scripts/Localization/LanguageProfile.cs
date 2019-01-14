using UnityEngine;

[CreateAssetMenu(fileName = "Language Profile", menuName = "Test/Localization/Language Profile", order = 3)]
public class LanguageProfile : ScriptableObject {

    #region Public Fields
    [Tooltip("Language tag of this profile")]
    public SystemLanguage language = SystemLanguage.Unknown;
    [Tooltip("Localization text file name")]
    public TextAsset textFile;
    #endregion
}
