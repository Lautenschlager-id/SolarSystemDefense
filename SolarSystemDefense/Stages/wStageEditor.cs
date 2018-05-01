using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SolarSystemDefense
{
    class wStageEditor : GameStage
    {
        Info.StageTable Level = new Info.StageTable();

        bool DrawStage = true;

        List<cBox> Buttons = new List<cBox>();
        cBox ItemPanel;
        cLabel totalLines, tutorial;
        public wStageEditor()
        {
            Main.Resize(900, 600);

            ItemPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height) {
                ComponentColor = Info.Colors["Container"],
                Alpha = .5f
            };
            ComponentManager.New(ItemPanel);

            totalLines = new cLabel("", Font.Text, 0, 0)
            {
                ContentColor = Color.Yellow.Collection()
            };
            AlignLineCounter();
            ComponentManager.New(totalLines);

            List<List<object>> ButtonTexts = new List<List<object>>() {
                new List<object> { "Export code", null },
                new List<object> { "Copy stage code", new EventHandler((obj, arg) => {
                    if (Level.Walkpoints.Count > 1)
                    {
                        MemoryStream memory = new MemoryStream();

                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Info.StageTable));
                        serializer.WriteObject(memory, Level);

                        byte[] JSON = memory.ToArray();
                        memory.Close();

                        System.Windows.Forms.Clipboard.SetText(Encoding.UTF8.GetString(JSON, 0, JSON.Length));
                    }
                })},
                new List<object> { "Reset building", new EventHandler((obj, arg) => {
                    Level.Walkpoints.Clear();
                    AlignLineCounter();

                    foreach (cBox btn in Buttons)
                        btn.Visible = (DrawStage && Level.Walkpoints.Count > 1);
                    DrawStage = true;
                })},
                new List<object> { "Exit", new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Menu) },
            };

            cBox Button;
            Vector2 Position = ItemPanel.GetCoordinates(ItemPanel.GetDimension, "xcenter", 0, 50);
            foreach (List<object> buttonInfo in ButtonTexts)
            {
                Button = new cBox((string)buttonInfo[0], Font.Text, 0, 0, 180, 40, true)
                {
                    Visible = false,
                    ComponentColor = Info.Colors["Button"],
                    TextColor = Info.Colors["ButtonText"],
                    Alpha = .5f
                };
                Position.Y += 50;
                Button.SetPosition((int)Position.X, (int)Position.Y);
                Button.OnClick += (EventHandler)buttonInfo[1];
                Buttons.Add(Button);
                ComponentManager.New(Button);
            }

            tutorial = new cLabel(
                "Click in the map besides\n" +
                "this container to define\n" +
                "the enemies route.\n\n" +
                "The initial line is\n" +
                "highlighted in green and\n" +
                "the final line will be red!\n\n" +
                "Press space to stop\n" +
                "building and click in\n" +
                "\"Copy stage code\" to\n" +
                "put the stage code in\n" +
                "your clipboard!", Font.SmallText, 0, 0)
            {
                ContentColor = new Color(146, 91, 255).Collection()
            };
            Position = tutorial.GetCoordinates(ItemPanel.GetDimension, "xcenter bottom", 0, -50);
            tutorial.SetPosition((int)Position.X, (int)Position.Y);
            ComponentManager.New(tutorial);

            Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ItemPanel.GetSize.X, Main.ViewPort.Height);
        }

        public void AlignLineCounter()
        {
            int index = (Level.Walkpoints.Count - 1);
            totalLines.Text = (index < 0 ? 0 : index) + " / 50";
            Vector2 pos = totalLines.GetCoordinates(ItemPanel.GetDimension, "xcenter top", 0, 10);
            totalLines.SetPosition((int)pos.X, (int)pos.Y);
        }

        public override void Update()
        {
            if (Control.KeyDown(Keys.Space))
            {
                if (Level.Walkpoints.Count > 1 || !DrawStage)
                {
                    foreach (cBox btn in Buttons)
                        btn.Visible = (DrawStage && Level.Walkpoints.Count > 1);
                    DrawStage = !DrawStage;
                }
            }
            else if (DrawStage)
            {
                bool alter = false;
                if (alter = Control.KeyDown(Keys.Delete))
                {
                    int index = Level.Walkpoints.Count - 1;
                    if (index >= 0)
                        Level.Walkpoints.RemoveAt(index);
                }
                else if (alter = (Control.MouseClicked && Level.Walkpoints.Count < 51))
                    Level.Walkpoints.Add(Vector2.Clamp(Control.MouseCoordinates, Vector2.Zero, new Vector2(Main.GameBound.Width, Main.GameBound.Height)));
                if (alter)
                    AlignLineCounter();
            }
        }

        public override void Draw(SpriteBatch Layer)
        {
            for (int p = 0; p < Level.Walkpoints.Count; p++)
            {
                Vector2 p1, p2;

                if (p == Level.Walkpoints.Count - 1)
                {
                    p1 = Level.Walkpoints[p];
                    p2 = DrawStage ? Vector2.Clamp(Control.MouseCoordinates, Vector2.Zero, new Vector2(Main.GameBound.Width, Main.GameBound.Height)) : p1;
                }
                else
                {
                    p1 = Level.Walkpoints[p + 1];
                    p2 = Level.Walkpoints[p];
                }

                new Utils.Line(p1, p2, 2, p == 0 ? Color.LimeGreen : p == Level.Walkpoints.Count - 2 ? Color.DarkRed : Color.Yellow).Draw(Layer);
            } 
        }
    }
}