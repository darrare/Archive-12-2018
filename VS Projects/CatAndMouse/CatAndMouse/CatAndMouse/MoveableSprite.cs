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
    class MoveableSprite : Sprite
    {
        #region fields

        protected float speed;

        #endregion

        #region constructor

        public MoveableSprite(string spriteName, float speed)
            : base(spriteName)
        {
            this.speed = speed;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Updates the sprite
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (velocity != Vector2.Zero)
            {
                velocity = Vector2.Normalize(velocity);
                position += velocity * (speed * (gameTime.ElapsedGameTime.Milliseconds / 1000f));
            }

            //Clamp the sprite within the screen bounds
            if (position.Y < 0)
            {
                position.Y = 0;
            }
            else if (position.Y > Constants.WINDOW_HEIGHT)
            {
                position.Y = Constants.WINDOW_HEIGHT;
            }
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > Constants.WINDOW_WIDTH)
            {
                position.X = Constants.WINDOW_WIDTH;
            }
        }

        #endregion
    }
}
