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
    class RechargeBar : Sprite
    {
        protected float offset;
        protected float spendTime;
        protected float rechargeTime;
        protected float timer = 0;
        protected Sprite obj;
        bool dontRechargeThisFrame = false;
        Color full, notFull;

        /// <summary>
        /// Constructor for the bar
        /// </summary>
        /// <param name="game"></param>
        /// <param name="offset">offset from the object it is attached too</param>
        /// <param name="cooldownTime">time it takes to reset, in seconds</param>
        public RechargeBar(float offset, float spendTime, float rechargeTime, Sprite obj, Color full, Color notFull)
            :base ("cooldown")
        {
            this.offset = offset;
            this.spendTime = spendTime * 1000;
            this.rechargeTime = rechargeTime * 1000;
            this.obj = obj;
            this.full = full;
            this.notFull = notFull;
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
        /// Enable the effect for a period of time.
        /// </summary>
        /// <param name="time">time to enable</param>
        /// <returns>Whether or not you are able to activate this effect</returns>
        public bool ActivateEffect(float time)
        {
            if (timer - time < 0)
            {
                return false;
            }
            else
            {
                timer -= (time / (spendTime / 1000)) * (rechargeTime / 1000);
                dontRechargeThisFrame = true;
                return true;
            }
        }

        /// <summary>
        /// Resets the timer to 0
        /// </summary>
        public void ResetTimer()
        {
            timer = 0;
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

                if (timer < rechargeTime && !dontRechargeThisFrame)
                {
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                    color = notFull;
                }
                else if (timer >= rechargeTime)
                {
                    color = full;
                }
                else
                {
                    color = Color.Red;
                }
                scale = new Vector2(timer / rechargeTime, 1f);
                dontRechargeThisFrame = false;
            }
        }

        public void LoadSerializableRechargeBar(SaveableRechargeBar saveData)
        {
            timer = saveData.timer;
        }
    }
}
