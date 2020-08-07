using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SolarSystemDefense
{
	class wHelp : GameWindow
	{
		int _image = 0;
		int Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (value < 0)
					Main.CurrentGameState = Main.GameState.Menu;
				else
				{
					if (value >= Graphic.Help.Length - 1)
					{
						value = Graphic.Help.Length - 1;
						RightButton.ComponentColor = Info.Colors["GhostButton"];
						RightButton.TextColor = Info.Colors["GhostButtonText"];
					}
					else
					{
						RightButton.ComponentColor = Info.Colors["Button"];
						RightButton.TextColor = Info.Colors["ButtonText"];
					}

					_image = value;
				}
			}
		}

		Vector2 ImagePosition;

		cBox LeftButton, RightButton;
		public wHelp()
		{
			Main.Resize(450, 600);

			ImagePosition = new Vector2(110, Main.ScreenDimension.Y - Graphic.Help[0].Height) / 2f;

			LeftButton = new cBox("«", Font.Text, 0, 0, 50, Main.ViewPort.Height, true)
			{
				ComponentColor = Info.Colors["Button"],
				TextColor = Info.Colors["ButtonText"],
				Alpha = .5f
			};
			LeftButton.OnClick += new EventHandler((obj, arg) => Image--);
			ComponentManager.New(LeftButton);

			RightButton = new cBox("»", Font.Text, (int)Main.ScreenDimension.X - 50, 0, 50, Main.ViewPort.Height, true)
			{
				ComponentColor = Info.Colors["Button"],
				TextColor = Info.Colors["ButtonText"],
				Alpha = .5f
			};
			RightButton.OnClick += new EventHandler((obj, arg) => Image++);
			ComponentManager.New(RightButton);
		}

		public override void Draw(SpriteBatch Layer)
		{
			Layer.Draw(Graphic.Help[Image], ImagePosition, Color.White);
		}
	}
}
