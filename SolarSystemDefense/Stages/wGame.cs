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
            Cash = 1110
        };

        enum RoundStage
        {
            PlacingShooters,
            Running,
            Paused
        }
        RoundStage CurrentStage;

        public Info.StageTable StageMap = new Info.StageTable();

        /* ID
         Shooter = 0 - 99
         Feature = 100 +
        */
        struct Selection
        {
            public bool isShooter;
            public int buildID;
            public float angle;
            public Texture2D actionArea;
            public bool allowBuild;
            public int price;
        }
        Selection SelectedObject = new Selection()
        {
            buildID = -1,
            angle = 0,
            allowBuild = false,
            price = 0,
        };

        object[] WatchObject;

        List<cBox> AvailableObjects = new List<cBox>();

        cBox ObjectsPanel;
        cLabel Title, PlayerLife, PlayerCash, PlayerScore;
        cLabel item_price; // visibility
        public wGame()
        {
            Instance = this;

            Main.Resize(900, 600);

            CurrentStage = RoundStage.PlacingShooters;

            LoadStage();
            EnemySpawner.SetInitialPosition(StageMap.Walkpoints[0]);

            ObjectsPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height)
            {
                ComponentColor = Info.Colors["Container"]
            };
            ObjectsPanel.OnClick += new EventHandler((obj, arg) =>
            {
                WatchObject = null;
                if (SelectedObject.buildID >= 0)
                    SelectedObject.buildID = -1;
            });
            ObjectsPanel.Alpha = .5f;
            ComponentManager.New(ObjectsPanel);

            Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ObjectsPanel.GetSize.X, Main.ViewPort.Height);

            PlayerScore = new cLabel("SCORE : " + Player.Score, Font.Text, 0, 0)
            {
                ContentColor = Color.GhostWhite.Collection()
            };
            Vector2 pos = PlayerScore.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 20);
            PlayerScore.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerScore);

            PlayerLife = new cLabel("LIFE : " + Player.Life, Font.Text, 0, 0)
            {
                ContentColor = Color.LawnGreen.Collection()
            };
            pos = PlayerLife.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 60);
            PlayerLife.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerLife);

            PlayerCash = new cLabel("CASH : $" + Player.Cash, Font.Text, 0, 0)
            {
                ContentColor = Color.Gold.Collection()
            };
            pos = PlayerCash.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 80);
            PlayerCash.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(PlayerCash);

            Title = new cLabel("Shooters", Font.Title, 0, 0)
            {
                ContentColor = new Color(54, 151, 168).Collection()
            };
            pos = Title.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 130);
            Title.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(Title);

            cBox item_square;
            cLabel item_name;
            for (int i = 0; i < Graphic.Shooters.Length; i++)
            {
                item_square = new cBox(Graphic.Shooters[i], (int)ObjectsPanel.GetPosition.X + 30 + (i % 2) * 70, (int)Title.GetPosition.Y + 30 + (i / 2) * 90, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection(),
                    ID = i
                };
                item_square.OnClick += eventSelectedObject;

                AvailableObjects.Add(item_square);

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
                    ID = item_square.ID
                };
                item_price.OnUpdate += UpdatePriceColor;
                pos = item_price.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(pos.Y + 15));
                item_price.SetPosition((int)pos.X, (int)pos.Y);
                ComponentManager.New(item_price);
            }

            Title = new cLabel("Features", Font.Title, 0, 0)
            {
                ContentColor = new Color(54, 151, 168).Collection()
            };
            pos = Title.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, (int)item_price.GetPosition.Y + 20);
            Title.SetPosition((int)pos.X, (int)pos.Y);
            ComponentManager.New(Title);

            for (int i = 0; i < Graphic.Features.Length; i++)
            {
                item_square = new cBox(Graphic.Features[i], (int)ObjectsPanel.GetPosition.X + 30 + (i % 2) * 70, (int)Title.GetPosition.Y + 50 + (i / 2) * 90, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection(),
                    ID = 100 + i
                };
                item_square.OnClick += eventSelectedObject;

                AvailableObjects.Add(item_square);

                item_name = new cLabel(Data.FeatureData[i].Name, Font.MenuTitle, 0, 0)
                {
                    ContentColor = Color.Yellow.Collection()
                };
                pos = item_name.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(item_square.GetPosition.Y + item_square.ContentRadius.Y + 30));
                item_name.SetPosition((int)pos.X, (int)pos.Y);
                ComponentManager.New(item_name);

                item_price = new cLabel("$" + Data.FeatureData[i].Price, Font.MenuText, 0, 0)
                {
                    ContentColor = Color.Red.Collection(),
                    ID = item_square.ID
                };
                item_price.OnUpdate += UpdatePriceColor;
                pos = item_price.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(pos.Y + 15));
                item_price.SetPosition((int)pos.X, (int)pos.Y);
                ComponentManager.New(item_price);
            }
        }

        private void LoadStage()
        {

            string LoadedStage = Data.OfficialStages[Maths.Random.Next(0, Data.OfficialStages.Count - 1)];

            MemoryStream memory = new MemoryStream(Encoding.UTF8.GetBytes(LoadedStage));

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(StageMap.GetType());
            StageMap = serializer.ReadObject(memory) as Info.StageTable;

            memory.Close();
        }

        private void eventSelectedObject(object obj, EventArgs arg)
        {
            int id = (obj as cBox).ID;

            bool isShooter = id < 100;

            if (!isShooter)
                foreach (Feature f in EntityManager.Features)
                    if (f.Type == id - 100)
                        return;

            int Price = isShooter ? Data.ShooterData[id].Price : Data.FeatureData[id - 100].Price;
            if (Player.Cash >= Price)
            {
                SelectedObject.isShooter = isShooter;
                SelectedObject.buildID = id - (isShooter ? 0 : 100);
                SelectedObject.price = Price;
                if (isShooter || Data.FeatureData[SelectedObject.buildID].ActionArea > 0)
                    SelectedObject.actionArea = Utils.CreateCircle(isShooter ? Data.ShooterData[id].ActionArea : Data.FeatureData[SelectedObject.buildID].ActionArea);
                else
                    SelectedObject.actionArea = null;
                SelectedObject.allowBuild = true;
            }
        }

        private void AlignLabel(cLabel label, string Text)
        {
            label.Text = Text;
            Vector2 pos = label.GetCoordinates(ObjectsPanel.GetDimension, "xcenter", 0, (int)label.GetPosition.Y);
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

        private void UpdatePriceColor(object sender, EventArgs e)
        {
            cLabel item_price = sender as cLabel;
            int id = item_price.ID;
            int price = id < 100 ? Data.ShooterData[id].Price : Data.FeatureData[id - 100].Price;
            item_price.ContentColor = ((Player.Cash >= price) ? Color.LimeGreen : Color.Red).Collection();
        }

        public override void Update()
        {
            if (CurrentStage != RoundStage.Paused)
            {
                // Place new shooter
                if (SelectedObject.buildID >= 0)
                {
                    SelectedObject.angle += (float)Control.MouseWheel / 5;
                    if (Control.MouseClicked && SelectedObject.allowBuild)
                    {
                        AlignLabel("CASH", "CASH : $" + (Player.Cash -= SelectedObject.price));

                        Vector2 Position = Control.MouseCoordinates;

                        if (SelectedObject.isShooter)
                            EntityManager.New(new Shooter(SelectedObject.buildID, Position, SelectedObject.angle));
                        else
                            EntityManager.New(new Feature(SelectedObject.buildID, Position, SelectedObject.angle));

                        SelectedObject.buildID = -1;
                    }
                }

                // Update enemy position
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

            foreach (cBox c in AvailableObjects)
                c.Update();

            // Gets the WatchObject
            bool updated = false;
            if (!(updated = !Control.MouseClicked) && SelectedObject.buildID < 0)
            {
                foreach (Entity e in EntityManager.Entities)
                {
                    float CollisionRadius;
                    if (e is Shooter)
                        CollisionRadius = Data.ShooterData[e.Type].CollisionRadius;
                    else if (e is Feature)
                        CollisionRadius = Data.FeatureData[e.Type].CollisionRadius;
                    else
                        continue;

                    if (Maths.Pythagoras(Control.MouseCoordinates, e.Position, CollisionRadius))
                    {
                        int ActionArea = 0;
                        if (e is Shooter)
                            ActionArea = (e as Shooter).ActionArea;
                        else if (e is Feature)
                            ActionArea = (e as Feature).ActionArea;

                        updated = true;
                        WatchObject = new object[]
                        {
                            Utils.CreateCircle(ActionArea),
                            e.Position
                        };
                        break;
                    }
                }
            }
            if (!updated)
                WatchObject = null;

            // Allows to build a new object
            if (SelectedObject.buildID >= 0)
            {
                Rectangle Bounds = Main.GameBound;
                float Radius = Data.ShooterData[SelectedObject.buildID].CollisionRadius;
                Bounds.Inflate(-(int)Radius, -(int)Radius);
                if (SelectedObject.allowBuild = Bounds.Contains(Control.MouseCoordinates.ToPoint()))
                    foreach (Entity e in EntityManager.Entities)
                    {
                        float CollisionRadius;
                        if (e is Shooter)
                            CollisionRadius = Data.ShooterData[e.Type].CollisionRadius;
                        else if (e is Feature)
                            CollisionRadius = Data.FeatureData[e.Type].CollisionRadius;
                        else
                            continue;

                        if (!(SelectedObject.allowBuild = !Maths.Pythagoras(Control.MouseCoordinates, e.Position, CollisionRadius + Radius)))
                            break;
                    }
            }
        }

        public override void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (cBox c in AvailableObjects)
                c.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            for (int p = 0; p < StageMap.Walkpoints.Count - 1; p++)
                new Utils.Line(StageMap.Walkpoints[p + 1], StageMap.Walkpoints[p], 2, Color.FloralWhite, .4f).Draw(MediumDepth);

            EntityManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            if (SelectedObject.buildID >= 0)
            {
                if (SelectedObject.actionArea != null)
                    ForegroundDepth.Draw(SelectedObject.actionArea, Control.MouseCoordinates, null, Color.LightBlue, 0, SelectedObject.actionArea.Center(), 1f, SpriteEffects.None, 0f);
                Texture2D sprite = (SelectedObject.isShooter ? Graphic.Shooters : Graphic.Features)[SelectedObject.buildID];
                ForegroundDepth.Draw(sprite, Control.MouseCoordinates, null, (SelectedObject.allowBuild ? Color.White : Color.Red) * .6f, SelectedObject.angle, sprite.Center(), 1f, SpriteEffects.None, 0f);
            }

            int LastWalkpoint = StageMap.Walkpoints.Count - 1;
            ForegroundDepth.Draw(Graphic.Earth, StageMap.Walkpoints[LastWalkpoint], null, Color.White, StageMap.Walkpoints[LastWalkpoint].Angle(StageMap.Walkpoints[LastWalkpoint - 1]) + MathHelper.PiOver2, Graphic.Earth.Center(), 1f, SpriteEffects.None, 1f);
            ForegroundDepth.Draw(Graphic.UFO, StageMap.Walkpoints[0], null, Color.White, StageMap.Walkpoints[0].Angle(StageMap.Walkpoints[1]) + MathHelper.PiOver2, Graphic.UFO.Center(), 1f, SpriteEffects.None, 1f);

            if (WatchObject != null)
            {
                ForegroundDepth.Draw((Texture2D)WatchObject[0], (Vector2)WatchObject[1], null, Color.LightBlue, 0, ((Texture2D)WatchObject[0]).Center(), 1f, SpriteEffects.None, 0f);

            }
        }
    }
}