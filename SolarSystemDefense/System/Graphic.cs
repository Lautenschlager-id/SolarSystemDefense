using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SolarSystemDefense
{
    static class Graphic
    {
        public static Texture2D[] Presentation { get; private set; }

        public static Texture2D Background { get; private set; }

        public static Texture2D[] Bullets { get; private set; }
        public static Texture2D[] Shooters { get; private set; }
        public static Texture2D[] Enemies { get; private set; }
        public static Texture2D[] Features { get; private set; }

        public static Texture2D Pixel { get; private set; }

        public static Texture2D Earth { get; private set; }
        public static Texture2D Water { get; private set; }
        public static Texture2D UFO { get; private set; }

        public static Texture2D InfoPower { get; private set; }
        public static Texture2D InfoTimer { get; private set; }
        public static Texture2D InfoSpeed { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Background = content.Load<Texture2D>("Graphic/background");

            Bullets = Enumerable.Range(1, 4).Select(id => content.Load<Texture2D>("Graphic/Objects/Bullets/" + (id - 1))).ToArray();

            Shooters = Enumerable.Range(1, 4).Select(id => content.Load<Texture2D>("Graphic/Objects/Shooters/" + (id - 1))).ToArray();

            for (int i = 0; i < Shooters.Length; i++)
                Data.ShooterData[i].CollisionRadius.Value = Shooters[i].Width / 2f;

            Pixel = content.Load<Texture2D>("Graphic/pixel");

            Enemies = Enumerable.Range(1, 4).Select(id => content.Load<Texture2D>("Graphic/Objects/Enemies/" + (id - 1))).ToArray();

            for (int i = 0; i < Enemies.Length; i++)
                Data.EnemyData[i].CollisionRadius.Value = Enemies[i].Width / 2f;

            Earth = content.Load<Texture2D>("Graphic/Objects/object_earth");
            Water = content.Load<Texture2D>("Graphic/Objects/feature_water");
            UFO = content.Load<Texture2D>("Graphic/Objects/object_ufo");

            Features = Enumerable.Range(1, 2).Select(id => content.Load<Texture2D>("Graphic/Objects/Features/" + (id - 1))).ToArray();
            for (int i = 0; i < Features.Length; i++)
                Data.FeatureData[i].CollisionRadius.Value = Features[i].Width / 2f;

            InfoPower = content.Load<Texture2D>("Graphic/UI/popup_power");
            InfoTimer = content.Load<Texture2D>("Graphic/UI/popup_timer");
            InfoSpeed = content.Load<Texture2D>("Graphic/UI/popup_speed");

            Presentation = Enumerable.Range(1, 1).Select(id => content.Load<Texture2D>("Graphic/Presentation/" + (id - 1))).ToArray();
        }
    }
}