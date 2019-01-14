using UnityEngine;

[CreateAssetMenu(fileName = "AR Messages", menuName = "Test/Localization/AR_Messages", order = 1)]
public class LocalizationSeries : ScriptableObject
{
    #region Public Field
    [Tooltip("Message items gonna be tracked for the texts on the thread.")]
    public ARLocalizationText[] localizationTexts;
    #endregion
}


