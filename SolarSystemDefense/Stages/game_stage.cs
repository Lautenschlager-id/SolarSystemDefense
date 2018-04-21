using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    class Game_Stage
    {
        enum GameStage
        {
            PlacingShooters,
            Running,
            Paused
        }
        GameStage CurrentStage;

        List<cSquare> ShooterList = new List<cSquare>();
        int Selected_BUILD_Shooter = -1;
        float Selected_ANGLE_Shooter = 0;
        public Game_Stage()
        {
            CurrentStage = GameStage.PlacingShooters;

            cSquare foo;

            foo = new cSquare(620, 100, 180, 300);
            foo.EventOnClick += new EventHandler((obj, arg) =>
            {
                if (Selected_BUILD_Shooter >= 0)
                {
                    Selected_BUILD_Shooter = -1;
                }
            });

            ComponentManager.New(foo);

            cSquare prot_square;
            for (int i = 0; i < 4; i++)
            {
                prot_square = new cSquare(Graphic.Shooters[i], 650 + (i % 2) * 70, 150 + (i / 2) * 70, 50, 50)
                {
                    ComponentColor = Color.Transparent.Collection()
                };
                prot_square.ID = i;

                ShooterList.Add(prot_square);
            }
        }

        public void Update()
        {
            foreach (cSquare c in ShooterList)
            {
                c.Update();
                Console.WriteLine(c.MouseHover);
                if (c.OnClick)
                    Selected_BUILD_Shooter = c.ID;
            }

            if (CurrentStage != GameStage.Paused)
            {
                if (Selected_BUILD_Shooter >= 0)
                    Selected_ANGLE_Shooter += (float)Control.MouseWheel / 5;

                EntityManager.Update();
            }
        }

        public void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (cSquare c in ShooterList)
                c.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            EntityManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);

            if (Selected_BUILD_Shooter >= 0)
                ForegroundDepth.Draw(Graphic.Shooters[Selected_BUILD_Shooter], Control.MouseCoordinates, null, Color.White * .6f, Selected_ANGLE_Shooter, ShooterList[Selected_BUILD_Shooter].ContentSize / 2f , 1f, SpriteEffects.None, 0f);
        }
    }
}
