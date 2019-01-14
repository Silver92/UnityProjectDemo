using UnityEngine;

public class FlowManger : MonoBehaviour
{
    #region Public Field
    [Tooltip("Manager to set up the languages")]
    public LocalizationManager localizationManager;
    [Tooltip("Manager to control the UI display")]
    public UIController uIController;
    [Tooltip("Manager to control the AR behaviours")]
    public ARManager aRManager;
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------

    /// <summary>
    /// To subscribe all the delegates of among the managers.
    /// </summary>
    private void Start()
    {
        UIControllerSubscription(true);
        ARManagerSubscription(true);
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To unsubscribe all the delegates of among the managers.
    /// </summary>
    private void OnDisable()
    {
        UIControllerSubscription(false);
        ARManagerSubscription(false);
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To manage the subscriptions in the UI controller.
    /// </summary>
    /// <param name="isSub"></param>
    private void UIControllerSubscription(bool isSub)
    {
        if (isSub)
        {
            uIController.LanguageSeletedAsked +=
                localizationManager.LoadLocalizedText;
            uIController.displayTextAsked +=
                localizationManager.GetLocalizedValue;
            uIController.enableARTrackingAsked +=
                aRManager.EnableARTracking;
            uIController.disableARTrackingAsked +=
                aRManager.DisableARTracking;
        }
        else
        {
            uIController.LanguageSeletedAsked -=
                localizationManager.LoadLocalizedText;
            uIController.displayTextAsked -=
                localizationManager.GetLocalizedValue;
            uIController.enableARTrackingAsked -=
                aRManager.EnableARTracking;
            uIController.disableARTrackingAsked -=
                aRManager.DisableARTracking;
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To manage the subscriptions in the AR manager.
    /// </summary>
    /// <param name="isSub"></param>
    private void ARManagerSubscription(bool isSub)
    {
        if (isSub)
        {
            aRManager.threadMessagePassAsked +=
                uIController.DisplayThreadText;
            aRManager.deleteCurrentThreadAsked +=
                uIController.DeleteCurrentThreadTexts;
        }
        else
        {
            aRManager.threadMessagePassAsked -=
                uIController.DisplayThreadText;
            aRManager.deleteCurrentThreadAsked -=
                uIController.DeleteCurrentThreadTexts;
        }
    }

    //-------------------------------------------------------------------------
    #endregion
}
