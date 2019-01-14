using UnityEngine;

public class TrackableEventHandler : DefaultTrackableEventHandler 
{
    #region Public Field
    [Range(0.5f, 3f)]
    [Tooltip("The delay time before activating the AR effects")]
    public float delayTime = 1f;
    [Tooltip("All the information to be sent to the UI part.")]
    public ARMessages aRMessages;
    #endregion

    #region Private Field
    /// <summary>
    /// The animation to play the animations.
    /// </summary>
    private Animator _animator;
    /// <summary>
    /// Count the current time to chech the time threshold.
    /// </summary>
    private float _currentTime = 0;
    /// <summary>
    /// To see if the vuforia has tracked the picture.
    /// </summary>
    private bool _isTracking = false;
    /// <summary>
    /// To see if the time threshold has been reached.
    /// </summary>
    private bool _isPlayingEffects = false;
    #endregion

    #region Events
    /// <summary>
    /// Sending thread messages to the UI part (time + text key).
    /// </summary>
    /// <param name="aRMessages"></param>
    public delegate void OnSendThreadMessageAsked(ARMessages aRMessages);
    public OnSendThreadMessageAsked sendThreadMessageAsked;
    /// <summary>
    /// Ask to delete the current displayed texts on the UI thread.
    /// </summary>
    public delegate void OnDeleteCurrentThreadAsked();
    public OnDeleteCurrentThreadAsked deleteCurrentThreadAsked;
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------

    /// <summary>
    /// Get the animator and the AR messages.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Tracking Management
    //-------------------------------------------------------------------------

    /// <summary>
    /// Check if the vuforia tracks the picture to perform the delay and play
    /// the effects.
    /// </summary>
    private void Update()
    {
        // When the picture gets tracked.
        if (_isTracking) 
        {
            _currentTime += Time.deltaTime;
            // If the time reaches the threshold.   
            if (_currentTime >= delayTime && !_isPlayingEffects)
            {
                // lock the current statement. Prevent from multiple processing.
                _isPlayingEffects = true;

                PlayAnimation(true);
                // Try to make it start to play at very beginning every time.
                _animator.Play(0, -1, 0);

                sendThreadMessageAsked(aRMessages);
            }
        } 
        // If the picture get lost.
        else
        {
            // Reset the time counting.
            _currentTime = 0;
            // If the time passed the threshold all the effects should be set to 
            // their original states.
            if (_isPlayingEffects) 
            {
                PlayAnimation(false);

                deleteCurrentThreadAsked();
                
                // Release the tracking lock and lock the reset part.
                _isPlayingEffects = false;
            }
        }
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Enable the animation objects renderers and the animator.
    /// </summary>
    /// <param name="isOn"></param>
    private void PlayAnimation(bool state)
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        foreach (var component in rendererComponents)
            component.enabled = state;
        _animator.enabled = state;
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// Activate the tracking process.
    /// </summary>
    protected override void OnTrackingFound()
    {
        _isTracking = true;
    }
    
    //-------------------------------------------------------------------------
    
    /// <summary>
    /// Close the tracking process.
    /// </summary>
    protected override void OnTrackingLost()
    {
        _isTracking = false;
    }

    //-------------------------------------------------------------------------

    /// <summary>
    /// To reset some settings when the tracking process is disabled.
    /// </summary>
    private void OnDisable()
    {
        PlayAnimation(false);
        _currentTime = 0;
        _isPlayingEffects = false;
    }

    //-------------------------------------------------------------------------
    #endregion

}
