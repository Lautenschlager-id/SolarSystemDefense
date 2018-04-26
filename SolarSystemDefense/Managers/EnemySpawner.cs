using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class EnemySpawner
    {
        static float CurrentQueue = 0;

        class Spawn
        {
            public int ID { get; private set; }
            public float TotalEnemy4Queue;

            public int stdSpawnCooldown = 8;
            public float stdTotalEnemy4Queue;

            public int SpawnCooldown;
            public int StandardCooldown
            {
                get
                {
                    return stdSpawnCooldown;
                }
            }
            public float StandardTotalEnemy4Queue
            {
                get
                {
                    return stdTotalEnemy4Queue;
                }
            }

            public Spawn(int ID, float TotalEnemy4Queue)
            {
                SpawnCooldown = stdSpawnCooldown;
                this.ID = ID;
                this.TotalEnemy4Queue = stdTotalEnemy4Queue = TotalEnemy4Queue;
            }

            public void RefreshTotalEnemy4Queue()
            {
                stdTotalEnemy4Queue = stdTotalEnemy4Queue * 1.75f;
                TotalEnemy4Queue = stdTotalEnemy4Queue;
            }
        }

        static List<Spawn> Data = new List<Spawn>();
        static EnemySpawner()
        {
            Data.Add(new Spawn(0, 8));
            Data.Add(new Spawn(1, 20));
            Data.Add(new Spawn(2, 10));
        }

        static Vector2 Position;
        public static void SetInitialPosition(Vector2 p)
        {
            Position = p;
        }

        public static void Update()
        {
            int index = ((int)CurrentQueue % Data.Count);

            Spawn s = Data[index];
            if (Data[index].TotalEnemy4Queue >= 0)
            {
                if (--s.SpawnCooldown <= 0)
                {
                    s.SpawnCooldown = s.StandardCooldown;
                    if (--s.TotalEnemy4Queue >= 0)
                        EntityManager.New(new Enemy(s.ID, Position));
                }
            }
            else if (EntityManager.Enemies.Count == 0)
            {
                Data[index].RefreshTotalEnemy4Queue();
                CurrentQueue++;
            }
        }
    }
}
