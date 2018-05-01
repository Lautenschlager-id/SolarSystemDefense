using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace SolarSystemDefense
{
    static class EntityManager
    {
        static bool Updating;

        public static List<Entity> Entities = new List<Entity>();
        static List<Entity> NewEntities = new List<Entity>();

        static List<Shooter> Shooters = new List<Shooter>();
        static List<Bullet> Bullets = new List<Bullet>();
        public static List<Enemy> Enemies = new List<Enemy>();
        public static List<Feature> Features = new List<Feature>();

        public static void Clear()
        {
            Entities.Clear();
            NewEntities.Clear();
            Shooters.Clear();
            Bullets.Clear();
            Enemies.Clear();
            Features.Clear();
        }

        public static int Count
        {
            get
            {
                return Entities.Count;
            }
        }

        private static void InsertEntity(Entity e)
        {
            Entities.Add(e);
            if (e is Bullet)
                Bullets.Add(e as Bullet);
            else if (e is Shooter)
                Shooters.Add(e as Shooter);
            else if (e is Enemy)
                Enemies.Add(e as Enemy);
            else if (e is Feature)
                Features.Add(e as Feature);
        }

        public static void New(Entity e)
        {
            if (Updating)
                NewEntities.Add(e);
            else
                InsertEntity(e);
        }

        static bool onCollision(Entity obj1, Entity obj2)
        {
            float radius = obj1.Radius + obj2.Radius;
            return obj1.Visible && obj2.Visible && Maths.Pythagoras(obj1.Position, obj2.Position, radius);
        }

        static void Push(Entity e1, Entity e2)
        {
            Vector2 d = e2.Position - e1.Position;
            e1.Velocity += 5 * d / (d.LengthSquared() + 1);
        }

        static void CollisionHandler()
        {
            // Enemies x Bullet
            foreach (Enemy e in Enemies)
                foreach (Bullet b in Bullets)
                    if (onCollision(e, b))
                    {
                        e.OnHit(b.Damage, b.SpeedDamage);
                        b.Visible = false;
                    }

            // Bullet x Bullet
            for (int i = 0; i < Bullets.Count; i++)
                for (int j = i + 1; j < Bullets.Count; j++)
                    if (onCollision(Bullets[i], Bullets[j]))
                    {
                        Push(Bullets[i], Bullets[j]);
                        Push(Bullets[j], Bullets[i]);
                    }
        }

        public static void Update()
        {
            if (wGame.Instance.CurrentStage == wGame.RoundStage.Running)
            {
                Updating = true;

                CollisionHandler();

                foreach (Entity e in Entities)
                    e.Update();

                Updating = false;
            }

            foreach (Entity e in NewEntities)
                InsertEntity(e);
            NewEntities.Clear();

            Entities = Entities.Where(e => e.Visible).ToList();
            Bullets = Bullets.Where(o => o.Visible).ToList();
            Shooters = Shooters.Where(o => o.Visible).ToList();
            Enemies = Enemies.Where(o => o.Visible).ToList();
            Features = Features.Where(o => o.Visible).ToList();
        }

        public static void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (Entity e in Entities)
                e.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
        }
    }
}
