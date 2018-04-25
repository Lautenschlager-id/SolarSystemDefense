using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    static class Font
    {
        public const int Margin = 5;

        public static SpriteFont Text { get; private set; }
        public static SpriteFont SmallText { get; private set; }
        public static SpriteFont Title { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            Text = content.Load<SpriteFont>("Font/Text");
            SmallText = content.Load<SpriteFont>("Font/SmallText");
            Title = content.Load<SpriteFont>("Font/Title");
        }
    }
}
