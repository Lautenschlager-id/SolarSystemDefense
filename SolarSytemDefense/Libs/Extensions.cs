using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
	static class Extensions
	{
		// Vector2
		public static float Angle(this Vector2 vector)
		{
			return (float)Math.Atan2(vector.Y, vector.X);
		}
		public static float Angle(this Vector2 vector, Vector2 complement)
		{
			return (float)Math.Atan2(vector.Y - complement.Y, vector.X - complement.X);
		}

		public static Point ToPoint(this Vector2 vector)
		{
			return new Point((int)vector.X, (int)vector.Y);
		}

		// Texture2D
		public static void Fill(this Texture2D texture, int length = 1)
		{
			Color[] fill = new Color[length];
			for (int i = 0; i < fill.Length; i++)
				fill[i] = Color.White;
			texture.SetData(fill);
		}

		public static Vector2 Center(this Texture2D texture)
		{
			return new Vector2(texture.Width / 2f, texture.Height / 2f);
		}

		// Color
		public static Color[] Collection(this Color color, int indexes = 2)
		{
			Color[] Out = new Color[indexes];
			for (int i = 0; i < indexes; i++)
				Out[i] = color;
			return Out;
		}
	}
}
