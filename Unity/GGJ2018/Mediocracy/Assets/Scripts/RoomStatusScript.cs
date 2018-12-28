using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AUTools.Unity;

namespace Mediocracy
{
    public class RoomStatusScript : PauseObjScript
    {
        bool muted = true;
        [SerializeField, Range(0, 1)] float insanity;

        [SerializeField] Transform leftBlinds;

        [SerializeField] Transform rightBlinds;

        public PsychedelicShaderDriver Shader { get; private set; }

        /// <summary>
        /// Neighbors that will affect this room.
        /// </summary>
        public List<RoomStatusScript> Neighbors
        { get; private set; } = new List<RoomStatusScript>();

        /// <summary>
        /// Insanity level. 0 is grey, 1 is crazy
        /// </summary>
        public float Insanity
        {
            get { return insanity; }
            set
            {
                float newValue = Mathf.Clamp01(value);
                if (newValue < insanity)
                {
                    GameManager.Instance.TotalInsanity -= (insanity - newValue) * Constants.SUCCESS_MULT;
                }
                insanity = newValue;
                GetComponent<RoomPopulator>().Person.GetComponentInChildren<CharacterAnimationScript>().SpeedPct = insanity;
                GetComponentInChildren<WallpaperShaderDriver>().insanity = insanity;
                foreach (GameObject roomSpawnedObject in GetComponent<RoomPopulator>().RoomSpawnedObjects)
                {
                    PsychedelicShaderDriver psychedelicShaderDriver = roomSpawnedObject.GetComponentInChildren<PsychedelicShaderDriver>();
                    if (psychedelicShaderDriver)
                    {
                        psychedelicShaderDriver.insanity = insanity;
                    }
                }

                if (value <= Constants.LOW_ROOM_INSANITY)
                {
                    foreach (RoomStatusScript n in Neighbors.Where(t => !t.IsUnlocked))
                    {
                        n.UnlockLevel();
                    }
                    GameManager.Instance.RoomClearedEvent.Invoke(gameObject);
                }
            }
        }

        public bool IsUnlocked
        { get; set; } = false;

        public AudioSource GreySource
        { get; set; }

        public AudioSource CrazySource
        { get; set; }

        public AudioSource SFXSource
        { get; set; }

        public int Level
        { get; set; } = 0;


        // Use this for initialization
        void Start()
        {
            GreySource = gameObject.AddComponent<AudioSource>();
            GreySource.loop = true;
            CrazySource = gameObject.AddComponent<AudioSource>();
            CrazySource.loop = true;
            SFXSource = gameObject.AddComponent<AudioSource>();

            GreySource.clip = AudioManager.Instance.GetRoomGreyMusic();
            GreySource.Play();
            CrazySource.clip = AudioManager.Instance.GetRoomCrazyMusic();
            CrazySource.Play();

            MuteAudioSources();

            if (Level == 0)
            {
                UnlockLevel();
            }
            Insanity = 1;
            Shader = GetComponent<PsychedelicShaderDriver>();
        }

        public void UnlockLevel()
        {
            if (!IsUnlocked)
            {
                IsUnlocked = true;
                StartCoroutine(OpenBlinds());
            }
        }

        IEnumerator OpenBlinds()
        {
            while (true)
            {
                leftBlinds.localScale = new Vector3(leftBlinds.localScale.x * .99f, leftBlinds.localScale.y, 1);
                rightBlinds.localScale = new Vector3(rightBlinds.localScale.x * .99f, rightBlinds.localScale.y, 1);
                if (leftBlinds.localScale.x <= .25f)
                {
                    break;
                }
                yield return null;
            }
        }

        protected override void NotPausedUpdate()
        {
            if (!IsUnlocked)
                return;
            UpdateInsanity();
        }

        void UpdateInsanity()
        {
            Insanity += ((Constants.INSANITY_BASE_DROP_PER_SECOND_MULTIPLIER * Neighbors.Average(n => n.Insanity)) + Constants.INSANITY_BASE_DROP_PER_SECOND_ADDITION) *
                Time.deltaTime;
            if (!muted)
            {
                if (insanity > .5f)
                {
                    CrazySource.volume = .75f;
                    GreySource.volume = 0;
                }
                else
                {
                    CrazySource.volume = 0;
                    GreySource.volume = .75f;
                }
            }
        }

        public void MuteAudioSources()
        {
            muted = true;
            GreySource.volume = 0;
            CrazySource.volume = 0;
            SFXSource.volume = 0;
        }

        public void EnableAudioSources()
        {
            muted = false;
            GreySource.volume = .75f;
            CrazySource.volume = .75f;
            SFXSource.volume = 1;
        }

        public void GetRandomVoice(float val)
        {
            SFXSource.PlayOneShot(AudioManager.Instance.GetRandomVoice(val));
        }
    }

}
