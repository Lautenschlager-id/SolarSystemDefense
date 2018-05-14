using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    class wPresentation : GameWindow
    {
        const float tUpdate = .02f;
        const int GoNext = 6;

        float Timer = 0;
        int CurrentImageID = 0;
        float Alpha = 1;

        Texture2D CurrentImage;
        public wPresentation()
        {
            CurrentImage = Graphic.Presentation[CurrentImageID];
        }

        public override void Update()
        {
            Timer += tUpdate;
            if (Math.Floor(Timer) >= GoNext)
            {
                if (++CurrentImageID < Graphic.Presentation.Length)
                {
                    CurrentImage = Graphic.Presentation[CurrentImageID];
                    Alpha = 1;
                    Timer = 0;
                }
                else
                    Main.CurrentGameState = Main.GameState.Menu;
            }
        }

        public override void Draw(SpriteBatch Layer)
        {
            Alpha -= tUpdate / 05f;
            Layer.Draw(CurrentImage, new Vector2(Main.ViewPort.Width - CurrentImage.Width, Main.ViewPort.Height - CurrentImage.Height) / 2f, Color.White * Alpha);
        }
    }
}
