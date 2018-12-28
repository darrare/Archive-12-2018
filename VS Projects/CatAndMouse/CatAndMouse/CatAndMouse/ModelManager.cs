using System;
using System.Collections;
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
    class ModelManager
    {
        static ModelManager instance;

        VertexPositionTexture[] verts;
        VertexBuffer vertexBuffer;
        Effect effect;
        Texture2D bgImage;

        /// <summary>
        /// Singleton for the Model Manager
        /// </summary>
        public static ModelManager Instance
        {
            get { return instance != null ? instance : instance = new ModelManager(); }
        }

        /// <summary>
        /// The 3D camera info
        /// </summary>
        public CameraInfo Camera
        { get; private set; }

        /// <summary>
        /// Private constructor for the model Manager
        /// </summary>
        private ModelManager()
        {
            SetupCameraProperty();
            CreateGround();
        }

        /// <summary>
        /// Updates the model manager
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {
            //for debuggings
            //Camera.view = Matrix.CreateLookAt(new Vector3(0, -4, 3), GameManager.Instance.Player.TEMP, new Vector3(0, 1, 0)); //position, target, up
            Camera.view = Matrix.CreateLookAt(GameManager.Instance.Player.ActualCubePosition - Vector3.Forward * .1f, new Vector3(GameManager.Instance.Player.FaceDirection.X * 1000, GameManager.Instance.Player.FaceDirection.Y * 1000, 0), new Vector3(0, 0, 1)); //position, target, up

            
        }

        void SetupCameraProperty()
        {
            Camera = new CameraInfo(Matrix.Identity, 
                Matrix.CreateLookAt(new Vector3(0, -4, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0)), //position, target, up
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameManager.Instance.Graphics.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f));
        }

        /// <summary>
        /// Creates the ground or background
        /// </summary>
        void CreateGround()
        {
            // Initialize vertices
            verts = new VertexPositionTexture[4];
            verts[0] = new VertexPositionTexture(
                new Vector3(-2, 2, 0), new Vector2(0, 0));
            verts[1] = new VertexPositionTexture(
                new Vector3(2, 2, 0), new Vector2(1, 0));
            verts[2] = new VertexPositionTexture(
                new Vector3(-2, -2, 0), new Vector2(0, 1));
            verts[3] = new VertexPositionTexture(
                new Vector3(2, -2, 0), new Vector2(1, 1));

            // Set vertex data in VertexBuffer
            vertexBuffer = new VertexBuffer(GameManager.Instance.Graphics.GraphicsDevice, typeof(VertexPositionTexture), verts.Length, BufferUsage.None);
            vertexBuffer.SetData(verts);

            bgImage = GameManager.Instance.Content.Load<Texture2D>(@"bgImage");
            // Load effect
            effect = GameManager.Instance.Content.Load<Effect>(@"Shader");
        }

        /// <summary>
        /// Draws the models
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Draw(GameTime gameTime)
        {
            //Draw Ground
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(Camera.world);
            effect.Parameters["View"].SetValue(Camera.view);
            effect.Parameters["Projection"].SetValue(Camera.projection);
            effect.Parameters["TextureImage"].SetValue(bgImage);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameManager.Instance.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>
                    (PrimitiveType.TriangleStrip, verts, 0, 2);
            }

            //Draw mouse
            GameManager.Instance.Player.Draw(gameTime);

            //draw cats
            foreach (Cat c in SpriteManager.Instance.Cats)
            {
                c.Draw(gameTime);
            }
        }
    }

    class CameraInfo
    {
        public Matrix world;
        public Matrix view;
        public Matrix projection;

        public CameraInfo(Matrix world, Matrix view, Matrix projection)
        {
            this.world = world;
            this.view = view;
            this.projection = projection;
        }
    }
}
