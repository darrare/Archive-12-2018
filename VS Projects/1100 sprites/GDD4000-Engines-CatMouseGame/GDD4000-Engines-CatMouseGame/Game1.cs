using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GDD4000_Engines_CatMouseGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<DrawableGameComponent> sprites;
        bool spaceDown = false;
        TimeSpan gameElapsedTime = new TimeSpan();
        Text countText;
        Random random;
        Texture2D texture;

        List<Task> tasks = new List<Task>();
        int spritesPerThread = 50;
        int spritesPerThreadBase = 300;
        int leftovers = 0;
        bool needToRecalculateList = true;
        int count = 0;

        // http://stackoverflow.com/questions/720429/how-do-i-set-the-window-screen-size-in-xna
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
            sprites = new List<DrawableGameComponent>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            countText = new Text(this, "", Vector2.Zero, Color.Black);
            Components.Add(countText);

            random = new Random();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            
            texture = Content.Load<Texture2D>("Images\\Mouse");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Calculate a random position for the agent
        protected Vector2 RandomPosition()
        {
            return new Vector2((float)(random.NextDouble() * (graphics.GraphicsDevice.Viewport.Width - texture.Width)),
                               (float)(random.NextDouble() * (graphics.GraphicsDevice.Viewport.Height - texture.Height)));
        }

        // Return a random velocity for the agent
        protected Vector2 RandomVelicity()
        {
            Vector2 velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
            velocity.Normalize();
            return velocity;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameElapsedTime += gameTime.ElapsedGameTime;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            AnimatedSprite sprite;

            // Allow the user to add 50 agents at a time by pressing space
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && !spaceDown)
            {
                needToRecalculateList = true;
                for (int i = 0; i < 50; ++i)
                {
                    sprite = new AnimatedSprite(this, texture, Vector2.Zero, RandomVelicity());
                    bool isInCollision;
                    do
                    {
                        sprite.Position = RandomPosition();
                        isInCollision = false;
                        for (int j = 0; j < sprites.Count; ++j)
                        {
                            if (sprite.IsInCollision(((AnimatedSprite)sprites[j])))
                            {
                                isInCollision = true;
                                break;
                            }
                        }
                    } while (isInCollision);
                    sprites.Add(sprite);
                    Components.Add(sprite);
                }
                spaceDown = true;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && spaceDown)
            {
                spaceDown = false;
            }

            // If there are no sprites, print a helpful message
            // If there are sprites, print the number and whether the game is running slowly or not
            if (sprites.Count != 0)
            {
                countText.Message = "" + sprites.Count + ":" + gameTime.IsRunningSlowly;
            }
            else
            {
                countText.Message = "Press Space\nto Add Agents";
            }

            base.Update(gameTime);

            // For each pair of objects, determine if they collide
            //for (int i = 0; i < sprites.Count - 1; ++i)
            //{
            //    for (int j = i + 1; j < sprites.Count; ++j)
            //    {
            //        ((AnimatedSprite)sprites[i]).Collide(((AnimatedSprite)sprites[j]));
            //    }
            //}

            if (sprites.Count == 0)
                return;


            if (needToRecalculateList)
            {
                //spritesPerThread = (int)Math.Ceiling((double)spritesPerThreadBase / (sprites.Count / (double)spritesPerThreadBase));
                //if (spritesPerThread > sprites.Count)
                //{
                //    spritesPerThread = sprites.Count;
                //}
                //leftovers = sprites.Count % spritesPerThread;

                tasks.Clear();
                count = (int)Math.Floor(sprites.Count / (double)spritesPerThread);
                leftovers = sprites.Count - (spritesPerThread * count);
                for (int i = 0; i < count; i++)
                {
                    int copy = i;
                    tasks.Add(Task.Factory.StartNew(() => DetectCollisions(copy * spritesPerThread, spritesPerThread)));
                }
                needToRecalculateList = false;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].IsCompleted)
                {
                    int copy = i;
                    tasks[i] = Task.Factory.StartNew(() => DetectCollisions(copy * spritesPerThread, spritesPerThread));
                }
            }

            if (leftovers != 0)
            {
                DetectCollisions(count * spritesPerThread, leftovers);
            }
        }

        /// <summary>
        /// Detects the collisions on a certain amount of objects
        /// </summary>
        /// <param name="k">The index to start at</param>
        /// <param name="count">The amount of collisions to detect</param>
        private void DetectCollisions(int k, int count)
        {
            for (int i = k; i < k + count - 1; ++i)
            {
                for (int j = i + 1; j < sprites.Count; ++j)
                {
                    ((AnimatedSprite)sprites[i]).Collide(((AnimatedSprite)sprites[j]));
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
