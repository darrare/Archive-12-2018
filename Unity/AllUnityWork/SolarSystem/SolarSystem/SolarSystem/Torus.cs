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
    class Torus
    {
        Model torus;

        public Matrix World
        { get; set; }

        public Planet OrbitingTarget
        { get; private set; }

        public Torus(Planet orbitingPlanet)
        {
            torus = PlanetManager.Instance.ContentManager.Load<Model>(@"sphere");
            World = orbitingPlanet.World * Matrix.CreateRotationZ(1.5f);
            OrbitingTarget = orbitingPlanet;
        }

        public void Update(GameTime gameTime)
        {
            World = OrbitingTarget.World;
            World *= Matrix.CreateTranslation(-World.Translation);
            World *= Matrix.CreateScale(1.7f, .001f, 1.7f);
            World *= Matrix.CreateFromAxisAngle(Vector3.One, -.1f);
            World *= Matrix.CreateTranslation(OrbitingTarget.World.Translation);
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[torus.Bones.Count];
            torus.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in torus.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.CurrentTechnique = PlanetManager.Instance.PerlinNoiseEffect.Techniques["Mercury"];
                    effect.Parameters["World"].SetValue(World);
                    effect.Parameters["View"].SetValue(PlanetManager.Instance.View);
                    effect.Parameters["Projection"].SetValue(PlanetManager.Instance.Projection);
                    //effect.Parameters["CloudsEffect"].SetValue(cloudTimer);
                    //effect.Parameters["SunEffect"].SetValue(timer);

                }
                mesh.Draw();
            }
        }
    }
}
