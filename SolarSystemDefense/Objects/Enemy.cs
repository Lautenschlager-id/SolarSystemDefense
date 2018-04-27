using Microsoft.Xna.Framework;
using System;

namespace SolarSystemDefense
{
    class Enemy : Entity
    {
        public float Speed, stdSpeed;
        public float Damage;

        public int EnemyType;
        int DamageCooldown;
        float Life;

        public int LastWalkpoint = 0;

        public Enemy(int EnemyType, Vector2 Position)
        {
            this.EnemyType = EnemyType;

            Sprite = Graphic.Enemies[EnemyType];
            Radius = Sprite.Width / 2f;

            Speed = stdSpeed = Data.EnemyData[EnemyType].Speed * Data.Level;
            Life = Data.EnemyData[EnemyType].Life;
            Damage = Data.EnemyData[EnemyType].Damage;

            this.Position = Position;
            Velocity = Vector2.Zero;
            Angle = 0;
        }

        public void OnHit(float BulletDamage, float SpeedDamage)
        {
            Life -= BulletDamage;

            if (Life <= 0)
            {
                wGame.Instance.AlignLabel("SCORE", "SCORE : " + (wGame.Instance.Player.Score += Data.EnemyData[EnemyType].Score));
                wGame.Instance.AlignLabel("CASH", "CASH : $" + (wGame.Instance.Player.Cash += Data.EnemyData[EnemyType].Cash));
                Visible = false;
            }
            else
            {
                Speed -= Maths.Percent(SpeedDamage, Speed);
                Velocity = Maths.PolarToVector(Angle, Speed);

                DamageCooldown = 10;
                ObjectColor = Color.LightPink * .85f;
            }
        }
        
        public void SetVelocity(Vector2 nextPosition)
        {
            Vector2 direction = nextPosition - Position;

            if (direction.LengthSquared() > 0)
            {
                Angle = direction.Angle();

                Velocity = Maths.PolarToVector(Angle, Speed);
            }
        }

        public override void Update()
        {
            if (DamageCooldown > 0)
                if (--DamageCooldown <= 0)
                    ObjectColor = Color.White;

            Speed = stdSpeed;

            Position += Velocity;
        }
    }
}
