using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Object = UnityEngine.Object;

namespace Mediocracy
{
    /// <summary>
    /// An enumeration that holds every sound effect in the game.
    /// </summary>
    public enum AmbianceSounds
    {
        BirdChirp,
        BirdChirp2,
        CarAlarm,
        CarHorn,
        Crickets,
    }

    public enum SoundEffects
    {
        ArrowHit,
        ArrowMiss,
        GameLose,
        GameWin,
        MiniGameWin,
        TransponderClick,
        UIButtonClick,
        UIButtonHover
    }

    public enum GreySounds
    {
        DullLoop,
        DullLoop2,
        DullLoop3
    }

    public enum CrazySounds
    {
        CrazyLoop,
        CrazyLoop2,
        CrazyLoop3,
        CrazyLoop4,
        CrazyLoop5
    }

    public enum RandomVoices
    {
        Betcha,
        Cannibalism,
        Cocaine,
        Fappin,
        GlimGlam,
        LegoMovie,
        Mickey,
        Mickey2,
        Spaghett
    }

    public enum RandomVoicesGrey
    {
        Communism,
        Dictionary,
        Documentaries,
        KennyG,
        Multivitamins,
        Oatmeal,
        Staplers,
        TaxDollars
    }

    public class AudioManager
    {
        static AudioManager instance;

        /// <summary>
        /// A collection of all of the sound effects in the game.
        /// </summary>
        private Dictionary<AmbianceSounds, AudioClip> Ambiance
        { get; set; } = new Dictionary<AmbianceSounds, AudioClip>();

        /// <summary>
        /// A collection of all of the sound effects in the game.
        /// </summary>
        private Dictionary<SoundEffects, AudioClip> SoundEffects
        { get; set; } = new Dictionary<SoundEffects, AudioClip>();

        /// <summary>
        /// A collection of all of the sound effects in the game.
        /// </summary>
        private Dictionary<GreySounds, AudioClip> GreySounds
        { get; set; } = new Dictionary<GreySounds, AudioClip>();

        /// <summary>
        /// A collection of all of the sound effects in the game.
        /// </summary>
        private Dictionary<CrazySounds, AudioClip> CrazySounds
        { get; set; } = new Dictionary<CrazySounds, AudioClip>();

        private Dictionary<RandomVoices, AudioClip> RandomVoices
        { get; set; } = new Dictionary<RandomVoices, AudioClip>();

        private Dictionary<RandomVoicesGrey, AudioClip> RandomVoicesGrey
        { get; set; } = new Dictionary<RandomVoicesGrey, AudioClip>();

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
            //Create a temporary dictionary that loads all of the Audio files from a specific location.
            //Key = name of file, Value = file itself.
            Dictionary<string, AudioClip> ambiance = Resources.LoadAll<AudioClip>(Constants.AMBIANCE_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
            Dictionary<string, AudioClip> sfx = Resources.LoadAll<AudioClip>(Constants.SFX_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
            Dictionary<string, AudioClip> grey = Resources.LoadAll<AudioClip>(Constants.GREY_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
            Dictionary<string, AudioClip> crazy = Resources.LoadAll<AudioClip>(Constants.CRAZY_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
            Dictionary<string, AudioClip> voices = Resources.LoadAll<AudioClip>(Constants.VOICES_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);
            Dictionary<string, AudioClip> voicesgrey = Resources.LoadAll<AudioClip>(Constants.VOICESGREY_AUDIO_FILE_LOCATION).ToDictionary(t => t.name);

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in ambiance)
            {
                Ambiance.Add((AmbianceSounds)Enum.Parse(typeof(AmbianceSounds), c.Key, true), c.Value);
            }

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in sfx)
            {
                SoundEffects.Add((SoundEffects)Enum.Parse(typeof(SoundEffects), c.Key, true), c.Value);
            }

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in grey)
            {
                GreySounds.Add((GreySounds)Enum.Parse(typeof(GreySounds), c.Key, true), c.Value);
            }

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in crazy)
            {
                CrazySounds.Add((CrazySounds)Enum.Parse(typeof(CrazySounds), c.Key, true), c.Value);
            }

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in voices)
            {
                RandomVoices.Add((RandomVoices)Enum.Parse(typeof(RandomVoices), c.Key, true), c.Value);
            }

            //Iterates through the loaded sound files and adds them to the Enum to AudioClip dictionary.
            foreach (KeyValuePair<string, AudioClip> c in voicesgrey)
            {
                RandomVoicesGrey.Add((RandomVoicesGrey)Enum.Parse(typeof(RandomVoicesGrey), c.Key, true), c.Value);
            }

            //Creates a single sound effect source. Can play every sound in the game through this unless you want to have different effects such as different pitch/volume for different sources.
            SoundEffectSource = new GameObject("SoundEffectSource", typeof(AudioSource)).GetComponent<AudioSource>();
            Object.DontDestroyOnLoad(SoundEffectSource.gameObject);

            //Creates a single background music source.
            BGMSource = new GameObject("BGMSource", typeof(AudioSource)).GetComponent<AudioSource>();
            BGMSource.volume = Constants.AUDIO_BGM_VOLUME_DEFAULT;
            BGMSource.loop = true;
            Object.DontDestroyOnLoad(BGMSource.gameObject);

            //ChangeBGM(Resources.Load<AudioClip>("Sound/Music/DancingMidgets"));
        }

        float timeCounter = 0.0f;
        float randTime = 3.0f;
        /// <summary>
        /// Updates the audio manager
        /// </summary>
        public void Update()
        {
            //Slows the pitch of the audio down whenenver the player activates the slow time feature.

            if(timeCounter > randTime)
            {
                randTime = UnityEngine.Random.Range(Constants.MIN_AMBIANCE_TIME, Constants.MAX_AMBIANCE_TIME);
                timeCounter = 0.0f;
                if (GameManager.Instance.CurRoom == null)
                {
                    RandomlyChooseAmbiance();
                }
            }

            timeCounter += Time.deltaTime;
        }

        /// <summary>
        /// Randomly chooses a SFX
        /// </summary>
        public void RandomlyChooseAmbiance()
        {
            PlayOneShot(Ambiance.ElementAt(UnityEngine.Random.Range(0, Ambiance.Count)).Key);
        }

        public AudioClip GetRandomVoice(float val)
        {
            return val > .5f ? RandomVoices.ElementAt(UnityEngine.Random.Range(0, RandomVoices.Count)).Value : RandomVoicesGrey.ElementAt(UnityEngine.Random.Range(0, RandomVoicesGrey.Count)).Value;
        }

        /// <summary>
        /// Plays a single sound effect
        /// </summary>
        /// <param name="sound">The sound effect we want to play</param>
        public void PlayOneShot(SoundEffects sound, float volumeScale = 1)
        {
            SoundEffectSource.PlayOneShot(SoundEffects[sound], volumeScale);
        }

        /// <summary>
        /// Plays a single sound effect
        /// </summary>
        /// <param name="sound">The sound effect we want to play</param>
        public void PlayOneShot(AmbianceSounds sound, float volumeScale = 1)
        {
            SoundEffectSource.PlayOneShot(Ambiance[sound], volumeScale);
        }

        /// <summary>
        /// Picks a room music file
        /// </summary>
        /// <param name="status"></param>
        public AudioClip GetRoomGreyMusic()
        {
            return GreySounds.ElementAt(UnityEngine.Random.Range(0, GreySounds.Count)).Value;
        }

        /// <summary>
        /// Picks a room music file
        /// </summary>
        /// <param name="status"></param>
        public AudioClip GetRoomCrazyMusic()
        {
            return CrazySounds.ElementAt(UnityEngine.Random.Range(0, GreySounds.Count)).Value;
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
}
