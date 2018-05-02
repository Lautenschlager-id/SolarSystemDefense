using Microsoft.Xna.Framework;

namespace SolarSystemDefense
{
    class Enemy : Entity
    {
        public float Speed, stdSpeed;
        public float Damage;

        int DamageCooldown;
        float Life;

        public int LastWalkpoint = 0;

        public Enemy(int Type, Vector2 Position)
        {
            this.Type = Type;

            Sprite = Graphic.Enemies[Type];
            Radius = Sprite.Width / 2f;

            Speed = stdSpeed = Data.EnemyData[Type].Speed * Data.Level;
            Life = Data.EnemyData[Type].Life;
            Damage = Data.EnemyData[Type].Damage;

            this.Position = Position;
            Velocity = Vector2.Zero;
            Angle = 0;
        }

        public void OnHit(float BulletDamage, float SpeedDamage)
        {
            Life -= BulletDamage;

            if (Life <= 0)
            {
                wGame.Instance.AlignLabel("SCORE", "SCORE : " + (wGame.Instance.Player.Score += Data.EnemyData[Type].Score));
                wGame.Instance.AlignLabel("CASH", "CASH : $" + (wGame.Instance.Player.Cash += Data.EnemyData[Type].Cash));
                Visible = false;

                Sound.Explosion.Play(.5f, 0, 0);
            }
            else
            {
                Speed -= Maths.Percent(SpeedDamage, Speed);
                Velocity = Maths.PolarToVector(Angle, Speed);

                DamageCooldown = 10;
                ObjectColor = Color.LightPink * .85f;

                Sound.Hit.Play(1f, 0, 0);
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
