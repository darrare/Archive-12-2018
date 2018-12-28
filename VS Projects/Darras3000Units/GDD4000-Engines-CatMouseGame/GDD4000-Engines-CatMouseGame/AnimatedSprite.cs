using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GDD4000_Engines_CatMouseGame
{
    class AnimatedSprite : Sprite
    {
        // Animated sprite's velocity
        public Vector2 Velocity { get; set; }

        // Constructor
        public AnimatedSprite(Game game, Texture2D texture, Vector2 initPosition, Vector2 initVelocity)
            : base(game, texture, initPosition)
        {
            Velocity = initVelocity;
        }

        public bool IsInCollision(AnimatedSprite sprite)
        {
            AnimatedSprite A = this;
            AnimatedSprite B = sprite;

            // Compute Minkowski sum
            float sumWidth = 0.5f * (A.Texture.Width + B.Texture.Width);
            float sumHeight = 0.5f * (A.Texture.Height + B.Texture.Height);
            float xCenterDist = A.Origin.X - B.Origin.X;
            float yCenterDist = A.Origin.Y - B.Origin.Y;

            // Is there a collision
            return (Math.Abs(xCenterDist) <= sumWidth && Math.Abs(yCenterDist) <= sumHeight);
        }

        public void Collide(AnimatedSprite sprite)
        {
            Vector2 xAxis = new Vector2(1, 0);
            Vector2 yAxis = new Vector2(0, 1);

            AnimatedSprite A = this;
            AnimatedSprite B = sprite;

            // Compute Minkowski sum
            float sumWidth = 0.5f * (A.Texture.Width + B.Texture.Width);
            float sumHeight = 0.5f * (A.Texture.Height + B.Texture.Height);
            float xCenterDist = A.Origin.X - B.Origin.X;
            float yCenterDist = A.Origin.Y - B.Origin.Y;

            // Is there a collision
            if (Math.Abs(xCenterDist) <= sumWidth && Math.Abs(yCenterDist) <= sumHeight)
            {
                float wy = sumWidth * yCenterDist;
                float hx = sumHeight * xCenterDist;

                if (wy > hx)
                {
                    // If the collision is at the top
                    if (wy > -hx)
                    {
                        Rebound(yAxis, A);
                        Rebound(yAxis, B);
                    }
                    // If the collision is on the right
                    else
                    {
                        Rebound(xAxis, A);
                        Rebound(xAxis, B);
                    }
                }
                else
                {
                    // If the collision is on the left
                    if (wy > -hx)
                    {
                        Rebound(xAxis, A);
                        Rebound(xAxis, B);
                    }
                    // If the collision is on the bottom
                    else
                    {
                        Rebound(yAxis, A);
                        Rebound(yAxis, B);
                    }
                }
            }

            // To catch the weird overlap cases "push" the 
            // two apart based on their overlap
            if (IsInCollision(sprite))
            {
                Rectangle overlap = Rectangle.Intersect(A.Rect, B.Rect);

                // if the width is smaller than the height, "push" them apart along the x
                if (overlap.Width < overlap.Height)
                {
                    // If A is to the left
                    if (A.Position.X < B.Position.X)
                    {
                        A.Position = new Vector2(A.Position.X - overlap.Width * 0.5f, A.Position.Y);
                        B.Position = new Vector2(B.Position.X + overlap.Width * 0.5f, B.Position.Y);
                    }
                    else
                    {
                        A.Position = new Vector2(A.Position.X + overlap.Width * 0.5f, A.Position.Y);
                        B.Position = new Vector2(B.Position.X - overlap.Width * 0.5f, B.Position.Y);
                    }
                }
                // Otherwise, Height < Width, "push" apart along the y
                else
                {
                    // If A is above B
                    if (A.Position.Y < B.Position.Y)
                    {
                        A.Position = new Vector2(A.Position.X, A.Position.Y - overlap.Height * 0.5f);
                        B.Position = new Vector2(B.Position.X, B.Position.Y + overlap.Height * 0.5f);
                    }
                    else
                    {
                        A.Position = new Vector2(A.Position.X, A.Position.Y + overlap.Height * 0.5f);
                        B.Position = new Vector2(B.Position.X, B.Position.Y - overlap.Height * 0.5f);
                    }
                }
            }
        }

        private static void Rebound(Vector2 yAxis, AnimatedSprite sprite)
        {
            sprite.Position -= sprite.Velocity;

            // Reflect the velocity vector for the sprite
            sprite.Velocity = Vector2.Reflect(sprite.Velocity, yAxis);
            sprite.Velocity.Normalize();

            sprite.Position += sprite.Velocity;
        }

        // Update - calculate the new position based on the velocity, stay in the bounds of the screen
        public override void Update(GameTime gameTime)
        {
            Vector2 newPos = Position + Velocity;

            // If off the left or right side
            if (newPos.X < 0 || newPos.X > Game.GraphicsDevice.Viewport.Width - Texture.Width)
            {
                // Reflect the velocity and reverse direction
                Velocity = Vector2.Reflect(Velocity, new Vector2(1, 0));
                Velocity.Normalize();
                newPos = Position + Velocity;
            }

            // If off the top or bottom
            if (newPos.Y < 0 || newPos.Y > Game.GraphicsDevice.Viewport.Height - Texture.Height)
            {
                // Reflect the velocity and reverse direction
                Velocity = Vector2.Reflect(Velocity, new Vector2(0, 1));
                Velocity.Normalize();
                newPos = Position + Velocity;
            }

            // Copy over the newly adjusted position
            Position = newPos;
        }
    }
}
