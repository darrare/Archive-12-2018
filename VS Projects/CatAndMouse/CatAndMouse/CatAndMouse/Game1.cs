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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public Game1()
        {
            GameManager.Instance.Game = this;
            GameManager.Instance.Graphics = new GraphicsDeviceManager(this);
            GameManager.Instance.Graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            GameManager.Instance.Graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
            Content.RootDirectory = "Content";
            GameManager.Instance.Content = Content;  
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            GameManager.Instance.SpriteBatch = new SpriteBatch(GraphicsDevice);
            GameManager.Instance.Font = Content.Load<SpriteFont>("basicFont");
            GameManager.Instance.Initialize();
            base.Initialize();
        }     

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            GameManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            ModelManager.Instance.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
