using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    class wHelp : GameWindow
    {
        int _image = 0;
        int Image {
            get
            {
                return _image;
            }
            set
            {
                if (value >= Graphic.Help.Length)
                    value = Graphic.Help.Length - 1;
                else if (value < 0)
                    Main.CurrentGameState = Main.GameState.Menu;

                if (value > 0)
                {
                    RightButton.ComponentColor = LeftButton.ComponentColor;
                    RightButton.TextColor = LeftButton.TextColor;
                }
                else
                {
                    RightButton.ComponentColor = Info.Colors["Button"];
                    RightButton.TextColor = Info.Colors["ButtonText"];
                }

                _image = value;
            }
        }

        Vector2 ImagePosition;

        cBox RightButton, LeftButton;
        public wHelp()
        {
            Main.Resize(450, 600);

            ImagePosition = new Vector2(110, Main.ScreenDimension.Y - Graphic.Help[0].Height) / 2f;

            RightButton = new cBox("«", Font.Text, 0, 0, 50, Main.ViewPort.Height, true)
            {
                ComponentColor = Info.Colors["Button"],
                TextColor = Info.Colors["ButtonText"],
                Alpha = .5f
            };
            RightButton.OnClick += new EventHandler((obj, arg) => Image--);
            ComponentManager.New(RightButton);

            LeftButton = new cBox("»", Font.Text, (int)Main.ScreenDimension.X - 50, 0, 50, Main.ViewPort.Height, true)
            {
                ComponentColor = new Color[] { new Color(240, 240, 240), Color.GhostWhite },
                TextColor = Color.White.Collection(),
                Alpha = .5f
            };
            LeftButton.OnClick += new EventHandler((obj, arg) => Image++);
            ComponentManager.New(LeftButton);
        }

        public override void Draw(SpriteBatch Layer)
        {
            Layer.Draw(Graphic.Help[Image], ImagePosition, Color.White);
        }
    }
}
