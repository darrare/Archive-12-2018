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
    class PlanetManager
    {
        static PlanetManager instance;

        const float scaleConstant = 50000; //What the diameter of the planet is divided by to get the scale
        const float distanceConstant = 1000f; //What the distance is multiplied by to get the proper distance

        /// <summary>
        /// accessor for the planetmanager class
        /// </summary>
        public static PlanetManager Instance
        { get { return instance != null ? instance : instance = new PlanetManager(); } }

        //The content manager
        public ContentManager ContentManager
        { get; set; }

        public Matrix View
        { get; set; }

        public Matrix Projection
        { get; set; }

        public Effect PerlinNoiseEffect
        { get; set; }


        /// <summary>
        /// The planets. Individually named for testing purposes.
        /// </summary>
        public Planet Sun
        { get; private set; }
        public Planet Mercury
        { get; private set; }
        public Planet Venus
        { get; private set; }
        public Planet Earth
        { get; private set; }
        public Planet Moon
        { get; private set; }
        public Planet Mars
        { get; private set; }
        public Planet Jupiter
        { get; private set; }
        public Planet Saturn
        { get; private set; }
        public Planet Uranus
        { get; private set; }
        public Planet Neptune
        { get; private set; }

        private PlanetManager()
        {

        }

        /// <summary>
        /// enchantedlearning.com/subjects/astronomy/planets
        /// </summary>
        public void GeneratePlanets()
        {
            Sun = new Planet(null, 4.5f, 0, 0, 27, false, "Sun", new PlanetEffectData("SunEffect", .15f, .25f));
            Mercury = new Planet(Sun, 1, 185, 87.96f, 58.7f, false, "Mercury");
            Venus = new Planet(Sun, 2f, 300, 224.68f, 243f, false, "Venus");
            Earth = new Planet(Sun, 2.1f, 450, 365.26f, 24f, false, "Earth", new PlanetEffectData("CloudsEffect", .05f, .1f));
            Moon = new Planet(Earth, .5f, 150f, 27.322f, 27f, false, "Mercury");
            Mars = new Planet(Sun, 1.7f, 600, 686.98f, 24.6f, false, "Mars");
            Jupiter = new Planet(Sun, 4f, 1100, 11.862f * 365.26f, 9.84f, false, "Jupiter");
            Saturn = new Planet(Sun, 3.8f, 1500, 29.456f * 365.26f, 10.2f, true, "Saturn");
            Uranus = new Planet(Sun, 3f, 2100, 84.07f * 365.26f, 17.9f, true, "Uranus");
            Neptune = new Planet(Sun, 2.8f, 2700, 164.81f * 365.26f, 19.2f, false, "Neptune");


            //Below is an accurate representation of the solar system, but this scale is unrealistic for this project.
            //Sun = new Planet(null, 864575.9f / scaleConstant, 0, 0, 27, "Sun", new PlanetEffectData("SunEffect", .15f, .25f));
            //Mercury = new Planet(Sun, 3031f / scaleConstant, .39f * distanceConstant, 87.96f, 58.7f, "Mercury");
            //Venus = new Planet(Sun, 7521f / scaleConstant, .723f * distanceConstant, 224.68f, 243f, "Venus");
            //Earth = new Planet(Sun, 7926f / scaleConstant, 1f * distanceConstant, 365.26f, 24f, "Earth", new PlanetEffectData("CloudsEffect", .05f, .1f));
            //Mars = new Planet(Sun, 4222f / scaleConstant, 1.524f * distanceConstant, 686.98f, 24.6f, "Mars");
            //Jupiter = new Planet(Sun, 88729f / scaleConstant, 5.203f * distanceConstant, 11.862f * 365.26f, 9.84f, "Jupiter");
            //Saturn = new Planet(Sun, 74600f / scaleConstant, 9.539f * distanceConstant, 29.456f * 365.26f, 10.2f, "Saturn");
            //Uranus = new Planet(Sun, 32600f / scaleConstant, 19.18f * distanceConstant, 84.07f * 365.26f, 17.9f, "Uranus");
            //Neptune = new Planet(Sun, 30200f / scaleConstant, 30.06f * distanceConstant, 164.81f * 365.26f, 19.2f, "Neptune");
        }

        public void Update(GameTime gameTime)
        {
            if (Sun != null)
            {
                Sun.Update(gameTime);
                Mercury.Update(gameTime);
                Venus.Update(gameTime);
                Earth.Update(gameTime);
                Moon.Update(gameTime);
                Mars.Update(gameTime);
                Jupiter.Update(gameTime);
                Saturn.Update(gameTime);
                Uranus.Update(gameTime);
                Neptune.Update(gameTime);
            }
        }

        public void DrawPlanets(GameTime gameTime)
        {
            if (Sun != null)
            {
                Sun.Draw();
                Mercury.Draw();
                Venus.Draw();
                Earth.Draw();
                Moon.Draw();
                Mars.Draw();
                Jupiter.Draw();
                Saturn.Draw();
                Uranus.Draw();
                Neptune.Draw();
            }
        }


    }
}
