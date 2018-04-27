using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    class Bullet : Entity
    {
        public float Damage { get; private set; }
        public float SpeedDamage { get; private set; }

        bool CanRotate = true;

        public Bullet(int BulletType, Vector2 Position, Vector2 Velocity)
        {
            Sprite = Graphic.Bullets[BulletType];
            Radius = Sprite.Width / 2f;

            Damage = Data.ShooterData[BulletType].Damage;
            SpeedDamage = Data.ShooterData[BulletType].SpeedDamage;

            this.Position = Position;
            this.Velocity = Velocity;

            Angle = Velocity.Angle();
        }
        public Bullet(Texture2D BulletSprite, Vector2 Position, Vector2 Velocity, float Damage = 0, float SpeedDamage = 0, bool CanRotate = false)
        {
            Sprite = BulletSprite;
            Radius = Sprite.Width / 2f;

            this.Damage = Damage;
            this.SpeedDamage = SpeedDamage;

            this.Position = Position;
            this.Velocity = Velocity;

            this.CanRotate = CanRotate;

            Angle = Velocity.Angle();
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0 && CanRotate)
                Angle += MathHelper.ToRadians(Maths.Random.Next(5, 10));
            Position += Velocity;

            if (!Main.GameBound.Contains(Position.ToPoint()))
                Visible = false;
        }
    }
}
