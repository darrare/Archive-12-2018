using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GDD4000_Engines_CatMouseGame
{
    // http://gamedev.stackexchange.com/questions/13395/a-cool-way-of-doing-z-index-for-xna-components
    public enum DisplayLayer
    {
        Background, //back-layer
        Particles,
        Player,
        MenuBack,
        MenuFront //front-layer
    }

    class Text : DrawableGameComponent
    {
        Vector2 position;
        string message;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Color color;

        // The message to render as text
        public String Message { get { return message; } set { message = value; } }

        public Text(Game game, string message, Vector2 initPosition, Color color)
            : base(game)
        {
            this.message = message;
            position = initPosition;
            this.color = color;
            this.DrawOrder = (int)DisplayLayer.MenuFront;
        }
        protected override void LoadContent()
        {
            spriteBatch = Game.Services.GetService(
                            typeof(SpriteBatch)) as SpriteBatch;
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Game.Content.Load<SpriteFont>("Fonts\\CourierNew");

            position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2,
                Game.GraphicsDevice.Viewport.Height / 2);
            base.LoadContent();
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch = Game.Services.GetService(
                            typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);

            // Find the center of the string
            Vector2 FontOrigin = spriteFont.MeasureString(message) / 2;
            // Draw the string
            spriteBatch.DrawString(spriteFont, message, position, color,
                0, FontOrigin, 1.0f, SpriteEffects.None, 0.1f);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
