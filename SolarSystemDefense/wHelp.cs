using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
    class wHelp : GameStage
    {
        public wHelp()
        {
            Main.Resize(400, 600);

            cBox Exit = new cBox("«", Font.Text, 0, 0, 50, Main.ViewPort.Height, true)
            {
                ComponentColor = Info.Colors["Button"],
                TextColor = Info.Colors["ButtonText"],
                Alpha = .5f
            };
            Exit.OnClick += new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Menu);
            ComponentManager.New(Exit);

            cLabel Text = new cLabel("Text", Font.Text, 0, 0)
            {
                ContentColor = new Color(146, 91, 255).Collection()
            };
            Vector2 Position = Text.GetCoordinates(Main.ViewPort.Bounds, "xcenter top", 50, 0);
            Text.SetPosition((int)Position.X, (int)Position.Y);
            ComponentManager.New(Text);
        }
    }
}
