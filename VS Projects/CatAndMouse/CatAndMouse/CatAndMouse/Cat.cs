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
    class Cat : AutoOrientingSprite
    {

        #region fields

        bool isChasing = false;

        Effect effect;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        VertexPositionColor[] vertices;
        short[] indices;
        float width = .1f;
        float height = .1f;
        float depth = .1f;
        Color norm, alt;

        #endregion

        #region constructor

        /// <summary>
        /// constructor for the mouse
        /// </summary>
        /// <param name="speed">the speed at which the mouse moves</param>
        public Cat(Vector2 position, float speed)
            : base("cat", speed)
        {
            this.speed = speed;
            velocity = new Vector2(0, -1);
            this.position = position;
            norm = new Color(0.870588f, 0.721569f, 0.529412f);
            alt = new Color(0.498039f, 1, 0);

            effect = GameManager.Instance.Content.Load<Effect>(@"CatEffect");


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

            SaveLoadManager.Instance.ObjectsToSave.Add(this);
        }

        #endregion

        #region properties

        /// <summary>
        /// Accessor for whether or not the cat is currently chasing the mouse.
        /// </summary>
        public bool IsChasing
        {
            get { return isChasing; }
            set
            {
                if (value)
                {
                    AudioManager.Instance.PlaySound("happyCat");
                }
                else
                {
                    AudioManager.Instance.PlaySound("sadCat");
                }
                isChasing = value;
            }
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
                CubePosition = new Vector3(Position.X / Constants.WINDOW_WIDTH, Position.Y / Constants.WINDOW_HEIGHT, 0);

                vertices[0].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[1].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[2].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[3].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, 0);
                vertices[4].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[5].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, 0 - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[6].Position = new Vector3(width + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, depth);
                vertices[7].Position = new Vector3(0 + (CubePosition.X * 4) - 2 - width / 2, -height - (CubePosition.Y * 4) + 2 + height / 2, depth);



                if (GameManager.Instance.Player.IsInAttackMode)
                {
                    velocity = Position - GameManager.Instance.Player.Position;
                }
                //Mouse is close enough, chase
                else if (IsChasing)
                {
                    velocity = GameManager.Instance.Player.Position - Position;
                }
                //Mouse is too far away, do nothing
                else
                {
                    velocity = Vector2.Zero;
                }

                //Hacked together nonsense
                SpriteManager.Instance.Cats.ForEach(t =>
                {
                    if (Vector2.Distance(t.Position, Position) < sprite.Width)
                    {
                        velocity += (Position - t.Position) * sprite.Width;
                    }
                });

                if (!GameManager.Instance.Player.IsInAttackMode && Vector2.Distance(Position, GameManager.Instance.Player.Position) < sprite.Width / 2 + ((GameManager.Instance.Player.Texture.Width / 2) * GameManager.Instance.Player.Scale.X))
                {
                    GameManager.Instance.AgentHasWon();
                }
                else if (Vector2.Distance(Position, GameManager.Instance.Player.Position) < sprite.Width / 2 + ((GameManager.Instance.Player.Texture.Width / 2) * GameManager.Instance.Player.Scale.X))
                {
                    Position = GameManager.Instance.Player.GetFarthestCorner();
                }

                ////rotate based on orientation
                //acceleration = target.Position - this.Position;
                //distance = Math.Abs(acceleration.Length());
                //if (distance < stopRadius)
                //{
                //    speed = 0;
                //    //game.AgentHasWon();
                //}
                //else if (distance < slowRadius)
                //{
                //    speed = maxSpeed * distance / slowRadius;
                //}
                //else
                //{
                //    speed = maxSpeed;
                //}
                //acceleration = Vector2.Normalize(target.Position - this.Position) * maxAccel;
                //velocity += velocity * gameTime.ElapsedGameTime.Milliseconds + .5f * acceleration * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds;
                //velocity = Vector2.Normalize(velocity) * speed;
                //position += velocity;

                //if (velocity != Vector2.Zero)
                //{
                //    orientation = velocity;
                //}

                base.Update(gameTime);
            }
        }

        public void LoadSerializableCat(SaveableCat saveData)
        {
            position = saveData.position;
        }

        public void Draw(GameTime gameTime)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["World"].SetValue(ModelManager.Instance.Camera.world);
            effect.Parameters["View"].SetValue(ModelManager.Instance.Camera.view);
            effect.Parameters["Projection"].SetValue(ModelManager.Instance.Camera.projection);
            if (SpriteManager.Instance.AreCatsNormColor)
            {
                effect.Parameters["Color"].SetValue(new float[] { norm.R / 255f, norm.G / 255f, norm.B / 255f, 1});
            }
            else
            {
                effect.Parameters["Color"].SetValue(new float[] { alt.R / 255f, alt.G / 255f, alt.B / 255f, 1});
            }
            
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