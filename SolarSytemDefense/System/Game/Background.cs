using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
	static class Background
	{
		static float backgroundPositionX = -50, backgroundPositionY = -20;
		static int backgroundXVelocity = -2, backgroundYVelocity = -2;

		public static void Draw(SpriteBatch Layer)
		{
			backgroundPositionX += .05f * backgroundXVelocity;
			if (backgroundPositionX < -500 || backgroundPositionX > -10)
				backgroundXVelocity = -backgroundXVelocity;

			backgroundPositionY += .03f * backgroundYVelocity;
			if (backgroundPositionY < -450 || backgroundPositionY > -10)
				backgroundYVelocity = -backgroundYVelocity;

			Layer.Draw(Graphic.Background, new Vector2(backgroundPositionX, backgroundPositionY), Color.White);
		}
	}
}
