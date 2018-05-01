using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    abstract class GameStage
    {
        public virtual void Update() { }
        public virtual void Draw(SpriteBatch Layer) { }
    }
}