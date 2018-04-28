using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    class Feature : Entity
    {
        int Timer;
        float Speed;
        float TimeSinceSpawn = 0;

        public int ActionArea;

        static List<EventHandler> eventUpdate4Each = new List<EventHandler>();
        static Feature()
        {
            // Earth
            eventUpdate4Each.Add(new EventHandler((obj, arg) =>
            {
                Feature f = obj as Feature;

                if (--f.Timer <= 0)
                {
                    int index = wGame.Instance.StageMap.Walkpoints.Count - 1;
                    foreach (Enemy e in EntityManager.Enemies)
                        if (e.LastWalkpoint >= index)
                        {
                            f.Timer = Data.FeatureData[f.Type].defaultCooldown;

                            Vector2 direction = wGame.Instance.StageMap.Walkpoints[index - 1] - wGame.Instance.StageMap.Walkpoints[index];

                            float Angle = direction.Angle();
                            Vector2 Velocity = Maths.PolarToVector(Angle, f.Speed);

                            Quaternion EulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, Angle);
                            Vector2 ShootDistance = Vector2.Transform(new Vector2(30, 0), EulerAim);

                            EntityManager.New(new Bullet(Graphic.Water, wGame.Instance.StageMap.Walkpoints[index] + ShootDistance, Velocity, 0, 50));
                            return;
                        }
                }
            }));

            // Black Hole
            eventUpdate4Each.Add(new EventHandler((obj, arg) =>
            {
                Feature f = obj as Feature;

                f.Angle += .008f;
                f.Scale = Utils.ScaleBounce(f.TimeSinceSpawn += .005f, 10);

                foreach (Enemy e in EntityManager.Enemies)
                {
                    if (Maths.Pythagoras(e.Position, f.Position, f.ActionArea))
                        e.Speed -= Maths.Percent(Data.FeatureData[f.Type].SpeedDamage, e.Speed);
                    e.Velocity = Maths.PolarToVector(e.Angle, e.Speed);
                }
            }));
        }

        public Feature(int Type, Vector2 Position, float Angle)
        {
            this.Type = Type;

            Sprite = Graphic.Features[Type];

            if (Data.FeatureData[Type].Cooldown > 0)
            {
                Timer = Data.FeatureData[Type].Cooldown;
                Speed = Data.FeatureData[Type].Speed;
            }
            if (Data.FeatureData[Type].ActionArea > 0)
                ActionArea = Data.FeatureData[Type].ActionArea;

            this.Position = Position;
            Velocity = Vector2.Zero;

            this.Angle = Angle;
        }

        public override void Update()
        {
            eventUpdate4Each[Type]?.Invoke(this, null);
        }
    }
}
