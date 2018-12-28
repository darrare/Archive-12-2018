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
    class AutoOrientingSprite : MoveableSprite
    {
        #region fields

        #endregion

        #region constructor

        public AutoOrientingSprite(string spriteName, float speed)
            : base(spriteName, speed)
        {

        }

        #endregion

        #region properties

        public Vector3 CubePosition
        { get; protected set; }

        public Vector3 ActualCubePosition
        { get; protected set; }

        #endregion

        #region public methods

        /// <summary>
        /// Updates the sprite
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Will orient the sprite towards the direction it is moving.
            if (velocity != Vector2.Zero)
                rotation = (float)Math.Atan2(velocity.X, -velocity.Y);
        }


        #endregion
    }
}
