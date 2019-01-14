using UnityEngine;

public class ARManager : MonoBehaviour
{
    #region Public Field
    [Tooltip("References of all the event handles")]
    public TrackableEventHandler[] eventHandlers;
    #endregion

    #region Events
    /// <summary>
    /// Sending thread messages to the UI part (time + text key).
    /// </summary>
    /// <param name="aRMessages">Time and text keys sent to the UI part.</param>
    public delegate void OnThreadMessagePassAsked(ARMessages aRMessages);
    public OnThreadMessagePassAsked threadMessagePassAsked;
    /// <summary>
    /// Ask to delete the current displayed texts on the UI thread.
    /// </summary>
    public delegate void OnDeleteCurrentThreadAsked();
    public OnDeleteCurrentThreadAsked deleteCurrentThreadAsked;
    #endregion

    #region Initialization
    //--------------------------------------------------------------------------
    
    /// <summary>
    /// Subscribe all the delegates from the event handlers.
    /// </summary>
    private void Start()
    {
        foreach (TrackableEventHandler eventHandler in eventHandlers)
        {
            eventHandler.sendThreadMessageAsked += AskThreadDisplay;
            eventHandler.deleteCurrentThreadAsked += AskDeleteThreadDisplay;
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Unsubscribe all the delegates from the event handlers.
    /// </summary>
    private void OnDisable()
    {
        foreach (TrackableEventHandler eventHandler in eventHandlers)
        {
            eventHandler.sendThreadMessageAsked -= AskThreadDisplay;
            eventHandler.deleteCurrentThreadAsked -= AskDeleteThreadDisplay;
        }
    }

    //--------------------------------------------------------------------------
    #endregion

    #region AR Management
    //--------------------------------------------------------------------------

    /// <summary>
    /// To enable the tracking process.
    /// </summary>
    public void EnableARTracking()
    {
        foreach(TrackableEventHandler eventHandler in eventHandlers)
        {
            eventHandler.enabled = true;
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To disable the tracking process.
    /// </summary>
    public void DisableARTracking()
    {
        foreach (TrackableEventHandler eventHandler in eventHandlers)
        {
            eventHandler.enabled = false;
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Ask for displaying texts on the UI thread.
    /// </summary>
    /// <param name="aRMessages">Time and text keys sent to the UI part.</param>
    private void AskThreadDisplay(ARMessages aRMessages) 
    {
        threadMessagePassAsked(aRMessages);
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Ask to delete the current displayed texts on the UI thread.
    /// </summary>
    private void AskDeleteThreadDisplay() 
    {
        deleteCurrentThreadAsked();
    }
    
    //--------------------------------------------------------------------------
    #endregion
}
