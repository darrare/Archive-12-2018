using System;
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
    class Sprite : DrawableGameComponent
    {
        #region fields

        protected Texture2D sprite;
        protected Rectangle drawRectangle;
        protected float rotation = 0;
        protected Vector2 scale;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Color color = Color.White;

        Random rand = new Random();

        #endregion

        #region constructor

        public Sprite(string spriteName)
            : base(GameManager.Instance.Game)
        {
            GameManager.Instance.Game.Components.Add(this);
            drawRectangle = new Rectangle();
            scale = Vector2.One;
            sprite = SpriteManager.Instance.Sprites[spriteName];
            drawRectangle.Width = sprite.Width;
            drawRectangle.Height = sprite.Height;
        }

        #endregion

        #region properties

        /// <summary>
        /// The position of the sprite
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            protected set { position = value; }
        }
        
        public Vector2 Velocity
        { get { return velocity; } }

        /// <summary>
        /// The sprites texture
        /// </summary>
        public Texture2D Texture
        { get { return sprite; } }

        /// <summary>
        /// The sprites scale
        /// </summary>
        public Vector2 Scale
        { get { return scale; } }

        #endregion

        /// <summary>
        /// Updates the sprite
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprite
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            GameManager.Instance.SpriteBatch.Begin();
            GameManager.Instance.SpriteBatch.Draw(sprite, position, null, color, rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, 0);
            GameManager.Instance.SpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Respawns the sprite in a new location when the game is reset
        /// </summary>
        /// <param name="location"></param>
        public virtual void RespawnInNewLocation(Vector2 location)
        {
            Position = location;
        }

    }
}
