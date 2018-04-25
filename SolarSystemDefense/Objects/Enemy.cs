using Microsoft.Xna.Framework;
using System;

namespace SolarSystemDefense
{
    class Enemy : Entity
    {
        public int EnemyType;
        int DamageCooldown;
        int Life;

        public int LastWalkpoint = 0;

        public Enemy(int EnemyType, Vector2 Position)
        {
            this.EnemyType = EnemyType;

            Sprite = Graphic.Enemies[EnemyType];

            Life = Data.EnemyData[EnemyType].Life;

            this.Position = Position;
            Velocity = Vector2.Zero;
            Angle = 0;
        }

        public void OnHit()
        {
            if (--Life <= 0)
                Visible = false;
            else
            {
                DamageCooldown = 10;
                ObjectColor = Color.LightPink * .85f;
            }
        }
        
        public void SetVelocity(Vector2 nextPosition)
        {
            float Speed = Data.EnemyData[EnemyType].Speed;

            Vector2 direction = nextPosition - Position;

            if (direction.LengthSquared() > 0)
            {
                Angle = direction.Angle();

                Velocity = Maths.PolarToVector(Angle, Data.EnemyData[EnemyType].Speed);
            }
        }

        public override void Update()
        {
            if (DamageCooldown > 0)
                if (--DamageCooldown <= 0)
                    ObjectColor = Color.White;

            Position += Velocity;
            Position = Vector2.Clamp(Position, Size / 2, new Vector2(Main.GameBound.Width, Main.GameBound.Height) - Size / 2);
        }
    }
}
