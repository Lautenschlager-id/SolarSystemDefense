using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class Data
    {
        public class ShooterInfo
        {
            public int Cooldown { get; private set; }
            public float Speed { get; set; }
            public int ShootRadius { get; private set; }
            public float Damage { get; private set; }
            public string Name { get; private set; }
            public int Price { get; private set; }


            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public int defaultCooldown { get; private set; }
            public float defaultSpeed { get; private set; }
            public int defaultRadius { get; private set; }

            public ShooterInfo(string Name, int Price, float Damage, int Cooldown, float Speed, int Radius)
            {
                this.Name = Name;
                this.Price = Price;
                this.Damage = Damage;
                this.Cooldown = defaultCooldown = Cooldown;
                this.Speed = defaultSpeed = Speed;
                this.ShootRadius = defaultRadius = Radius;
            }
        }

        public static Dictionary<int, ShooterInfo> ShooterData = new Dictionary<int, ShooterInfo>() {
            { 0, new ShooterInfo("Mercury", 10, 1, 28, 7.5f, 80) },
            { 1, new ShooterInfo("Venus", 40, 3, 40, 5, 150) },
            { 2, new ShooterInfo("Mars", 120, 2.5f, 32, 4.5f, 110) },
            { 3, new ShooterInfo("Jupiter", 300, 10, 100, 3, 280) },
        };

        public class EnemyInfo
        {
            public float Damage { get; set; }
            public float Speed { get; set; }
            public float Life { get; set; }
            public int Score { get; set; }
            public int Cash { get; set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public EnemyInfo(float Damage, float Speed, float Life, int Score, int Cash)
            {
                this.Damage = Damage;
                this.Speed = Speed;
                this.Life = Life;
                this.Score = Score;
                this.Cash = Cash;
            }
        }

        public static Dictionary<int, EnemyInfo> EnemyData = new Dictionary<int, EnemyInfo>() {
            { 0, new EnemyInfo(1, 2, 3, 50, 5) },
            { 1, new EnemyInfo(5, 3, 6, 100, 15) },
            { 2, new EnemyInfo(10, 5, 10, 250, 20) },
        };

        public struct PlayerData
        {
            public int Score;
            public float Life;
            public int Cash;
        }
    }
}   
