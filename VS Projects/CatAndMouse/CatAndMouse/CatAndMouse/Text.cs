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
    class Text : DrawableGameComponent
    {
        protected string text;
        protected Vector2 position;

        public Text(string text, Vector2 position)
            :base (GameManager.Instance.Game)
        {
            GameManager.Instance.Game.Components.Add(this);
            this.text = text;
            this.position = position;
            SaveLoadManager.Instance.ObjectsToSave.Add(this);
        }

        public string TextContent
        {
            get { return text; }
            set { text = value; }
        }

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
            GameManager.Instance.SpriteBatch.DrawString(GameManager.Instance.Font, text, position, Color.White);
            GameManager.Instance.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public void LoadSerializableText(SaveableText saveData)
        {
            TextContent = saveData.text;
        }

    }
}
