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
        public static wGame Instance { get; private set; }

        public Data.PlayerData Player = new Data.PlayerData()
        {
            Score = 0,
            Life = 100,
            Cash = 810
        };

        enum RoundStage
        {
            PlacingShooters,
            Running,
            Paused
        }
        RoundStage CurrentStage;

        Info.StageTable StageMap = new Info.StageTable();

        List<List<int>> ActiveShooterDatas = new List<List<int>>();

        struct Selection
        {
            public int buildID;
            public float angle;
            public Texture2D shootArea;
            public bool allowBuild;
            public int price;
        }
        Selection SelectedShooter = new Selection()
        {
            buildID = -1,
            angle = 0,
            allowBuild = false,
            price = 0,
        };

        List<cBox> AvailableShooters = new List<cBox>();

        cBox ShooterPanel;
        cLabel ShooterTitle, PlayerLife, PlayerCash, PlayerScore;
        public wGame()
        {
            Instance = this;

            Main.Resize(900, 600);

            CurrentStage = RoundStage.PlacingShooters;

            LoadStage();
            EnemySpawner.SetInitialPosition(StageMap.Walkpoints[0]);

            ShooterPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height)
            {
                ComponentColor = Info.Colors["Container"]
            };
            ShooterPanel.OnClick += new EventHandler((obj, arg) =>
            {
                if (SelectedShooter.buildID >= 0)
                    SelectedShooter.buildID = -1;
            });
            ShooterPanel.Alpha = .5f;
            ComponentManager.New(ShooterPanel);

            Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ShooterPanel.GetSize.X, Main.ViewPort.Height);

            PlayerScore = new cLabel("SCORE : " + Player.Score, Font.Text, 0, 0)
            {
                ContentColor = Color.GhostWhite.Collection()
            };
            Vector2 pos = PlayerScore.GetCoordinates(ShooterPanel.GetDimension, "xcenter top", 0, 20);
            PlayerScore.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerScore);

            PlayerLife = new cLabel("LIFE : " + Player.Life, Font.Text, 0, 0)
            {
                ContentColor = Color.LawnGreen.Collection()
            };
            pos = PlayerLife.GetCoordinates(ShooterPanel.GetDimension, "xcenter top", 0, 60);
            PlayerLife.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerLife);

            PlayerCash = new cLabel("CASH : $" + Player.Cash, Font.Text, 0, 0)
            {
                ContentColor = Color.Gold.Collection()
            };
            pos = PlayerCash.GetCoordinates(ShooterPanel.GetDimension, "xcenter top", 0, 80);
            PlayerCash.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerCash);

            ShooterTitle = new cLabel("Shooters", Font.Title, 0, 0)
            {
                ContentColor = new Color(54, 151, 168).Collection()
            };
            pos = ShooterTitle.GetCoordinates(ShooterPanel.GetDimension, "xcenter top", 0, 120);
            ShooterTitle.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(ShooterTitle);

            cBox item_square;
            cLabel item_name, item_price;
            for (int i = 0; i < 4; i++)
            {
                item_square = new cBox(Graphic.Shooters[i], (int)ShooterPanel.GetPosition.X + 30 + (i % 2) * 70, 180 + (i / 2) * 90, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection(),
                    ID = i
                };
                item_square.OnClick += new EventHandler((obj, arg) =>
                {
                    int id = (obj as cBox).ID;
                    int price = Data.ShooterData[id].Price;

                    if (Player.Cash >= price)
                    {
                        SelectedShooter.buildID = id;
                        SelectedShooter.price = price;
                        SelectedShooter.shootArea = Utils.CreateCircle(Data.ShooterData[SelectedShooter.buildID].ShootRadius);
                        SelectedShooter.allowBuild = true;
                    }
                });

                AvailableShooters.Add(item_square);

                item_name = new cLabel(Data.ShooterData[i].Name, Font.MenuTitle, 0, 0)
                {
                    ContentColor = Color.Yellow.Collection()
                };
                pos = item_name.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(item_square.GetPosition.Y + item_square.ContentRadius.Y + 30));
                item_name.SetPosition((int)pos.X, (int)pos.Y);
                ComponentManager.New(item_name);

                item_price = new cLabel("$" + Data.ShooterData[i].Price, Font.MenuText, 0, 0)
                {
                    ContentColor = Color.Red.Collection(),
                    ID = i
                };
                item_price.OnUpdate += UpdatePriceColor;
                pos = item_price.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(pos.Y + 15));
                item_price.SetPosition((int)pos.X, (int)pos.Y);
                ComponentManager.New(item_price);
            }
        }

        private void AlignLabel(cLabel label, string Text)
        {
            label.Text = Text;
            Vector2 pos = label.GetCoordinates(ShooterPanel.GetDimension, "xcenter", 0, (int)label.GetPosition.Y);
            label.SetPosition((int)pos.X, (int)pos.Y);
        }
        public void AlignLabel(string labelName, string Text)
        {
            switch (labelName)
            {
                case "SCORE":
                    AlignLabel(PlayerScore, Text);
                    break;
                case "LIFE":
                    AlignLabel(PlayerLife, Text);
                    break;
                case "CASH":
                    AlignLabel(PlayerCash, Text);
                    break;
            }
        }

        private void LoadStage()
        {
            string LoadedStage = "{\"Walkpoints\":[{\"X\":263,\"Y\":114},{\"X\":347,\"Y\":226},{\"X\":353,\"Y\":382},{\"X\":538,\"Y\":362},{\"X\":468,\"Y\":124},{\"X\":596,\"Y\":167},{\"X\":682,\"Y\":417},{\"X\":276,\"Y\":597},{\"X\":58,\"Y\":435},{\"X\":143,\"Y\":183},{\"X\":196,\"Y\":217},{\"X\":233,\"Y\":342},{\"X\":239,\"Y\":138},{\"X\":109,\"Y\":37},{\"X\":416,\"Y\":53},{\"X\":415,\"Y\":186}]}";

            MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(LoadedStage));

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(StageMap.GetType());
            StageMap = serializer.ReadObject(memory) as Info.StageTable;

            memory.Close();
        }

        private void UpdatePriceColor(object sender, EventArgs e)
        {
            cLabel price = sender as cLabel;
            price.ContentColor = ((Player.Cash >= Data.ShooterData[price.ID].Price) ? Color.LimeGreen : Color.Red).Collection();
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
                        AlignLabel("CASH", "CASH : $" + (Player.Cash -= SelectedShooter.price));

                        Vector2 Position = Control.MouseCoordinates;

                        EntityManager.New(new Shooter(SelectedShooter.buildID, Position, SelectedShooter.angle));
                        ActiveShooterDatas.Add(new List<int>() { SelectedShooter.buildID, (int)Position.X, (int)Position.Y });

                        SelectedShooter.buildID = -1;
                    }
                }

                foreach (Enemy e in EntityManager.Enemies.GetRange(0, EntityManager.Enemies.Count))
                    if (Math.Floor(Vector2.Distance(e.Position, StageMap.Walkpoints[e.LastWalkpoint])) <= e.Speed * 2)
                    {
                        int index = e.LastWalkpoint + 1;
                        if (index < StageMap.Walkpoints.Count)
                        {
                            e.LastWalkpoint = index;
                            e.SetVelocity(StageMap.Walkpoints[index]);
                        }
                        else
                        {
                            AlignLabel("LIFE", "LIFE : " + Math.Ceiling(Player.Life -= e.Damage));
                            e.Visible = false;
                        }
                    }

                EntityManager.Update();
                EnemySpawner.Update();
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

            for (int p = 0; p < StageMap.Walkpoints.Count - 1; p++)
                new Utils.Line(StageMap.Walkpoints[p + 1], StageMap.Walkpoints[p], 2, Color.FloralWhite, .4f).Draw(MediumDepth);

            EntityManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            if (SelectedShooter.buildID >= 0)
            {
                ForegroundDepth.Draw(SelectedShooter.shootArea, Control.MouseCoordinates, null, Color.LightBlue, 0, SelectedShooter.shootArea.Center(), 1f, SpriteEffects.None, 0f);
                ForegroundDepth.Draw(Graphic.Shooters[SelectedShooter.buildID], Control.MouseCoordinates, null, (SelectedShooter.allowBuild ? Color.White : Color.Red) * .6f, SelectedShooter.angle, AvailableShooters[SelectedShooter.buildID].GetContent.Center(), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}