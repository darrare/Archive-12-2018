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
    //Delegate for firing the sequence event
    public delegate void ButtonSequenceEvent();
    public delegate List<Keys> ButtonSequenceSequence();

    /// <summary>
    /// Handles all of the user input
    /// </summary>
    class InputManager
    {
        static InputManager instance;
        KeyboardState keyboard, previousKeyboard;
        List<ButtonSequence> buttonSequences = new List<ButtonSequence>();

        /// <summary>
        /// Singleton for the InputManager
        /// </summary>
        public static InputManager Instance
        {
            get { return instance != null ? instance : instance = new InputManager(); }
        }

        /// <summary>
        /// Returns the keyboardState
        /// </summary>
        public KeyboardState KeyState
        {
            get { return keyboard; }
        }

        /// <summary>
        /// Access to the list of button sequences
        /// </summary>
        public List<ButtonSequence> ButtonSequences
        { get { return buttonSequences; } }

        /// <summary>
        /// private constructor
        /// </summary>
        private InputManager()
        {

        }

        /// <summary>
        /// Updates the input manager
        /// </summary>
        /// <param name="gameTime">gameTime</param>
        public void Update(GameTime gameTime)
        {
            previousKeyboard = keyboard;
            keyboard = Keyboard.GetState();
            UpdateSequences(gameTime);

            if (keyboard.IsKeyDown(Keys.D1))
            {
                KeyboardConfiguration.SetConfiguration(Keys.W, Keys.A, Keys.S, Keys.D, Keys.Space, new List<Keys>() { Keys.LeftShift, Keys.RightShift }, new List<Keys>() { Keys.N, Keys.M, Keys.N, Keys.M, Keys.J, Keys.K, Keys.J, Keys.K, Keys.J, Keys.J, Keys.L, Keys.L, Keys.N });
            }
            else if (keyboard.IsKeyDown(Keys.D2))
            {
                KeyboardConfiguration.SetConfiguration(Keys.I, Keys.J, Keys.K, Keys.L, Keys.Space, new List<Keys>() { Keys.LeftShift, Keys.RightShift }, new List<Keys>() { Keys.V, Keys.C, Keys.V, Keys.C, Keys.F, Keys.D, Keys.F, Keys.D, Keys.F, Keys.F, Keys.S, Keys.S, Keys.V });
            }
        }

        /// <summary>
        /// Updates the sequences to see if any have made progress or need to be reset
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdateSequences(GameTime gameTime)
        {
            foreach (ButtonSequence sequence in buttonSequences)
            {
                sequence.CheckSequence(gameTime);
            }
        }

        /// <summary>
        /// Check to see if multiple keys are pressed simultaniously
        /// </summary>
        /// <param name="keys">list of keys we want to check</param>
        public bool AreKeysDown(List<Keys> keys)
        {
            foreach (Keys key in keys)
            {
                if (!keyboard.GetPressedKeys().Contains(key))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Is the key down, and was previously up on the prior frame
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool IsKeyDown(Keys key)
        {
            return !previousKeyboard.GetPressedKeys().Contains(key) && keyboard.GetPressedKeys().Contains(key);
        }

        /// <summary>
        /// Is the key up, regardless of previous frame
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool IsKeyUp(Keys key)
        {
            return keyboard.IsKeyUp(key);
        }

        /// <summary>
        /// Is the key currently down, regardless of the previous frame
        /// </summary>
        /// <param name="key">Key to check</param>
        public bool IsKey(Keys key)
        {
            return keyboard.IsKeyDown(key);
        }
    }

    /// <summary>
    /// A button sequence
    /// </summary>
    class ButtonSequence
    {
        int index = 0;
        float timeLimit;
        float timeLimitPerKey;
        double startTime;
        double previousPressTime;
        ButtonSequenceEvent method;
        ButtonSequenceSequence sequenceMethod;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sequence">list of keys that need to be pressed in order</param>
        /// <param name="method">method to call if sequence is achieved</param>
        /// <param name="timeLimit">time limit from start to finish</param>
        /// <param name="timeLimitPerKey">time limit in between each key press</param>
        public ButtonSequence(ButtonSequenceEvent method, ButtonSequenceSequence sequenceMethod, float timeLimit = 10000f, float timeLimitPerKey = 1000f)
        {
            this.timeLimit = timeLimit;
            this.method = method;
            this.sequenceMethod = sequenceMethod;
            this.timeLimitPerKey = timeLimitPerKey;
        }

        /// <summary>
        /// Checks the progress on the sequence
        /// </summary>
        public void CheckSequence(GameTime gameTime)
        {
            if (InputManager.Instance.IsKeyDown(sequenceMethod()[index]) && InputManager.Instance.KeyState.GetPressedKeys().Length == 1)
            {
                //start the timer for the sequence
                if (index == 0)
                {
                    startTime = gameTime.TotalGameTime.TotalMilliseconds;
                }

                previousPressTime = gameTime.TotalGameTime.TotalMilliseconds;
                index++;
                if (index == sequenceMethod().Count)
                {
                    SequenceActivated();
                }
            }
            //if there is any other key down or we have passed the required time limit.
            else if (index >= 1 ? InputManager.Instance.KeyState.GetPressedKeys().Any(t => !(t == sequenceMethod()[index - 1]))
                || gameTime.TotalGameTime.TotalMilliseconds - timeLimit > startTime
                || gameTime.TotalGameTime.TotalMilliseconds - timeLimitPerKey > previousPressTime :
                gameTime.TotalGameTime.TotalMilliseconds - timeLimit > startTime
                || gameTime.TotalGameTime.TotalMilliseconds - timeLimitPerKey > previousPressTime
                )
            {
                Console.WriteLine(InputManager.Instance.KeyState.GetPressedKeys().Length);
                ResetSequence(gameTime);
            }
        }

        /// <summary>
        /// user has failed, reset the sequence
        /// </summary>
        public void ResetSequence(GameTime gameTime)
        {
            index = 0;
            previousPressTime = gameTime.TotalGameTime.TotalMilliseconds;
            startTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// user has succeeded, activate the event
        /// </summary>
        public void SequenceActivated()
        {
            index = 0;
            previousPressTime = 0;
            startTime = 0;
            method();
        }
    }

    /// <summary>
    /// The configuration of the keyboard to support righties and lefties
    /// </summary>
    [Serializable]
    static class KeyboardConfiguration
    {
        public static Keys MoveUp
        { get; private set; }
        public static Keys MoveDown
        { get; private set; }
        public static Keys MoveLeft
        { get; private set; }
        public static Keys MoveRight
        { get; private set; }
        public static Keys MainKey
        { get; private set; }
        public static List<Keys> Boost
        { get; private set; }
        public static List<Keys> AttackSequence
        { get; private set; }

        /// <summary>
        /// sets the keys to the configuration
        /// </summary>
        /// <param name="mainKey">Spacebar</param>
        /// <param name="boost">Any key that can be used to boost</param>
        /// <param name="attackSequence">Sequence for the attack</param>
        public static void SetConfiguration(Keys moveUp, Keys moveLeft, Keys moveDown, Keys moveRight, Keys mainKey, List<Keys> boost, List<Keys> attackSequence)
        {
            MoveUp = moveUp;
            MoveLeft = moveLeft;
            MoveRight = moveRight;
            MoveDown = moveDown;
            MainKey = mainKey;
            Boost = boost;
            AttackSequence = attackSequence;
        }

        /// <summary>
        /// Used by a delegate to return the proper attack sequence if this is the proper sequence we want.
        /// </summary>
        /// <returns></returns>
        public static List<Keys> GetAttackSequence()
        {
            return AttackSequence;
        }
    }
}
