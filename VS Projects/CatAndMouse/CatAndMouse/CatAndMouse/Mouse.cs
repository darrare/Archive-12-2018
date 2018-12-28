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
    class Mouse : AutoOrientingSprite
    {

        #region fields

        CooldownBar hyperJumpCooldown;
        RechargeBar boost;
        RechargeBar attackMode;
        bool canJump = false;
        Random rand = new Random();
        float baseSpeed;
        bool isInAttackMode = false;
        int framesToSwapColorWhenAttacking = 10;
        int frameCounter = 0;

        Effect effect;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        VertexPositionColor[] vertices;
        short[] indices;
        float width = .05f;
        float height = .05f;
        float depth = .05f;

        #endregion

        #region constructor

        /// <summary>
        /// constructor for the mouse
        /// </summary>
        /// <param name="speed">the speed at which the mouse moves</param>
        public Mouse(float speed)
            :base("mouse", speed)
        {
            GameManager.Instance.Player = this;
            baseSpeed = speed;
            FaceDirection = new Vector2(0, 1);
            hyperJumpCooldown = new CooldownBar(-18f, 5, this);
            attackMode = new RechargeBar(-14f, 5, 10, this, Color.Yellow, Color.DarkBlue);
            boost = new RechargeBar(-10f, 1, 2, this, Color.Green, Color.Red);
            Position = new Vector2(Constants.WINDOW_WIDTH / 2, Constants.WINDOW_HEIGHT / 2);
            //position = Vector2.Zero;

            effect = GameManager.Instance.Content.Load<Effect>(@"MouseEffect");


            vertices = new VertexPositionColor[8];
            indices = new short[36];
            vertexBuffer = new VertexBuffer(GameManager.Instance.Graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(GameManager.Instance.Graphics.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);

            vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.White);
            vertices[1] = new VertexPositionColor(new Vector3(width, 0, 0), Color.Red);
            vertices[2] = new VertexPositionColor(new Vector3(width, -height, 0), Color.Green);
            vertices[3] = new VertexPositionColor(new Vector3(0, -height, 0), Color.Black);
            vertices[4] = new VertexPositionColor(new Vector3(0, 0, depth), Color.Blue);
            vertices[5] = new VertexPositionColor(new Vector3(width, 0, depth), Color.Purple);
            vertices[6] = new VertexPositionColor(new Vector3(width, -height, depth), Color.Orange);
            vertices[7] = new VertexPositionColor(new Vector3(0, -height, depth), Color.Yellow);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            indices[0] = 0; indices[1] = 1; indices[2] = 2;
            indices[3] = 0; indices[4] = 3; indices[5] = 2;
            indices[6] = 4; indices[7] = 0; indices[8] = 3;
            indices[9] = 4; indices[10] = 7; indices[11] = 3;
            indices[12] = 3; indices[13] = 7; indices[14] = 6;
            indices[15] = 3; indices[16] = 6; indices[17] = 2;
            indices[18] = 1; indices[19] = 5; indices[20] = 6;
            indices[21] = 1; indices[22] = 5; indices[23] = 2;
            indices[24] = 4; indices[25] = 5; indices[26] = 6;
            indices[27] = 4; indices[28] = 7; indices[29] = 6;
            indices[30] = 0; indices[31] = 1; indices[32] = 5;
            indices[33] = 0; indices[34] = 4; indices[35] = 5;
            indexBuffer.SetData(indices);

            InputManager.Instance.ButtonSequences.Add(new ButtonSequence(EnableAttackMode, KeyboardConfiguration.GetAttackSequence));
            SaveLoadManager.Instance.ObjectsToSave.Add(this);
        }

        #endregion

        #region properties


        /// <summary>
        /// Whether or not the mouse can use the hyper jump
        /// </summary>
        public bool CanJump
        {
            get { return canJump; }
            set
            {
                if (value)
                {
                    AudioManager.Instance.PlaySound("happyMouse");
                }
                else
                {
                    AudioManager.Instance.PlaySound("sadMouse");
                }
                canJump = value;
            }
        }

        /// <summary>
        /// Whether or not the mouse is currently in attack mode
        /// </summary>
        public bool IsInAttackMode
        {
            get { return isInAttackMode; }
        }

        public CooldownBar CDBar
        { get { return hyperJumpCooldown; } }

        public Vector2 FaceDirection
        {
            get; private set;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Updates the mouse
        /// </summary>
        public override void Update(GameTime gameTime)
        {

            if (!GameManager.Instance.IsPaused)
            {
                //if (InputManager.Instance.IsKey(Keys.W))
                //{
                //    for (int i = 0; i < vertices.Length; i++)
                //    {
                //        vertices[i].Position += Vector3.Up * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                //    }
                //}
                //if (InputManager.Instance.IsKey(Keys.S))
                //{
                //    for (int i = 0; i < vertices.Length; i++)
                //    {
                //        vertices[i].Position -= Vector3.Up * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                //    }
                //}
                //if (InputManager.Instance.IsKey(Keys.A))
                //{
                //    for (int i = 0; i < vertices.Length; i++)
                //    {
                //        vertices[i].Position -= Vector3.Right * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                //    }
                //}
                //if (InputManager.Instance.IsKey(Keys.D))
                //{
                //    for (int i = 0; i < vertices.Length; i++)
                //    {
                //        vertices[i].Position += Vector3.Right * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                //    }
                //}
                CubePosition = new Vector3(Position.X / Constants.WINDOW_WIDTH, Position.Y / Constants.WINDOW_HEIGHT, 0);

                vertices[0].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[1].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[2].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[3].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[4].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[5].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[6].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[7].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, depth);

                ActualCubePosition = new Vector3(vertices[0].Position.X + width / 2, vertices[0].Position.Y + height / 2, vertices[0].Position.Z);

                //User controls
                velocity = Vector2.Zero;

                speed = baseSpeed;
                if (isInAttackMode && attackMode.ActivateEffect(gameTime.ElapsedGameTime.Milliseconds))
                {
                    scale = new Vector2(50f / 13f, 50f / 13f);
                    speed = baseSpeed * .75f;
                    if (frameCounter >= framesToSwapColorWhenAttacking)
                    {
                        frameCounter = 0;
                        if (color == Color.White)
                        {
                            color = Color.Yellow;
                            SpriteManager.Instance.AreCatsNormColor = true;
                        }
                        else
                        {
                            color = Color.White;
                            SpriteManager.Instance.AreCatsNormColor = false;
                        }
                    }
                    else
                    {
                        frameCounter++;
                    }
                }
                else if (isInAttackMode)
                {
                    isInAttackMode = false;
                    scale = Vector2.One;
                    color = Color.White;
                    SpriteManager.Instance.AreCatsNormColor = true;
                    //reset everything
                }

                if (!isInAttackMode
                    && InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveUp, KeyboardConfiguration.Boost.FirstOrDefault(t => InputManager.Instance.IsKey(t)) })
                    && boost.ActivateEffect(gameTime.ElapsedGameTime.Milliseconds))
                {
                    speed = baseSpeed * 1.25f;
                    velocity = new Vector2(FaceDirection.X * speed, -FaceDirection.Y * speed);
                }
                else if (InputManager.Instance.IsKey(KeyboardConfiguration.MoveUp))
                {
                    velocity = new Vector2(FaceDirection.X * speed, -FaceDirection.Y * speed);
                }
                if (!isInAttackMode
                    && InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveDown, KeyboardConfiguration.Boost.FirstOrDefault(t => InputManager.Instance.IsKey(t)) })
                    && boost.ActivateEffect(gameTime.ElapsedGameTime.Milliseconds))
                {
                    speed = baseSpeed * 1.25f;
                    velocity = new Vector2(-FaceDirection.X * speed, FaceDirection.Y * speed);
                }
                else if (InputManager.Instance.IsKey(KeyboardConfiguration.MoveDown))
                {
                    velocity = new Vector2(-FaceDirection.X * speed, FaceDirection.Y * speed);
                }
                if (InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveLeft, Keys.LeftControl}))
                {
                    if (FaceDirection == new Vector2(0, 1))
                    {
                        velocity = new Vector2(-1 * speed, 0);
                    }
                    else if (FaceDirection == new Vector2(1, 0))
                    {
                        velocity = new Vector2(0, -1 * speed);
                    }
                    else if (FaceDirection == new Vector2(0, -1))
                    {
                        velocity = new Vector2(1 * speed, 0);
                    }
                    else if (FaceDirection == new Vector2(-1, 0))
                    {
                        velocity = new Vector2(0, 1 * speed);
                    }
                }
                else if (InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveRight, Keys.LeftControl }))
                {
                    if (FaceDirection == new Vector2(0, 1))
                    {
                        velocity = new Vector2(1 * speed, 0);
                    }
                    else if (FaceDirection == new Vector2(1, 0))
                    {
                        velocity = new Vector2(0, 1 * speed);
                    }
                    else if (FaceDirection == new Vector2(0, -1))
                    {
                        velocity = new Vector2(-1 * speed, 0);
                    }
                    else if (FaceDirection == new Vector2(-1, 0))
                    {
                        velocity = new Vector2(0, -1 * speed);
                    }
                }
                else if (!InputManager.Instance.IsKey(Keys.LeftControl))
                {
                    if (!isInAttackMode
                        && InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveLeft, KeyboardConfiguration.Boost.FirstOrDefault(t => InputManager.Instance.IsKey(t)) })
                        && boost.ActivateEffect(gameTime.ElapsedGameTime.Milliseconds))
                    {
                        speed = baseSpeed * 1.25f;
                        if (FaceDirection == new Vector2(0, 1))
                        {
                            FaceDirection = new Vector2(-1, 0);
                        }
                        else if (FaceDirection == new Vector2(1, 0))
                        {
                            FaceDirection = new Vector2(0, 1);
                        }
                        else if (FaceDirection == new Vector2(0, -1))
                        {
                            FaceDirection = new Vector2(1, 0);
                        }
                        else if (FaceDirection == new Vector2(-1, 0))
                        {
                            FaceDirection = new Vector2(0, -1);
                        }
                    }
                    else if (InputManager.Instance.IsKeyDown(KeyboardConfiguration.MoveLeft))
                    {
                        if (FaceDirection == new Vector2(0, 1))
                        {
                            FaceDirection = new Vector2(-1, 0);
                        }
                        else if (FaceDirection == new Vector2(1, 0))
                        {
                            FaceDirection = new Vector2(0, 1);
                        }
                        else if (FaceDirection == new Vector2(0, -1))
                        {
                            FaceDirection = new Vector2(1, 0);
                        }
                        else if (FaceDirection == new Vector2(-1, 0))
                        {
                            FaceDirection = new Vector2(0, -1);
                        }
                    }
                    if (!isInAttackMode
                        && InputManager.Instance.AreKeysDown(new List<Keys>() { KeyboardConfiguration.MoveRight, KeyboardConfiguration.Boost.FirstOrDefault(t => InputManager.Instance.IsKey(t)) })
                        && boost.ActivateEffect(gameTime.ElapsedGameTime.Milliseconds))
                    {
                        speed = baseSpeed * 1.25f;
                        if (FaceDirection == new Vector2(0, 1))
                        {
                            FaceDirection = new Vector2(1, 0);
                        }
                        else if (FaceDirection == new Vector2(1, 0))
                        {
                            FaceDirection = new Vector2(0, -1);
                        }
                        else if (FaceDirection == new Vector2(0, -1))
                        {
                            FaceDirection = new Vector2(-1, 0);
                        }
                        else if (FaceDirection == new Vector2(-1, 0))
                        {
                            FaceDirection = new Vector2(0, 1);
                        }
                    }
                    else if (InputManager.Instance.IsKeyDown(KeyboardConfiguration.MoveRight))
                    {
                        if (FaceDirection == new Vector2(0, 1))
                        {
                            FaceDirection = new Vector2(1, 0);
                        }
                        else if (FaceDirection == new Vector2(1, 0))
                        {
                            FaceDirection = new Vector2(0, -1);
                        }
                        else if (FaceDirection == new Vector2(0, -1))
                        {
                            FaceDirection = new Vector2(-1, 0);
                        }
                        else if (FaceDirection == new Vector2(-1, 0))
                        {
                            FaceDirection = new Vector2(0, 1);
                        }
                    }
                }
               


                if (!isInAttackMode && hyperJumpCooldown.IsReady && InputManager.Instance.IsKey(KeyboardConfiguration.MainKey))
                {
                    WarpToRandomLocation();
                    hyperJumpCooldown.ResetTimer();
                }



                base.Update(gameTime);
            }
        }

        /// <summary>
        /// Respawns the mouse in a new location
        /// </summary>
        /// <param name="location">location to spawn in</param>
        public override void RespawnInNewLocation(Vector2 location)
        {
            base.RespawnInNewLocation(location);
            hyperJumpCooldown.ResetTimer();
            attackMode.ResetTimer();
            boost.ResetTimer();
        }


        /// <summary>
        /// Warps the sprite to a random location within the bounds of the screen
        /// </summary>
        public void WarpToRandomLocation()
        {
            Position = new Vector2(rand.Next(0, Constants.WINDOW_WIDTH), rand.Next(0, Constants.WINDOW_HEIGHT));
        }

        /// <summary>
        /// Called from an event fire
        /// </summary>
        public void EnableAttackMode()
        {
            isInAttackMode = true;
            Console.WriteLine("Enabled attack mode");
        }

        /// <summary>
        /// Gets the farthest corner from the mouse
        /// </summary>
        /// <returns>the farthest corner</returns>
        public Vector2 GetFarthestCorner()
        {
            Vector2[] corners = new Vector2[] { new Vector2(0, 0), new Vector2(Constants.WINDOW_WIDTH, 0), new Vector2(0, Constants.WINDOW_HEIGHT), new Vector2(Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT) };
            Vector2 farthestCorner = corners[0];

            foreach (Vector2 corner in corners)
            {
                if (Vector2.Distance(Position, corner) > Vector2.Distance(Position, farthestCorner))
                {
                    farthestCorner = corner;
                }
            }
            return farthestCorner;
        }

        public void LoadSerializableMouse(SaveableMouse saveData)
        {
            canJump = saveData.canJump;
            isInAttackMode = saveData.isInAttackMode;
            Position = saveData.position;
            scale = saveData.scale;
        }

        /// <summary>
        /// Draws the 3D model of the mouse
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(ModelManager.Instance.Camera.world);
            effect.Parameters["View"].SetValue(ModelManager.Instance.Camera.view);
            effect.Parameters["Projection"].SetValue(ModelManager.Instance.Camera.projection);
            effect.Parameters["Color"].SetValue(new float[] { color.R, color.G, color.B, 1f });

            //GameManager.Instance.Graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //GameManager.Instance.Graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 8);
                //GameManager.Instance.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 4);
                GameManager.Instance.Graphics.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, 8, indices, 0, 34);
            }
        }

        #endregion

    }
}
