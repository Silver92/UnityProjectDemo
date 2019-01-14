using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Public fields
    [Header("Audio Clips")]
    [Tooltip("Sound clip played in the main track.")]
    public AudioClip audio1;
    [Tooltip("Sound clip played in the main track.")]
    public AudioClip audio2;
    [Tooltip("Sound clip played in the main track.")]
    public AudioClip audio3;
    [Tooltip("Sound clip played in the main track.")]
    public AudioClip audio4;
    [Tooltip("Sound clip played in the main track.")]
    public AudioClip audio5;
    #endregion

    #region Private fields
    /// <summary>
    /// The main track.
    /// </summary>
    private AudioSource _soundPlayer;
    #endregion

    #region Initialization
    //-------------------------------------------------------------------------

    /// <summary>
    /// Get the main track of the player.
    /// </summary>
    private void Start()
    {
        _soundPlayer = GetComponent<AudioSource>();
    }

    //-------------------------------------------------------------------------
    #endregion

    #region Sound Management
    //-------------------------------------------------------------------------

    /// <summary>
    /// Audio selector used in the scripts.
    /// </summary>
    /// <param name="element">Sound type</param>
    public void PlaySound(GlobalEnum.SoundType element)
    {
        switch (element)
        {
            case GlobalEnum.SoundType.Audio1:
                _soundPlayer.clip = audio1;
                _soundPlayer.Play();
                break;

            case GlobalEnum.SoundType.Audio2:
                _soundPlayer.clip = audio2;
                _soundPlayer.Play();
                break;

            case GlobalEnum.SoundType.Audio3:
                _soundPlayer.clip = audio3;
                _soundPlayer.Play();
                break;

            case GlobalEnum.SoundType.Audio4:
                _soundPlayer.clip = audio4;
                _soundPlayer.Play();
                break;

            case GlobalEnum.SoundType.Audio5:
                _soundPlayer.clip = audio5;
                _soundPlayer.Play();
                break;
        }
    }

    //-------------------------------------------------------------------------
    #endregion
}
