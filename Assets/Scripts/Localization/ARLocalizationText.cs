using UnityEngine;

[CreateAssetMenu(fileName = "Localization Text", menuName = "Test/Localization/Text", order = 2)]
public class ARLocalizationText : ScriptableObject
{
    #region Public Field
    [Header("Localization Key and Text")]
    [Tooltip("Localization key to find the corresponding text.")]
    public string key;
    [Tooltip("The text object reference to assign the key.")]
    public float delayTime;
    #endregion
}
