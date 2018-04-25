using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SolarSystemDefense
{
    class wGame : GameStage
    {
        enum RoundStage
        {
            PlacingShooters,
            Running,
            Paused
        }
        RoundStage CurrentStage;

        Info.StageTable Level = new Info.StageTable();

        List<List<int>> ActiveShooterDatas = new List<List<int>>();

        struct Selection
        {
            public int buildID;
            public float angle;
            public Texture2D shootArea;
            public bool allowBuild;
        }

        List<cBox> AvailableShooters = new List<cBox>();
        Selection SelectedShooter = new Selection()
        {
            buildID = -1,
            angle = 0,
            allowBuild = false
        };

        cBox ShooterPanel;
        cLabel ShooterTitle;
        public wGame()
        {
            Main.Resize(900, 600);

            CurrentStage = RoundStage.PlacingShooters;

            LoadStage();

            ShooterPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height)
            {
                ComponentColor = Info.Colors["Container"]
            };
            ShooterPanel.OnClick += new EventHandler((obj, arg) =>
            {
                if (SelectedShooter.buildID >= 0)
                    SelectedShooter.buildID = -1;
            });
            ComponentManager.New(ShooterPanel);

            Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ShooterPanel.GetSize.X, Main.ViewPort.Height);

            ShooterTitle = new cLabel("Shooters", Font.Title, 0, 0)
            {
                ContentColor = Color.LimeGreen.Collection()
            };
            Vector2 pos = ShooterTitle.GetCoordinates(ShooterPanel.GetDimension, "xcenter top", 0, 30);
            ShooterTitle.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(ShooterTitle);

            Enemy e = new Enemy(2, Level.Walkpoints[0]);
            EntityManager.New(e);

            cBox prot_square;
            for (int i = 0; i < 4; i++)
            {
                prot_square = new cBox(Graphic.Shooters[i], (int)ShooterPanel.GetPosition.X + 30 + (i % 2) * 70, 90 + (i / 2) * 90, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection(),
                    ID = i
                };
                prot_square.OnClick += new EventHandler((obj, arg) =>
                {
                    SelectedShooter.buildID = (obj as cBox).ID;
                    SelectedShooter.shootArea = Utils.CreateCircle(Data.ShooterData[SelectedShooter.buildID].Radius, Color.LightBlue);
                    SelectedShooter.allowBuild = true;
                });

                AvailableShooters.Add(prot_square);
            }
        }

        private void LoadStage()
        {
            string LoadedStage = "{\"Walkpoints\":[{\"X\":104,\"Y\":3},{\"X\":103,\"Y\":342},{\"X\":500,\"Y\":341},{\"X\":499,\"Y\":136},{\"X\":292,\"Y\":221},{\"X\":293,\"Y\":35}]}";

            MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(LoadedStage));

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(Level.GetType());
            Level = serializer.ReadObject(memory) as Info.StageTable;

            memory.Close();
        }

        public override void Update()
        {
            if (CurrentStage != RoundStage.Paused)
            {
                if (SelectedShooter.buildID >= 0)
                {
                    SelectedShooter.angle += (float)Control.MouseWheel / 5;
                    if (Control.MouseClicked && SelectedShooter.allowBuild)
                    {
                        Vector2 Position = Control.MouseCoordinates;

                        EntityManager.New(new Shooter(SelectedShooter.buildID, Position, SelectedShooter.angle));
                        ActiveShooterDatas.Add(new List<int>() { SelectedShooter.buildID, (int)Position.X, (int)Position.Y });

                        SelectedShooter.buildID = -1;
                    }
                }

                foreach (Enemy e in EntityManager.Enemies)
                    if (Math.Floor(Vector2.Distance(e.Position, Level.Walkpoints[e.LastWalkpoint])) <= Data.EnemyData[e.EnemyType].Speed * 2)
                    {
                        int index = e.LastWalkpoint + 1;
                        if (index < Level.Walkpoints.Count)
                        {
                            e.LastWalkpoint = index;
                            e.SetVelocity(Level.Walkpoints[index]);
                        }
                        else
                            e.Velocity = Vector2.Zero;
                    }

                EntityManager.Update();
            }

            foreach (cBox c in AvailableShooters)
                c.Update();

            if (SelectedShooter.buildID >= 0)
            {
                Rectangle Bounds = Main.GameBound;
                float Radius = Data.ShooterData[SelectedShooter.buildID].CollisionRadius;
                Bounds.Inflate(-(int)Radius, -(int)Radius);
                if (SelectedShooter.allowBuild = Bounds.Contains(Control.MouseCoordinates.ToPoint()))
                    foreach (List<int> s in ActiveShooterDatas)
                        if (!(SelectedShooter.allowBuild = !Maths.Pythagoras(Control.MouseCoordinates, new Vector2(s[1], s[2]), Data.ShooterData[s[0]].CollisionRadius + Radius)))
                            break;
            }
        }

        public override void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (cBox c in AvailableShooters)
                c.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            for (int p = 0; p < Level.Walkpoints.Count - 1; p++)
                new Utils.Line(Level.Walkpoints[p + 1], Level.Walkpoints[p], 2, Color.FloralWhite, .4f).Draw(MediumDepth);

            EntityManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            if (SelectedShooter.buildID >= 0)
            {
                ForegroundDepth.Draw(SelectedShooter.shootArea, Control.MouseCoordinates, null, Color.White, 0, SelectedShooter.shootArea.Center(), 1f, SpriteEffects.None, 0f);
                ForegroundDepth.Draw(Graphic.Shooters[SelectedShooter.buildID], Control.MouseCoordinates, null, (SelectedShooter.allowBuild ? Color.White : Color.Red) * .6f, SelectedShooter.angle, AvailableShooters[SelectedShooter.buildID].GetContent.Center(), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}