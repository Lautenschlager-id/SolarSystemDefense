using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class Data
    {
        public static float Level = 1;

        public static List<string> OfficialStages = new List<string>()
        {
            "{\"Walkpoints\":[{\"X\":565,\"Y\":576},{\"X\":563,\"Y\":442},{\"X\":99,\"Y\":443},{\"X\":97,\"Y\":299},{\"X\":549,\"Y\":303},{\"X\":548,\"Y\":175},{\"X\":94,\"Y\":169},{\"X\":94,\"Y\":50}]}",
            "{\"Walkpoints\":[{\"X\":643,\"Y\":63},{\"X\":303,\"Y\":561},{\"X\":51,\"Y\":111},{\"X\":362,\"Y\":297},{\"X\":363,\"Y\":97},{\"X\":213,\"Y\":96}]}",
            "{\"Walkpoints\":[{\"X\":31,\"Y\":533},{\"X\":28,\"Y\":27},{\"X\":679,\"Y\":526},{\"X\":685,\"Y\":44},{\"X\":28,\"Y\":571},{\"X\":368,\"Y\":569},{\"X\":369,\"Y\":437}]}",
            "{\"Walkpoints\":[{\"X\":352,\"Y\":186},{\"X\":352,\"Y\":332},{\"X\":479,\"Y\":404},{\"X\":601,\"Y\":356},{\"X\":602,\"Y\":104},{\"X\":405,\"Y\":49},{\"X\":162,\"Y\":156},{\"X\":160,\"Y\":397},{\"X\":322,\"Y\":493},{\"X\":558,\"Y\":496},{\"X\":613,\"Y\":461}]}",
            "{\"Walkpoints\":[{\"X\":388,\"Y\":182},{\"X\":389,\"Y\":327},{\"X\":470,\"Y\":376},{\"X\":545,\"Y\":344},{\"X\":545,\"Y\":123},{\"X\":678,\"Y\":263},{\"X\":677,\"Y\":486},{\"X\":366,\"Y\":573},{\"X\":90,\"Y\":500},{\"X\":89,\"Y\":169},{\"X\":167,\"Y\":298},{\"X\":222,\"Y\":133},{\"X\":143,\"Y\":58},{\"X\":393,\"Y\":60},{\"X\":305,\"Y\":129},{\"X\":275,\"Y\":376}]}",
            "{\"Walkpoints\":[{\"X\":347,\"Y\":225},{\"X\":347,\"Y\":288},{\"X\":380,\"Y\":313},{\"X\":407,\"Y\":318},{\"X\":432,\"Y\":321},{\"X\":457,\"Y\":322},{\"X\":485,\"Y\":316},{\"X\":508,\"Y\":298},{\"X\":517,\"Y\":280},{\"X\":530,\"Y\":232},{\"X\":513,\"Y\":172},{\"X\":451,\"Y\":144},{\"X\":383,\"Y\":125},{\"X\":324,\"Y\":132},{\"X\":249,\"Y\":170},{\"X\":199,\"Y\":247},{\"X\":194,\"Y\":300},{\"X\":202,\"Y\":360},{\"X\":248,\"Y\":403},{\"X\":347,\"Y\":444},{\"X\":406,\"Y\":455},{\"X\":500,\"Y\":459},{\"X\":558,\"Y\":428},{\"X\":588,\"Y\":376},{\"X\":622,\"Y\":289},{\"X\":613,\"Y\":224},{\"X\":581,\"Y\":121},{\"X\":504,\"Y\":65},{\"X\":401,\"Y\":45},{\"X\":316,\"Y\":35},{\"X\":230,\"Y\":59},{\"X\":169,\"Y\":102},{\"X\":140,\"Y\":156},{\"X\":113,\"Y\":255},{\"X\":95,\"Y\":343},{\"X\":106,\"Y\":412},{\"X\":162,\"Y\":477},{\"X\":248,\"Y\":526},{\"X\":337,\"Y\":548},{\"X\":619,\"Y\":548}]}",
        };

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
