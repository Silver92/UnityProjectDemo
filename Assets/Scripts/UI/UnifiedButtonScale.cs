using UnityEngine;

public class UnifiedButtonScale : MonoBehaviour {

    #region Public Field
    [Tooltip("References of all the menu buttons")]
    public Transform[] buttonPos;
    [Range(0, 1f)]
    [Tooltip("Manage the scale effect of the buttons")]
    public float scaleCoefficient = 0.01f;
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------

    /// <summary>
    /// Let the button of the default screen show up.
    /// </summary>
    private void Start()
    {
        ButtonScale(0);
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Menu Effect
    //-------------------------------------------------------------------------

    /// <summary>
    /// To shrink the other unselected butttons
    /// </summary>
    /// <param name="index"></param>
    public void ButtonScale(int index)
    {
        for (int i = 0; i < buttonPos.Length; i++)
        {
            if (i == index)
            {
                buttonPos[i].localScale =
                    new Vector3(1, 1, 1);
            }
            else
            {
                buttonPos[i].localScale = 
                    new Vector3(scaleCoefficient, scaleCoefficient, 1f);
            }
        }
    }

    //-------------------------------------------------------------------------
    #endregion
}
