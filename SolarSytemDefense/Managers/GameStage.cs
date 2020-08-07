using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
	abstract class GameWindow
	{
		public virtual void Update() { }
		public virtual void Draw(SpriteBatch Layer) { }
	}
}