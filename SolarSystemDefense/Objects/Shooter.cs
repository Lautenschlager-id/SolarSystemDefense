using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Shooter : Entity
    {
        Data.Type ShooterType;
        int Timer;

        public Shooter(Data.Type ShooterType, Vector2 Position)
        {
            this.ShooterType = ShooterType;

            Sprite = Graphic.Shooters[(int)ShooterType];

            Timer = Data.ShooterCooldown[(int)ShooterType];

            this.Position = Position;
            Velocity = Vector2.Zero;

            Angle = 0;
        }

        public void Shoot()
        {
            Timer = Data.ShooterCooldown[(int)ShooterType];

            Vector2 direction = Control.MouseCoordinates - Position;
            if (direction.LengthSquared() > 0)
            {
                float Angle = direction.Angle();

                Vector2 Velocity = Maths.PolarToVector(Angle, 2f);

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
