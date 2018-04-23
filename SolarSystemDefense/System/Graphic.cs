using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SolarSystemDefense
{
    static class Graphic
    {
        public static Texture2D Background { get; private set; }
        public static Texture2D MousePointer { get; private set; }

        public static Texture2D[] Bullets { get; private set; }
        public static Texture2D[] Shooters { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Background = content.Load<Texture2D>("Graphic/background");
            MousePointer = content.Load<Texture2D>("Graphic/mousepointer");

            Bullets = Enumerable.Range(1, 4).Select(id => content.Load<Texture2D>("Graphic/Objects/Bullets/bullet_" + (id - 1))).ToArray();

            Shooters = Enumerable.Range(1, 4).Select(id => content.Load<Texture2D>("Graphic/Objects/Shooters/shooter_" + (id - 1))).ToArray();

            for (int i = 0; i < 4; i++)
                Data.ShooterData[i].CollisionRadius.Value = Shooters[i].Width / 2f;
        }
    }
}