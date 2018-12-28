using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GDD4000_Engines_CatMouseGame
{
    // http://www.informit.com/articles/article.aspx?p=1671634&seqNum=4
    public class Sprite : DrawableGameComponent
    {
        // Texture of this sprite
        protected Texture2D Texture { get; set; }

        // Color applied to this sprite
        protected Color Color { get; set; }

        // Position of the sprite
        public Vector2 Position { get; set; }

        protected Vector2 Origin { get { return new Vector2(Position.X + Texture.Width * 0.5f, Position.Y + Texture.Height * 0.5f);  } }

        // Rectangular bounding volume of the sprite
        public Rectangle Rect { get { return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); } }

        // Sprite constructor
        public Sprite(Game game, Texture2D texture, Vector2 initPosition) : base(game)
        {
            this.Texture = texture;
            Position = initPosition;
            Color = Color.White;
            this.DrawOrder = (int)DisplayLayer.Background;
        }

        // Sprite's load content - for this version of the game, all sprites are the same
        // so we save texture memory by loading it once in Game1 and then referencing it
        protected override void LoadContent()
        {
            base.LoadContent();
        }
        
        // Draw for the Sprite
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService(
                            typeof(SpriteBatch)) as SpriteBatch;
            spriteBatch.Begin(SpriteSortMode.FrontToBack, null);
            spriteBatch.Draw(Texture, Position, null, Color, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.9f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
