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

namespace SolarSystem
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix projection, view;
        Model sphere;
        Effect perlinNoiseEffect;

        Texture2D permTexture2d;
        Texture2D permGradTexture;

        Vector3 cameraPosition;

        PerlinNoise noiseEngine = new PerlinNoise();

        float timer = .15f;
        float sunEffectMin = .15f;
        float sunEffectMax = .25f;
        bool sunEffectGoingUp = true;

        float cloudTimer = .05f;
        float cloudEffectMin = .05f;
        float cloudEffectMax = .1f;
        bool cloudEffectGoingUp = true;

        Planet planetToView;
        float distance;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1080;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            projection =
                Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 10000.0f);
            noiseEngine.InitNoiseFunctions(3435, graphics.GraphicsDevice);
            IsMouseVisible = true;
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
            sphere = Content.Load<Model>(@"sphere");
            perlinNoiseEffect = Content.Load<Effect>(@"PerlinNoiseEffect");

            permTexture2d = noiseEngine.GeneratePermTexture2d();
            permGradTexture = noiseEngine.GeneratePermGradTexture();

            foreach (ModelMesh mesh in sphere.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = perlinNoiseEffect;
                    part.Effect.Parameters["permTexture2d"].SetValue(permTexture2d);
                    part.Effect.Parameters["permGradTexture"].SetValue(permGradTexture);
                }
            }

            // TODO: use this.Content to load your game content here

            PlanetManager.Instance.ContentManager = Content;
            PlanetManager.Instance.PerlinNoiseEffect = perlinNoiseEffect;
            PlanetManager.Instance.Projection = projection;
            PlanetManager.Instance.View = view;
            PlanetManager.Instance.GeneratePlanets();
            planetToView = PlanetManager.Instance.Sun;
            distance = 5000;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            PlanetManager.Instance.Update(gameTime);
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.OemTilde))
            {
                planetToView = PlanetManager.Instance.Sun;
                distance = 5000;
            }
            else if (keys.IsKeyDown(Keys.D1))
            {
                planetToView = PlanetManager.Instance.Mercury;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D2))
            {
                planetToView = PlanetManager.Instance.Venus;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D3))
            {
                planetToView = PlanetManager.Instance.Earth;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D4))
            {
                planetToView = PlanetManager.Instance.Mars;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D5))
            {
                planetToView = PlanetManager.Instance.Jupiter;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D6))
            {
                planetToView = PlanetManager.Instance.Saturn;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D7))
            {
                planetToView = PlanetManager.Instance.Uranus;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D8))
            {
                planetToView = PlanetManager.Instance.Neptune;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D9))
            {
                planetToView = PlanetManager.Instance.Moon;
                distance = 500;
            }
            else if (keys.IsKeyDown(Keys.D0))
            {
                planetToView = PlanetManager.Instance.Sun;
                distance = 500;
            }

            // Rotating camera
            cameraPosition = new Vector3(planetToView.World.Translation.X, 0f, planetToView.World.Translation.Z + distance);
            //view = Matrix.CreateLookAt(new Vector3(distance * (float)Math.Sin(timer), distance, distance * (float)Math.Cos(timer)),
            //view = Matrix.CreateLookAt(cameraPosition, new Vector3(0f, 0f, 0f), Vector3.Up);
            view = Matrix.CreateLookAt(cameraPosition, planetToView.World.Translation, Vector3.Up);
            PlanetManager.Instance.View = view;
            PlanetManager.Instance.Projection = projection;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix translate = Matrix.CreateTranslation(new Vector3(0, 0, 0));
            Matrix rotate = Matrix.CreateRotationY(0);
            
            // TODO: Add your drawing code here
            //DrawModel(sphere, rotate * translate, projection, view, "Neptune");
            PlanetManager.Instance.DrawPlanets(gameTime);
            base.Draw(gameTime);
        }

        private void DrawModel(Model m, Matrix world, Matrix projection, Matrix view, string technique)
        {
            //m = PlanetManager.Instance.Earth.Sphere;
            //technique = "Earth";

            Matrix[] transforms = new Matrix[m.Bones.Count];
            m.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = perlinNoiseEffect.Techniques[technique];
                    effect.Parameters["World"].SetValue(world);
                    effect.Parameters["View"].SetValue(view);
                    effect.Parameters["Projection"].SetValue(projection);
                    //effect.Parameters["CloudsEffect"].SetValue(cloudTimer);
                    //effect.Parameters["SunEffect"].SetValue(timer);
                }
                mesh.Draw();
            }
        }
    }
}
