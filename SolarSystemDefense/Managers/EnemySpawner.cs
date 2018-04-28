using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    static class EnemySpawner
    {
        static int CurrentQueuePosition = 0;
        static int[] Queue = new int[] { 0, 0, 0, 1, 0, 1, 0, 2, 0, 0, 3 };

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
                stdTotalEnemy4Queue += Graphic.Enemies.Length - Queue[CurrentQueuePosition];
                TotalEnemy4Queue = stdTotalEnemy4Queue;
            }
        }

        static List<Spawn> SpawnData = new List<Spawn>();
        static EnemySpawner()
        {
            SpawnData.Add(new Spawn(ID: 0, TotalEnemy4Queue: 8));
            SpawnData.Add(new Spawn(ID: 1, TotalEnemy4Queue: 20));
            SpawnData.Add(new Spawn(ID: 2, TotalEnemy4Queue: 10));
            SpawnData.Add(new Spawn(ID: 3, TotalEnemy4Queue: 1));
        }

        static Vector2 Position;
        public static void SetInitialPosition(Vector2 p)
        {
            Position = p;
        }

        public static void Update()
        {
            int index = Queue[CurrentQueuePosition];

            Spawn s = SpawnData[index];
            if (SpawnData[index].TotalEnemy4Queue >= 0)
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
                SpawnData[index].RefreshTotalEnemy4Queue();
                if (++CurrentQueuePosition >= Queue.Length)
                {
                    CurrentQueuePosition = 0;
                    Data.Level += .4f;
                }
            }
        }
    }
}
