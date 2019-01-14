using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizationText : MonoBehaviour {

    #region Public Region
    [Tooltip("The key of the local static text")]
    public string key;
    [Tooltip("The reference of the text object to be localized")]
    public Text text;
    #endregion
}
