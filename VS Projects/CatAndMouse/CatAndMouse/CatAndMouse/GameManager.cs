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
    class GameManager
    {
        #region fields

        static GameManager instance;

        float timeRemaining = 30000;
        string timeText = "Time Remaining: ";
        Text timeRemainingText;
        Text winnerMessage;

        #endregion

        #region Instance and constructor

        /// <summary>
        /// Singleton for the GameManager class
        /// </summary>
        public static GameManager Instance
        {
            get { return instance != null ? instance : instance = new GameManager(); }
        }

        /// <summary>
        /// Constructor for the GameManager class
        /// </summary>
        private GameManager()
        {
            //Initialize the game to the righty keyboard config
            KeyboardConfiguration.SetConfiguration(Keys.W, Keys.A, Keys.S, Keys.D, Keys.Space, new List<Keys>() { Keys.LeftShift, Keys.RightShift }, new List<Keys>() { Keys.N, Keys.M, Keys.N, Keys.M, Keys.J, Keys.K, Keys.J, Keys.K, Keys.J, Keys.J, Keys.L, Keys.L, Keys.N });
        }

        #endregion

        #region properties

        /// <summary>
        /// Whether or not the game is currently paused
        /// </summary>
        public bool IsPaused
        { get; set; }

        /// <summary>
        /// The game - Set from game1's constructor
        /// </summary>
        public Game Game
        { get; set; }

        /// <summary>
        /// The spritebatch - Set from Game1's constructor
        /// </summary>
        public SpriteBatch SpriteBatch
        { get; set; }

        /// <summary>
        /// The default font we are going to use for everything
        /// </summary>
        public SpriteFont Font
        { get; set; }

        /// <summary>
        /// The graphics device manager - Set from Game1's constructor
        /// </summary>
        public GraphicsDeviceManager Graphics
        { get; set; }

        /// <summary>
        /// The content manager - Set from Game1's constructor
        /// </summary>
        public ContentManager Content
        { get; set; }

        /// <summary>
        /// Accessor for the player controlled mouse
        /// </summary>
        public Mouse Player
        { get; set; }

        /// <summary>
        /// Accessor for the time remaining in the game
        /// </summary>
        public float TimeRemaining
        { get { return timeRemaining; } set { timeRemaining = value; } }

        #endregion

        #region public methods

        /// <summary>
        /// Initialize the game manager
        /// </summary>
        public void Initialize()
        {
            timeRemainingText = new Text("", new Vector2(50, 50));
            
        }

        /// <summary>
        /// Updates the GameManager class
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            AudioManager.Instance.Update(gameTime);
            SpriteManager.Instance.Update(gameTime);
            InputManager.Instance.Update(gameTime);
            SaveLoadManager.Instance.Update();
            ModelManager.Instance.Update(gameTime);

            if (!IsPaused && timeRemaining > 0)
            {
                timeRemaining -= gameTime.ElapsedGameTime.Milliseconds;
                timeRemainingText.TextContent = timeText + (timeRemaining / 1000).ToString();
            }
            else if (!IsPaused && timeRemaining <= 0)
            {
                timeRemainingText.TextContent = timeText + "0";
                IsPaused = true;
                winnerMessage = new Text("Game over! The mouse won!", new Vector2(Constants.WINDOW_WIDTH / 2, Constants.WINDOW_HEIGHT / 2));
            }
            else if (IsPaused && InputManager.Instance.IsKey(Keys.Space))
            {
                //Reset the game
                timeRemaining = 30000;
                SpriteManager.Instance.ResetUnitLocations();
                Game.Components.Remove(winnerMessage);
                IsPaused = false;
            }
        }

        /// <summary>
        /// The agents have caught the mouse
        /// </summary>
        public void AgentHasWon()
        {
            IsPaused = true;
            winnerMessage = new Text("Game over! The cats won...", new Vector2(Constants.WINDOW_WIDTH / 2, Constants.WINDOW_HEIGHT / 2));
        }

        #endregion

        #region private methods



        #endregion

    }
}
