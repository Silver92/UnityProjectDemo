using UnityEngine;

[System.Serializable]
public class LocalizationData 
{
    #region Array of messages
    [Tooltip("To save all the text data")]
    public LocalizationItem[] items;
    #endregion

}

[System.Serializable]
public class LocalizationItem
{
    #region Key + Value
    [Tooltip("Key to call the corresponding value")]
    public string key;
    [Tooltip("The text saved in diferrent languages as needed")]
    public string value;
    #endregion

}
