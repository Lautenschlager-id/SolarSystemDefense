using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Shooter : Entity
    {
        int ShooterType;
        int Timer;

        public Shooter(int ShooterType, Vector2 Position, float Angle)
        {
            this.ShooterType = ShooterType;

            Sprite = Graphic.Shooters[ShooterType];

            Timer = Data.ShooterData[ShooterType].Cooldown;

            this.Position = Position;
            Velocity = Vector2.Zero;

            this.Angle = Angle;
        }

        public void Shoot()
        {
            Timer = Data.ShooterData[ShooterType].defaultCooldown;

            Vector2 direction = Control.MouseCoordinates - Position;
            if (direction.LengthSquared() > 0)
            {
                float Angle = direction.Angle();

                Vector2 Velocity = Maths.PolarToVector(Angle, Data.ShooterData[ShooterType].Speed);

                Quaternion EulerAim = Quaternion.CreateFromYawPitchRoll(0, 0, Angle);
                // The vector is set as Y, X
                Vector2 ShootDistance = Vector2.Transform(new Vector2(30, 0), EulerAim);

                EntityManager.New(new Bullet(ShooterType, Position + ShootDistance, Velocity));
            }
        }

        public override void Update()
        {
            Angle = Control.MouseCoordinates.Angle(Position);

            if (--Timer <= 0)
                Shoot();
        }
    }
}
