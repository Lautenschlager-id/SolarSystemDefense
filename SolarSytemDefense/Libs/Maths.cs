﻿using Microsoft.Xna.Framework;
using System;

namespace SolarSystemDefense
{
	static class Maths
	{
		public static Random Random = new Random();

		public static Vector2 PolarToVector(float angle, float magnitude)
		{
			return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		public static bool Pythagoras(Vector2 p1, Vector2 p2, float range)
		{
			return Vector2.DistanceSquared(p1, p2) < (range * range);
		}

		public static float Percent(float x, float t)
		{
			return (x / 100) * t;
		}
	}
}
