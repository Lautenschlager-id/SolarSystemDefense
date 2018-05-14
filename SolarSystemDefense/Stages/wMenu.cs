using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    class wMenu : GameWindow
    {
        public wMenu()
        {
            cBox Button = new cBox(0, 0, 180, 40); // Getting coordinates only

            List<List<object>> ButtonTexts = new List<List<object>>() {
                new List<object> { "Play", new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Playing) },
                new List<object> { "Map Editor", new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.MapEditor) },
                new List<object> { "Help", new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Help) },
                new List<object> { "Exit", new EventHandler((obj, arg) => Main.Instance.Exit()) },
            };

            Main.Resize(180, ButtonTexts.Count * 50);

            Vector2 Position = Button.GetCoordinates(Main.ViewPort.Bounds, "xcenter", 0, -45); // Getting coordinates only
            foreach (List<object> buttonInfo in ButtonTexts)
            {
                Button = new cBox((string)buttonInfo[0], Font.Text, 0, 0, 180, 40, true)
                {
                    ComponentColor = Info.Colors["Button"],
                    TextColor = Info.Colors["ButtonText"],
                    Alpha = .5f
                };
                Position.Y += 50;
                Button.SetPosition((int)Position.X, (int)Position.Y);
                Button.OnClick += (EventHandler)buttonInfo[1];
                ComponentManager.New(Button);
            }
        }
    }
}
