using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Bullet : Entity
    {
        public int Damage { get; private set; }
        
        public Bullet(Data.Type BulletType, Vector2 Position, Vector2 Velocity) : base()
        {
            Sprite = Graphic.Bullets[(int)BulletType];

            Damage = (int)BulletType + 1;

            this.Position = Position;
            this.Velocity = Velocity;

            Angle = Velocity.Angle();
        }

        public override void Update()
        {
            if (Velocity.LengthSquared() > 0)
                Angle += MathHelper.ToRadians(Maths.Random.Next(5, 10));
            Position += Velocity;

            if (!Main.ViewPort.Bounds.Contains(Position.toPoint()))
                Visible = false;
        }
    }
}
