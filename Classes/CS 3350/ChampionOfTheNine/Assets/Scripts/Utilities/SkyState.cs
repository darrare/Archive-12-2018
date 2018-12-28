using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Class that holds sky state information
/// </summary>
public class SkyState
{
    #region Fields

    SkyStateType nextState;
    Color startSkyColor;
    Color endSkyColor;
    Color startDarkness;
    Color endDarkness;
    float timeInState;
    float startVolume;
    float endVolume;
    AudioClip endAudio;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="nextState">the state after this state</param>
    /// <param name="startSkyColor">the starting sky color for this state</param>
    /// <param name="endSkyColor">the ending sky color for this state</param>
    /// <param name="timeInState">the time to spend in this state</param>
    /// <param name="startDarkness">the starting darkness color for this state</param>
    /// <param name="endDarkness">the ending darkness color for this state</param>
    /// <param name="startVolume">the starting background volume for this state (defaults to maximum)</param>
    /// <param name="endVolume">the ending background volume for this state (defaults to maximum)</param>
    /// <param name="endAudio">the background sound to switch to at the end of this state (default to null, no change)</param>
    public SkyState(SkyStateType nextState, Color startSkyColor, Color endSkyColor, float timeInState, Color startDarkness, Color endDarkness,
        float startVolume = Constants.BGM_MAX_VOLUME, float endVolume = Constants.BGM_MAX_VOLUME, AudioClip endAudio = null)
    {
        this.nextState = nextState;
        this.startSkyColor = startSkyColor;
        this.endSkyColor = endSkyColor;
        this.timeInState = timeInState;
        this.startDarkness = startDarkness;
        this.endDarkness = endDarkness;
        this.startVolume = startVolume;
        this.endVolume = endVolume;
        this.endAudio = endAudio;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the starting sky color for this state
    /// </summary>
    public Color StartSkyColor
    {
        get { return startSkyColor; }
    }

    /// <summary>
    /// Gets the ending sky color for this state
    /// </summary>
    public Color EndSkyColor
    {
        get { return endSkyColor; }
    }

    /// <summary>
    /// Gets the time to spend in this state
    /// </summary>
    public float TimeInState
    {
        get { return timeInState; }
    }

    /// <summary>
    /// Gets the state after this state
    /// </summary>
    public SkyStateType NextState
    {
        get { return nextState; }
    }

    /// <summary>
    /// Gets the starting darkness color for this state
    /// </summary>
    public Color StartDarkness
    {
        get { return startDarkness; }
    }

    /// <summary>
    /// Gets the ending darkness color for this state
    /// </summary>
    public Color EndDarkness
    {
        get { return endDarkness; }
    }

    /// <summary>
    /// Gets the starting background volume for this state
    /// </summary>
    public float StartVolume
    {
        get { return startVolume; }
    }

    /// <summary>
    /// Gets the ending background volume for this state
    /// </summary>
    public float EndVolume
    {
        get { return endVolume; }
    }

    /// <summary>
    /// Gets the background sound to switch to at the end of this state
    /// </summary>
    public AudioClip EndAudio
    {
        get { return endAudio; }
    }

    #endregion
}
