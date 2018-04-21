using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    class cSquare : Component
    {
        Boolean HasContent = false;
        Boolean ToggleColorOnHover;

        Texture2D ContentTexture;
        Vector2 ContentPosition, ContentRadius;

        public Vector2 ContentSize
        {
            get
            {
                return new Vector2(ContentTexture.Width, ContentTexture.Height);
            }
        }

        public cSquare(int XPosition, int YPosition, int Width, int Height, Boolean ToggleColorOnHover = false)
            : base(XPosition, YPosition, Width, Height)
        {
            ComponentColor = Color.DarkSlateBlue.Collection();
            this.ToggleColorOnHover = ToggleColorOnHover;
        }

        public cSquare(Texture2D Texture, int XPosition, int YPosition, int Width, int Height)
            : base(XPosition, YPosition, Width, Height)
        {
            HasContent = true;

            SetContent(Texture);
            SetPosition(XPosition, YPosition);
        }

        public void SetContent(Texture2D NewContent)
        {
            if (HasContent)
            {
                ContentTexture = NewContent;
                ContentRadius = new Vector2(NewContent.Width, NewContent.Height) / 2f;
            }
        }

        public override void SetPosition(int XPosition, int YPosition)
        {
            if (HasContent)
                ContentPosition = new Vector2(XPosition + (Shape.Width / 2), YPosition + (Shape.Height / 2));
            base.SetPosition(XPosition, YPosition);
        }

        public override void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            if (!Remove)
            {
                BackgroundDepth.Draw(Texture, Shape, ComponentColor[(ToggleColorOnHover && MouseHover) ? 1 : 0] * Alpha);
                if (HasContent)
                    BackgroundDepth.Draw(ContentTexture, ContentPosition, null, Color.White * Alpha, 0, ContentRadius, 1f, 0, 0);
            }
        }
    }
}
