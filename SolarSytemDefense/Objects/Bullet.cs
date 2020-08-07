using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
	class Bullet : Entity
	{
		public float Damage { get; private set; }
		public float SpeedDamage { get; private set; }

		bool CanRotate = true;

		public Bullet(int Type, Vector2 Position, Vector2 Velocity)
		{
			Sprite = Graphic.Bullets[this.Type = Type];
			Radius = Sprite.Width / 2f;

			Damage = Data.ShooterData[Type].Damage;
			SpeedDamage = Data.ShooterData[Type].SpeedDamage;

			this.Position = Position;
			this.Velocity = Velocity;

			Angle = Velocity.Angle();

			Sound.Shoot.Play(.15f, 0, 0);
		}
		public Bullet(Texture2D BulletSprite, Vector2 Position, Vector2 Velocity, float Damage = 0, float SpeedDamage = 0, bool CanRotate = false)
		{
			Sprite = BulletSprite;
			Radius = Sprite.Width / 2f;

			this.Damage = Damage;
			this.SpeedDamage = SpeedDamage;

			this.Position = Position;
			this.Velocity = Velocity;

			this.CanRotate = CanRotate;

			Angle = Velocity.Angle();
		}

		public override void Update()
		{
			if (Velocity.LengthSquared() > 0 && CanRotate)
				Angle += Velocity.Angle() / 300;
			Position += Velocity;

			if (!Main.GameBound.Contains(Position.ToPoint()))
				Visible = false;
		}
	}
}
