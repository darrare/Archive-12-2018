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
    class SpriteManager
    {
        #region fields

        static SpriteManager instance;
        Dictionary<string, Texture2D> sprites = new Dictionary<string, Texture2D>();
        List<Cat> cats = new List<Cat>();
        Mouse mouse;

        Random rand = new Random();

        #endregion

        #region Instance and constructor

        /// <summary>
        /// Singleton for the SpriteManager class
        /// </summary>
        public static SpriteManager Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                instance = new SpriteManager();
                instance.Initialize();
                return instance;
            }
        }

        /// <summary>
        /// Constructor for the SpriteManager class
        /// </summary>
        private SpriteManager()
        {
            sprites.Add("mouse", GameManager.Instance.Content.Load<Texture2D>("mouseInvis")); //Mouse
            sprites.Add("cat", GameManager.Instance.Content.Load<Texture2D>("catInvis")); //cat
            sprites.Add("cooldown", GameManager.Instance.Content.Load<Texture2D>("cooldown"));
            AreCatsNormColor = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Accessor for the sprites dictionary
        /// </summary>
        public Dictionary<string, Texture2D> Sprites
        {
            get { return sprites; }
        }

        /// <summary>
        /// Accessor for the cats
        /// </summary>
        public List<Cat> Cats
        { get { return cats; } }

        public bool AreCatsNormColor
        { get; set; }
        #endregion

        #region public methods

        /// <summary>
        /// Initialize the sprites
        /// </summary>
        public void Initialize()
        {
            mouse = new Mouse(250f);

            Vector2 spawnLocation;
            for (int i = 0; i < 12; i++)
            {
                do
                {
                    spawnLocation = new Vector2(rand.Next(0, Constants.WINDOW_WIDTH), rand.Next(0, Constants.WINDOW_HEIGHT));
                } while (Vector2.Distance(mouse.Position, spawnLocation) <= 100 || cats.Any(t => Vector2.Distance(t.Position, spawnLocation) <= 50));

                //cats.Add(new Cat(spawnLocation, 200f));
                cats.Add(new Cat(spawnLocation, 1f));
            }

        }

        /// <summary>
        /// Updates the SpriteManager class
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            cats.Where(t => t.IsChasing && Vector2.Distance(t.Position, mouse.Position) > 250).ToList().ForEach(t => t.IsChasing = false);
            cats.Where(t => !t.IsChasing && Vector2.Distance(t.Position, mouse.Position) <= 250).ToList().ForEach(t => t.IsChasing = true);
        }

        public void ResetUnitLocations()
        {
            mouse.RespawnInNewLocation(new Vector2(Constants.WINDOW_WIDTH / 2, Constants.WINDOW_HEIGHT / 2));
            Vector2 spawnLocation;
            for (int i = 0; i < 12; i++)
            {
                do
                {
                    spawnLocation = new Vector2(rand.Next(0, Constants.WINDOW_WIDTH), rand.Next(0, Constants.WINDOW_HEIGHT));
                } while (Vector2.Distance(mouse.Position, spawnLocation) <= 100 || cats.Any(t => Vector2.Distance(t.Position, spawnLocation) <= 50));

                cats[i].RespawnInNewLocation(spawnLocation);
            }
        }

        #endregion

        #region private methods

        #endregion
    }
}
