using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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

        List<List<int>> ActiveShooterDatas = new List<List<int>>();

        List<cSquare> AvailableShooters = new List<cSquare>();
        int Selected_BUILD_Shooter = -1;
        float Selected_ANGLE_Shooter = 0;
        Texture2D Selected_RADIUS_Shooter;
        Boolean Selected_ALLOW_Shooter = false;

        cSquare ShooterPanel;
        Rectangle RoundBounds;
        public wGame()
        {
            Main.Resize(900, 600);

            CurrentStage = RoundStage.PlacingShooters;

            ShooterPanel = new cSquare(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height);
            ShooterPanel.OnClick += new EventHandler((obj, arg) =>
            {
                if (Selected_BUILD_Shooter >= 0)
                    Selected_BUILD_Shooter = -1;
            });
            ComponentManager.New(ShooterPanel);

            RoundBounds = new Rectangle(0, 0, Main.ViewPort.Width - (int)ShooterPanel.GetSize.X, Main.ViewPort.Height);

            cSquare prot_square;
            for (int i = 0; i < 4; i++)
            {
                prot_square = new cSquare(Graphic.Shooters[i], (int)ShooterPanel.GetPosition.X + 30 + (i % 2) * 70, 100 + (i / 2) * 70, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection(),
                    ID = i
                };
                prot_square.OnClick += new EventHandler((obj, arg) =>
                {
                    Selected_BUILD_Shooter = (obj as cSquare).ID;
                    Selected_RADIUS_Shooter = Utils.CreateCircle(Data.ShooterData[Selected_BUILD_Shooter].Radius, Color.LightBlue);
                    Selected_ALLOW_Shooter = true;
                });

                AvailableShooters.Add(prot_square);
            }
        }

        public void Update()
        {
            if (CurrentStage != RoundStage.Paused)
            {
                if (Selected_BUILD_Shooter >= 0)
                {
                    Selected_ANGLE_Shooter += (float)Control.MouseWheel / 5;
                    if (Control.MouseClicked && Selected_ALLOW_Shooter)
                    {
                        Vector2 Position = Control.MouseCoordinates;

                        EntityManager.New(new Shooter(Selected_BUILD_Shooter, Position, Selected_ANGLE_Shooter));
                        ActiveShooterDatas.Add(new List<int>() { Selected_BUILD_Shooter, (int)Position.X, (int)Position.Y });

                        Selected_BUILD_Shooter = -1;
                    }
                }

                EntityManager.Update();
            }

            foreach (cSquare c in AvailableShooters)
                c.Update();

            if (Selected_BUILD_Shooter >= 0)
            {
                Rectangle Bounds = RoundBounds;
                float Radius = Data.ShooterData[Selected_BUILD_Shooter].CollisionRadius;
                Bounds.Inflate(-(int)Radius, -(int)Radius);
                if (Selected_ALLOW_Shooter = Bounds.Contains(Control.MouseCoordinates.ToPoint()))
                    foreach (List<int> s in ActiveShooterDatas)
                        if (!(Selected_ALLOW_Shooter = !Maths.Pythagoras(Control.MouseCoordinates, new Vector2(s[1], s[2]), Data.ShooterData[s[0]].CollisionRadius + Radius)))
                            break;
            }
        }

        public void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (cSquare c in AvailableShooters)
                c.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            EntityManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            if (Selected_BUILD_Shooter >= 0)
            {
                ForegroundDepth.Draw(Selected_RADIUS_Shooter, Control.MouseCoordinates, null, Color.White, 0, Selected_RADIUS_Shooter.Center(), 1f, SpriteEffects.None, 0f);
                ForegroundDepth.Draw(Graphic.Shooters[Selected_BUILD_Shooter], Control.MouseCoordinates, null, (Selected_ALLOW_Shooter ? Color.White : Color.Red) * .6f, Selected_ANGLE_Shooter, AvailableShooters[Selected_BUILD_Shooter].GetContent.Center() , 1f, SpriteEffects.None, 0f);
            }
        }
    }
}