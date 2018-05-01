using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    static class Font
    {
        public const int Margin = 5;

        public static SpriteFont BigText { get; private set; }
        public static SpriteFont Text { get; private set; }
        public static SpriteFont SmallText { get; private set; }
        public static SpriteFont Title { get; private set; }
        public static SpriteFont MenuTitle { get; private set; }
        public static SpriteFont MenuText { get; private set; }
        public static SpriteFont PopUpTitle { get; private set; }

        public static void LoadContent(ContentManager content)
        {
            BigText = content.Load<SpriteFont>("Font/BigText");
            Text = content.Load<SpriteFont>("Font/Text");
            SmallText = content.Load<SpriteFont>("Font/SmallText");
            Title = content.Load<SpriteFont>("Font/Title");
            MenuTitle = content.Load<SpriteFont>("Font/MenuTitle");
            MenuText = content.Load<SpriteFont>("Font/MenuText");
            PopUpTitle = content.Load<SpriteFont>("Font/PopUpTitle");
        }
    }
}
