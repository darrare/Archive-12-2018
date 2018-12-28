using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// An enumeration that holds every sound effect in the game.
/// </summary>
public enum SoundEffect
{
    Waka1, Waka2,
    Powerup, //big pelet
    Bonus, //fruit
    Death, 
    EatGhost,
    ExtraLife,
    Intermission,
    StartGame,
    Siren,
}

public class AudioManager
{
    static AudioManager instance;

    /// <summary>
    /// A collection of all of the sound effects in the game.
    /// </summary>
    private Dictionary<SoundEffect, AudioClip> SoundEffects
    { get; set; }

    /// <summary>
    /// Source for the sound effects
    /// </summary>
    private AudioSource SoundEffectSource
    { get; set; }

    /// <summary>
    /// Source for the BGM
    /// </summary>
    private AudioSource BGMSource
    { get; set; }

    /// <summary>
    /// A bool that dictates whether we are on the first waka or the 2nd (true = first)
    /// </summary>
    public bool WakaToggle
    { get; set; }


    /// <summary>
    /// Singleton for the audiomanager class
    /// </summary>
    public static AudioManager Instance
    { get { return instance ?? (instance = new AudioManager()); } }

    /// <summary>
    /// Constructor for the audiomanager
    /// </summary>
    private AudioManager()
    {
        WakaToggle = true;

        SoundEffects = new Dictionary<SoundEffect, AudioClip>();
        Dictionary<string, AudioClip> clips = Resources.LoadAll<AudioClip>(Constants.AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
        foreach (KeyValuePair<string, AudioClip> c in clips.Where(t => Constants.SOUND_EFFECT_STRINGS.ContainsKey(t.Key)))
        {
            SoundEffects.Add(Constants.SOUND_EFFECT_STRINGS[c.Key], c.Value);
        }

        SoundEffectSource = new GameObject("SoundEffectSource", typeof(AudioSource)).GetComponent<AudioSource>();
        Object.DontDestroyOnLoad(SoundEffectSource.gameObject);

        BGMSource = new GameObject("BGMSource", typeof(AudioSource)).GetComponent<AudioSource>();
        Object.DontDestroyOnLoad(BGMSource.gameObject);
    }

    /// <summary>
    /// Updates the audio manager
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// Plays a single sound effect
    /// </summary>
    /// <param name="sound"></param>
    public void PlayOneShot(SoundEffect sound)
    {
        SoundEffectSource.PlayOneShot(SoundEffects[sound]);
    }

    /// <summary>
    /// Plays the right waka depending on other circumstances
    /// </summary>
    public void PlayWaka()
    {
        if (WakaToggle)
        {
            PlayOneShot(SoundEffect.Waka1);
        }
        else
        {
            PlayOneShot(SoundEffect.Waka2);
        }
        WakaToggle = !WakaToggle;
    }

    /// <summary>
    /// Changes the BGM to something else
    /// </summary>
    /// <param name="sound">The bgm sound we want to play</param>
    public void ChangeBGM(SoundEffect sound)
    {
        BGMSource.clip = SoundEffects[sound];
    }
}
