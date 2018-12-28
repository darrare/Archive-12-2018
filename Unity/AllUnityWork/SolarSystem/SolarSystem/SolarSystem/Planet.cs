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
    class Planet
    {

        Matrix t;

        public Planet OrbitingTarget
        { get; private set; }

        public float Scale
        { get; private set; }

        public float DistanceToOrbit
        { get; private set; }

        public float YearDuration
        { get; private set; }

        public float DayDuration
        { get; private set; }

        public string Technique
        { get; private set; }

        public PlanetEffectData AdditionalParam
        { get; private set; }

        public Model Sphere
        { get; private set; }

        public Matrix World
        { get; set; }

        public Torus Ring
        { get; private set; }

        /// <summary>
        /// Constructor for the planet class
        /// </summary>
        /// <param name="orbitTarget">The target this planet will orbit around</param>
        /// <param name="scale">The scale of the planet</param>
        /// <param name="distanceFromTarget">The distance from the object we are orbiting around. In Astronomical Units</param>
        /// <param name="yearDuration">Duration of one orbit in earth days</param>
        /// <param name="dayDuration">Duration of one rotation in earth hours</param>
        /// <param name="effectName">Name of the effect we are going to use to draw this planet</param>
        /// <param name="additionalParam">Any aditional parameters an effect needs as well as a timer to support it</param>
        public Planet(Planet orbitTarget, float scale, float distanceFromTarget, float yearDuration, float dayDuration, bool hasRing, string effectName, PlanetEffectData additionalParam = null)
        {
            OrbitingTarget = orbitTarget;
            DistanceToOrbit = distanceFromTarget;
            YearDuration = yearDuration;
            DayDuration = dayDuration;
            Technique = effectName;
            AdditionalParam = additionalParam;
            Sphere = PlanetManager.Instance.ContentManager.Load<Model>(@"sphere");
            if (orbitTarget != null)
            {
                Matrix translate = Matrix.CreateScale(scale) * Matrix.CreateRotationZ(1.5f) * Matrix.CreateTranslation(new Vector3(OrbitingTarget.World.Translation.X + distanceFromTarget, 0, 0));
                World = translate;
            }
            else
            {
                World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(new Vector3(0, 0, 0)) * Matrix.CreateRotationX(1.5f);
            }

            if (hasRing)
            {
                Ring = new Torus(this); //TODO: Add rings
            }
            
            
        }

        public void Update(GameTime gameTime)
        {
            if (AdditionalParam != null)
            {
                AdditionalParam.Update(gameTime);
            }
            if (OrbitingTarget != null)
            {
                t = World;
                t *= Matrix.CreateTranslation(-World.Translation); //translate to origin
                t *= Matrix.CreateRotationY(.25f / DayDuration); //rotate the day
                t *= Matrix.CreateTranslation(World.Translation - OrbitingTarget.World.Translation); //translate to where we should be relative to orbiting target
                t *= Matrix.CreateRotationY(3 / YearDuration); //rotate for the year
                t *= Matrix.CreateTranslation(OrbitingTarget.World.Translation); //rotates back to our planet. Does nothing when planet is the sun
                World = t;
            }
            else
            {
                t *= Matrix.CreateRotationY(.25f / DayDuration); //rotate the day
            }
            
            if (Ring != null)
            {
                Ring.Update(gameTime);
            }
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[Sphere.Bones.Count];
            Sphere.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Sphere.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = PlanetManager.Instance.PerlinNoiseEffect.Techniques[Technique];
                    effect.Parameters["World"].SetValue(World);
                    effect.Parameters["View"].SetValue(PlanetManager.Instance.View);
                    effect.Parameters["Projection"].SetValue(PlanetManager.Instance.Projection);
                    //effect.Parameters["CloudsEffect"].SetValue(cloudTimer);
                    //effect.Parameters["SunEffect"].SetValue(timer);
                    if (AdditionalParam != null)
                    {
                        effect.Parameters[AdditionalParam.ParamName].SetValue(AdditionalParam.Timer);
                    }
                }
                mesh.Draw();
            }

            if (Ring != null)
            {
                Ring.Draw();
            }
        }
    }

    class PlanetEffectData
    {
        public bool IsGoingUp;
        public string ParamName;
        public float Min;
        public float Max;
        public float Timer;

        public PlanetEffectData(string param, float min, float max)
        {
            IsGoingUp = true;
            ParamName = param;
            Min = min;
            Max = max;
            Timer = min;
        }

        public void Update(GameTime gameTime)
        {
            if (IsGoingUp && Timer <= Max)
            {
                Timer += (float)gameTime.ElapsedGameTime.Milliseconds / 100000;
            }
            else
            {
                IsGoingUp = false;
            }

            if (!IsGoingUp && Timer >= Min)
            {
                Timer -= (float)gameTime.ElapsedGameTime.Milliseconds / 100000;
            }
            else
            {
                IsGoingUp = true;
            }
        }
    }
}
