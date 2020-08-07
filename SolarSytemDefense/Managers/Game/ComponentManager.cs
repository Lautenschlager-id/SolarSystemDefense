using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SolarSystemDefense
{
	static class ComponentManager
	{
		static List<Component> Components = new List<Component>();

		public static void Clear()
		{
			Components.Clear();
		}

		public static void New(Component c)
		{
			Components.Add(c);
		}

		public static void Update()
		{
			foreach (Component c in Components.GetRange(0, Components.Count))
				c.Update();

			Components = Components.Where(e => !e.Remove).ToList();
		}

		public static void Draw(SpriteBatch Layer)
		{
			foreach (Component c in Components.GetRange(0, Components.Count))
				c.Draw(Layer);
		}
	}
}
