using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Shooter : Entity
    {
        int Timer;
        float Speed;

        public int ActionArea;

        public Shooter(int Type, Vector2 Position, float Angle)
        {
            this.Type = Type;

            Sprite = Graphic.Shooters[Type];

            ActionArea = Data.ShooterData[Type].ActionArea;
            Speed = Data.ShooterData[Type].Speed;

            Timer = Data.ShooterData[Type].Cooldown;

            this.Position = Position;
            Velocity = Vector2.Zero;

            this.Angle = Angle;
        }

        public void Shoot()
        {
            foreach (Enemy e in EntityManager.Enemies)
                if (Maths.Pythagoras(e.Position, Position, ActionArea))
                {
                    Timer = Data.ShooterData[Type].defaultCooldown;

                    Vector2 direction = e.Position - Position;
                    if (direction.LengthSquared() > 0)
                    {
                        Angle = direction.Angle();

                        Vector2 Velocity = Maths.PolarToVector(Angle, Speed);

                        Quaternion EulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, Angle);
                        // The vector is set as Y, X
                        Vector2 ShootDistance = Vector2.Transform(new Vector2(30, 0), EulerAim);

                        EntityManager.New(new Bullet(Type, Position + ShootDistance, Velocity));
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
