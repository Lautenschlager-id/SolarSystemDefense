using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class Data
    {
        public class ShooterInfo
        {
            public int Cooldown { get; private set; }
            public float Speed { get; set; }
            public int Radius { get; private set; }
            public int Damage { get; private set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public int defaultCooldown { get; private set; }
            public float defaultSpeed { get; private set; }
            public int defaultRadius { get; private set; }
            public int defaultDamage { get; private set; }

            public ShooterInfo(int Damage, int Cooldown, float Speed, int Radius)
            {
                this.Damage = defaultDamage = Damage;
                this.Cooldown = defaultCooldown = Cooldown;
                this.Speed = defaultSpeed = Speed;
                this.Radius = defaultRadius = Radius;
            }
        }

        public static Dictionary<int, ShooterInfo> ShooterData = new Dictionary<int, ShooterInfo>() {
            { 0, new ShooterInfo(1, 25, 8, 80) },
            { 1, new ShooterInfo(3, 35, 5, 150) },
            { 2, new ShooterInfo(2, 40, 6, 110) },
            { 3, new ShooterInfo(10, 100, 2, 280) },
        };

        public class EnemyInfo
        {
            public float Speed { get; set; }
            public int Life { get; set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public EnemyInfo(float Speed, int Life)
            {
                this.Speed = Speed;
                this.Life = Life;
            }
        }

        public static Dictionary<int, EnemyInfo> EnemyData = new Dictionary<int, EnemyInfo>() {
            { 0, new EnemyInfo(2, 3) },
            { 1, new EnemyInfo(3, 6) },
            { 2, new EnemyInfo(5, 10) },
        };

    }
}   
