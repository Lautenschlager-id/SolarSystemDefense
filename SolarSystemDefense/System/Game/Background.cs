using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    static class Background
    {
        static float backgroundPositionX = -50, backgroundPositionY = -20;
        static int backgroundXVelocity = 1, backgroundYVelocity = 1;

        public static void Draw(SpriteBatch Layer)
        {
            backgroundPositionX += .05f * backgroundXVelocity;
            if (backgroundPositionX < -400 || backgroundPositionX > -50)
                backgroundXVelocity = -backgroundXVelocity;

            backgroundPositionY += .01f * backgroundYVelocity;
            if (backgroundPositionY < -100 || backgroundPositionY > -1)
                backgroundYVelocity = -backgroundYVelocity;

            Layer.Draw(Graphic.Background, new Vector2(backgroundPositionX, backgroundPositionY), Color.White);
        }
    }
}
