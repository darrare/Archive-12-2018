using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CatAndMouse
{
    class AudioManager
    {
        #region fields

        static AudioManager instance;
        Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        SoundEffectInstance backgroundSound;

        #endregion

        #region Instance and constructor

        /// <summary>
        /// Singleton for the AudioManager class
        /// </summary>
        public static AudioManager Instance
        {
            get { return instance != null ? instance : instance = new AudioManager(); }
        }

        /// <summary>
        /// Constructor for the AudioManager class
        /// </summary>
        private AudioManager()
        {
            sounds.Add("happyCat", GameManager.Instance.Content.Load<SoundEffect>("happyCat"));
            sounds.Add("sadCat", GameManager.Instance.Content.Load<SoundEffect>("sadCat"));
            sounds.Add("happyMouse", GameManager.Instance.Content.Load<SoundEffect>("happyMouse"));
            sounds.Add("sadMouse", GameManager.Instance.Content.Load<SoundEffect>("sadMouse"));
            sounds.Add("background", GameManager.Instance.Content.Load<SoundEffect>("background"));
            backgroundSound = sounds["background"].CreateInstance();
            backgroundSound.IsLooped = true;
            backgroundSound.Play();
        }

        #endregion

        #region properties

        #endregion

        #region public methods

        /// <summary>
        /// Updates the AudioManager class
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// plays a sound
        /// </summary>
        /// <param name="soundName">Key for the dictionary for the proper sound</param>
        public void PlaySound(string soundName)
        {
            sounds[soundName].Play();
        }

        #endregion

        #region private methods

        #endregion
    }
}
