using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Object = UnityEngine.Object;

/// <summary>
/// An enumeration that holds every sound effect in the game.
/// </summary>
public enum SoundEffect
{
    GunShot,
    BulletHitWall,
    Dash,
    Jump,
    BossShoot,
    Explosion,
    NukeFall,
}

public class AudioManager
{
    static AudioManager instance;
    float soundPitchMin = .75f;
    float soundPitchMax = 1.0f;

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
    /// Singleton for the audiomanager class
    /// </summary>
    public static AudioManager Instance
    { get { return instance ?? (instance = new AudioManager()); } }

    /// <summary>
    /// Constructor for the audiomanager
    /// </summary>
    private AudioManager()
    {
        SoundEffects = new Dictionary<SoundEffect, AudioClip>();

        //Create a temporary dictionary that loads all of the Audio files from a specific location.
        //Key = name of file, Value = file itself.
        Dictionary<string, AudioClip> clips = Resources.LoadAll<AudioClip>(Constants.AUDIO_FILE_LOCATION).ToDictionary(t => t.name);

        //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
        foreach (KeyValuePair<string, AudioClip> c in clips)
        {
            SoundEffects.Add((SoundEffect)Enum.Parse(typeof(SoundEffect), c.Key, true), c.Value);
        }

        //Creates a single sound effect source. Can play every sound in the game through this unless you want to have different effects
        //such as different pitch/volume for different sources.
        SoundEffectSource = new GameObject("SoundEffectSource", typeof(AudioSource)).GetComponent<AudioSource>();
        Object.DontDestroyOnLoad(SoundEffectSource.gameObject);

        //Creates a single background music source.
        BGMSource = new GameObject("BGMSource", typeof(AudioSource)).GetComponent<AudioSource>();
        BGMSource.volume = .5f;
        BGMSource.loop = true;
        Object.DontDestroyOnLoad(BGMSource.gameObject);

        ChangeBGM(Resources.Load<AudioClip>("Sound/Music/DancingMidgets"));
    }

    /// <summary>
    /// Updates the audio manager
    /// </summary>
    public void Update()
    {
        //Slows the pitch of the audio down whenenver the player activates the slow time feature.
        SoundEffectSource.pitch = Mathf.Lerp(soundPitchMin, soundPitchMax, (Time.timeScale - .5f) * 2);
        //BGMSource.pitch = Mathf.Lerp(soundPitchMin, soundPitchMax, (Time.timeScale - .5f) * 2);
    }

    /// <summary>
    /// Plays a single sound effect
    /// </summary>
    /// <param name="sound">The sound effect we want to play</param>
    public void PlayOneShot(SoundEffect sound, float volumeScale = 1)
    {
        SoundEffectSource.PlayOneShot(SoundEffects[sound], volumeScale * Constants.AUDIO_SOUND_EFFECT_VOLUME_MULTIPLIER);
    }

    /// <summary>
    /// Changes the BGM to something else
    /// </summary>
    /// <param name="sound">The bgm sound we want to play</param>
    public void ChangeBGM(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }
}