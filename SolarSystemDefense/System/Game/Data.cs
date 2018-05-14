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
                    Price: 20,
                    Damage: 1.5f,
                    Cooldown: 30,
                    ActionArea: 80,
                    Speed: 7.5f
                )
            },

            {
                1, new ShooterInfo(
                    Name: "Venus",
                    Price: 120,
                    Damage: 3.75f,
                    Cooldown: 65,
                    ActionArea: 150,
                    Speed: 5
                )
            },

            {
                2, new ShooterInfo(
                    Name: "Mars",
                    Price: 70,
                    Damage: 2.3f,
                    Cooldown: 50,
                    ActionArea: 110,
                    Speed: 4f,
                    SpeedDamage: 6f
                )
            },

            {
                3, new ShooterInfo(
                    Name: "Jupiter",
                    Price: 300,
                    Damage: 10,
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
                    Life: 5,
                    Score: 10,
                    Cash: 5,
                    Damage: 2.5f,
                    Speed: 1.3f
                )
            },

            {
                1, new EnemyInfo(
                    Life: 10,
                    Score: 60,
                    Cash: 15,
                    Damage: 8,
                    Speed: 3
                )
            },

            {
                2, new EnemyInfo(
                    Life: 20,
                    Score: 120,
                    Cash: 20,
                    Damage: 10,
                    Speed: 5.2f
                )
            },

            {
                3, new EnemyInfo(
                    Life: 300,
                    Score: 500,
                    Cash: 300,
                    Damage: 50,
                    Speed: 1.5f
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
                    Price: 500,
                    Speed: 4,
                    SpeedDamage: 25,
                    Cooldown: 20
                )
            },

            {
                1, new FeatureInfo(
                    Name: "Black Hole",
                    Price: 3500,
                    ActionArea: 200,
                    SpeedDamage: 50
                )
            },
        };
    }
}   
