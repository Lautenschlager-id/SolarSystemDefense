using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Shooter : Entity
    {
        int ShooterType;
        int Timer;
        float Speed;
        int ShootRadius;

        public Shooter(int ShooterType, Vector2 Position, float Angle)
        {
            this.ShooterType = ShooterType;

            Sprite = Graphic.Shooters[ShooterType];

            ShootRadius = Data.ShooterData[ShooterType].ActionArea;
            Speed = Data.ShooterData[ShooterType].Speed;

            Timer = Data.ShooterData[ShooterType].Cooldown;

            this.Position = Position;
            Velocity = Vector2.Zero;

            this.Angle = Angle;
        }

        public void Shoot()
        {
            foreach (Enemy e in EntityManager.Enemies)
                if (Maths.Pythagoras(e.Position, Position, ShootRadius))
                {
                    Timer = Data.ShooterData[ShooterType].defaultCooldown;

                    Vector2 direction = e.Position - Position + e.Velocity * 2;
                    if (direction.LengthSquared() > 0)
                    {
                        Angle = direction.Angle();

                        Vector2 Velocity = Maths.PolarToVector(Angle, Speed);

                        Quaternion EulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, Angle);
                        // The vector is set as Y, X
                        Vector2 ShootDistance = Vector2.Transform(new Vector2(30, 0), EulerAim);

                        EntityManager.New(new Bullet(ShooterType, Position + ShootDistance, Velocity));
                    }
                    return;
                }
        }

        public override void Update()
        {
            if (--Timer <= 0)
                Shoot();
        }
    }
}
