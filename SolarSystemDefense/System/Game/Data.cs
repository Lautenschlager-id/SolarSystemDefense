using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class Data
    {
        public static float Level = 1;

        public static List<Info.MapData> OfficialMaps = new List<Info.MapData>();

        public struct PlayerData
        {
            public int Score;
            public float Life;
            public int Cash;
        }

        public class ShooterInfo
        {
            public int Cooldown { get; private set; }
            public float Speed { get; set; }
            public int ActionArea { get; private set; }
            public float Damage { get; private set; }
            public float SpeedDamage { get; private set; }
            public string Name { get; private set; }
            public int Price { get; private set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public int defaultCooldown { get; private set; }

            public ShooterInfo(string Name, int Price, float Damage, int Cooldown, int ActionArea, float Speed, float SpeedDamage = 0)
            {
                this.Name = Name;
                this.Price = Price;
                this.Damage = Damage;
                this.SpeedDamage = SpeedDamage;
                this.Cooldown = defaultCooldown = Cooldown;
                this.Speed = Speed;
                this.ActionArea = ActionArea;
            }
        }
        public static Dictionary<int, ShooterInfo> ShooterData = new Dictionary<int, ShooterInfo>() {
            {
                0, new ShooterInfo(
                    Name: "Mercury",
                    Price: 0,
                    Damage: 1000,
                    Cooldown: 30,
                    ActionArea: 80,
                    Speed: 7.5f
                )
            },

            {
                1, new ShooterInfo(
                    Name: "Venus",
                    Price: 0,
                    Damage: 1000,
                    Cooldown: 65,
                    ActionArea: 150,
                    Speed: 5
                )
            },

            {
                2, new ShooterInfo(
                    Name: "Mars",
                    Price: 0,
                    Damage: 1000,
                    Cooldown: 50,
                    ActionArea: 110,
                    Speed: 4f,
                    SpeedDamage: 6f
                )
            },

            {
                3, new ShooterInfo(
                    Name: "Jupiter",
                    Price: 0,
                    Damage: 10000,
                    Cooldown: 100,
                    ActionArea: 250,
                    Speed: 3.3f,
                    SpeedDamage: 10
                )
            },
        };

        public class EnemyInfo
        {
            public float Damage { get; set; }
            public float Speed { get; set; }
            public float Life { get; set; }
            public int Score { get; set; }
            public int Cash { get; set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public EnemyInfo(float Life, int Score, int Cash, float Damage, float Speed)
            {
                this.Damage = Damage;
                this.Speed = Speed;
                this.Life = Life;
                this.Score = Score;
                this.Cash = Cash;
            }
        }
        public static Dictionary<int, EnemyInfo> EnemyData = new Dictionary<int, EnemyInfo>() {
            {
                0, new EnemyInfo(
                    Life: 1,
                    Score: 10000000,
                    Cash: 500000,
                    Damage: 0,
                    Speed: 0
                )
            },

            {
                1, new EnemyInfo(
                    Life: 1,
                    Score: 6000000,
                    Cash: 1000000,
                    Damage: 0,
                    Speed: 0
                )
            },

            {
                2, new EnemyInfo(
                    Life: 1,
                    Score: 120000000,
                    Cash: 20000000,
                    Damage: 0,
                    Speed: 0
                )
            },

            {
                3, new EnemyInfo(
                    Life: 1,
                    Score: 5000000000,
                    Cash: 30000000,
                    Damage: 0,
                    Speed: 0
                )
            },
        };
        
        public class FeatureInfo
        {
            public string Name { get; private set; }
            public int Price { get; private set; }
            public float SpeedDamage { get; private set; }
            public float Speed { get; set; }
            public int Cooldown { get; private set; }
            public int ActionArea { get; private set; }

            public Utils.AssignOnce<float> CollisionRadius = new Utils.AssignOnce<float>();

            public int defaultCooldown { get; private set; }

            public FeatureInfo(string Name, int Price, float SpeedDamage, float Speed = 0, int Cooldown = 0, int ActionArea = 0)
            {
                this.Name = Name;
                this.Price = Price;
                this.SpeedDamage = SpeedDamage;
                this.Speed = Speed;
                this.Cooldown = defaultCooldown = Cooldown;
                this.ActionArea = ActionArea;
            }
        }
        public static Dictionary<int, FeatureInfo> FeatureData = new Dictionary<int, FeatureInfo>() {
            {
                0, new FeatureInfo(
                    Name: "Earth",
                    Price: 0,
                    Speed: 4,
                    SpeedDamage: 2500,
                    Cooldown: 0
                )
            },

            {
                1, new FeatureInfo(
                    Name: "Black Hole",
                    Price: 0,
                    ActionArea: 20000,
                    SpeedDamage: 5000
                )
            },
        };
    }
}   
