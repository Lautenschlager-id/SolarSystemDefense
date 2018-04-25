using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    abstract class GameStage
    {
        public abstract void Update();
        public abstract void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth);
    }
}