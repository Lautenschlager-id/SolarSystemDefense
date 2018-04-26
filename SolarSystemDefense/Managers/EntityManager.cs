using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolarSystemDefense
{
    static class EntityManager
    {
        static bool Updating;

        static List<Entity> Entities = new List<Entity>();
        static List<Entity> NewEntities = new List<Entity>();

        static List<Shooter> Shooters = new List<Shooter>();
        static List<Bullet> Bullets = new List<Bullet>();
        public static List<Enemy> Enemies = new List<Enemy>();

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

        static void CollisionHandler()
        {
            foreach (Enemy e in Enemies)
                foreach (Bullet b in Bullets)
                    if (onCollision(e, b))
                    {
                        e.OnHit(b.Damage);
                        b.Visible = false;
                    }
        }

        public static void Update()
        {
            Updating = true;

            CollisionHandler();

            foreach (Entity e in Entities)
            {
                e.Update();
            }

            Updating = false;

            foreach (Entity e in NewEntities)
                InsertEntity(e);
            NewEntities.Clear();

            Entities = Entities.Where(e => e.Visible).ToList();
            Bullets = Bullets.Where(o => o.Visible).ToList();
            Shooters = Shooters.Where(o => o.Visible).ToList();
            Enemies = Enemies.Where(o => o.Visible).ToList();
        }

        public static void Draw(SpriteBatch BackgroundDepth, SpriteBatch MediumDepth, SpriteBatch ForegroundDepth)
        {
            foreach (Entity e in Entities)
                e.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
        }
    }
}
