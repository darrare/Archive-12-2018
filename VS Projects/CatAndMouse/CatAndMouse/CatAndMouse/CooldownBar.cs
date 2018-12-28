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
    class CooldownBar : Sprite
    {
        protected float offset;
        protected float cooldownTime;
        protected float timer = 0;
        protected Sprite obj;

        /// <summary>
        /// Constructor for the bar
        /// </summary>
        /// <param name="game"></param>
        /// <param name="offset">offset from the object it is attached too</param>
        /// <param name="cooldownTime">time it takes to reset, in seconds</param>
        public CooldownBar(float offset, float cooldownTime, Sprite obj)
            :base ("cooldown")
        {
            this.offset = offset;
            this.cooldownTime = cooldownTime * 1000;
            this.obj = obj;
            color = Color.Cyan;
            SaveLoadManager.Instance.ObjectsToSave.Add(this);
        }

        /// <summary>
        /// Accessor for the offset
        /// </summary>
        public float Offset
        { get { return offset; } }

        public float Timer
        { get { return timer; } }

        /// <summary>
        /// Accessor for whether or not the ability associated with this bar is available
        /// </summary>
        public bool IsReady
        { get { return timer >= cooldownTime; } }

        /// <summary>
        /// resets the timer to 0 so it can start charging up
        /// </summary>
        public void ResetTimer()
        {
            timer = 0;
            ((Mouse)obj).CanJump = false;
        }

        /// <summary>
        /// Updates the bar
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (!GameManager.Instance.IsPaused)
            {
                base.Update(gameTime);
                Vector3 screenSpace = GameManager.Instance.Graphics.GraphicsDevice.Viewport.Project(GameManager.Instance.Player.ActualCubePosition, ModelManager.Instance.Camera.projection, ModelManager.Instance.Camera.view, ModelManager.Instance.Camera.world);
                //position = new Vector2(obj.Position.X, obj.Position.Y + offset);
                position = new Vector2(screenSpace.X, screenSpace.Y + offset);

                if (!((Mouse)obj).CanJump)
                {
                    if (timer < cooldownTime)
                    {
                        timer += gameTime.ElapsedGameTime.Milliseconds;
                        scale = new Vector2(timer / cooldownTime, 1f);
                    }
                    else
                    {
                        scale = new Vector2(timer / cooldownTime, 1f);
                        ((Mouse)obj).CanJump = true;
                    }
                }
            }
        }

        public void LoadSerializableCooldownBar(SaveableCooldownBar saveData)
        {
            timer = saveData.timer;
        }
    }
}
