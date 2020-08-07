using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
	class wGame : GameWindow
	{
		public static wGame Instance { get; private set; }

		public Data.PlayerData Player = new Data.PlayerData()
		{
			Score = 0,
			Life = 100,
			Cash = 150
		};

		public enum RoundStatus
		{
			Running,
			Paused,
			GameOver
		}
		public RoundStatus CurrentStatus = RoundStatus.Paused;

		public Info.MapData Map = new Info.MapData();

		/* ID
         Shooter = 0 - 99
         Feature = 100 +
        */
		struct Selection
		{
			public bool isShooter;
			public int buildID;
			public float angle;
			public Texture2D actionArea;
			public bool allowBuild;
			public int price;
		}
		Selection SelectedObject = new Selection()
		{
			buildID = -1,
			angle = 0,
			allowBuild = false,
			price = 0,
		};

		class MapObject
		{
			public Texture2D texture;
			public Vector2 position { get; private set; }
			public Vector2 origin { get; private set; }
			public float angle { get; private set; }
			public float radius { get; private set; }

			public MapObject(Texture2D texture, int iniIndex, int finalIndex)
			{
				this.texture = texture;
				position = Instance.Map.Walkpoints[iniIndex];
				origin = texture.Center();
				angle = Instance.Map.Walkpoints[iniIndex].Angle(Instance.Map.Walkpoints[finalIndex]) + MathHelper.PiOver2;
				radius = texture.Width / 2f;
			}
		}
		List<MapObject> mapObjects = new List<MapObject>();

		object[] WatchObject;

		List<cBox> AvailableObjects = new List<cBox>();

		cBox ObjectsPanel, InfoPopUp;
		List<Component> PopUpComponents = new List<Component>();

		cLabel PlayerLife, PlayerCash, PlayerScore;
		cLabel item_price; // visibility
		public wGame(object map = null)
		{
			Instance = this;

			Main.Resize(900, 600);

			if (map == null)
				LoadMap();
			else
			{
				Player.Cash = 9999;
				Player.Life = 500;
				LoadMap(map);
			}

			EnemySpawner.SetInitialPosition(Map.Walkpoints[0]);
			EnemySpawner.Reset();
			Data.Level = 1;

			ObjectsPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height)
			{
				ComponentColor = Info.Colors["Container"]
			};
			ObjectsPanel.OnClick += new EventHandler((obj, arg) =>
			{
				if (SelectedObject.buildID >= 0)
					SelectedObject.buildID = -1;
			});
			ObjectsPanel.Alpha = .5f;
			ComponentManager.New(ObjectsPanel);

			Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ObjectsPanel.Size.X, Main.ViewPort.Height);

			cBox ToggleGameState = new cBox(Graphic.GameState, 0, 0)
			{
				Visible = false,
				ComponentColor = Color.Transparent.Collection()
			};
			ToggleGameState.ContentSourceRectangle = new Rectangle(0, 0, 34, 40);
			ToggleGameState.OnClick += new EventHandler((obj, arg) => {
				if (CurrentStatus != RoundStatus.GameOver)
				{
					if (CurrentStatus == RoundStatus.Paused)
						CurrentStatus = RoundStatus.Running;
					else
						CurrentStatus = RoundStatus.Paused;

					int i = CurrentStatus == RoundStatus.Paused ? 1 : 0;
					(obj as cBox).ContentSourceRectangle = new Rectangle(i * 34, 0, 34, 40);
				}
			});
			Vector2 pos = ToggleGameState.GetCoordinates(ObjectsPanel.GetDimension, "left top", 15);
			ToggleGameState.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(ToggleGameState);

			bool Button_FirstExe = true;
			cBox Button = new cBox("Start!", Font.Text, 0, 0, 125, 30, true)
			{
				ComponentColor = Info.Colors["Button"],
				TextColor = Info.Colors["ButtonText"],
				Alpha = .5f
			};
			Button.OnClick += new EventHandler((obj, arg) => {
				if (CurrentStatus != RoundStatus.Paused)
				{
					if (map != null)
						Main.GameStateParameter = Map;
					Main.CurrentGameState = map == null ? Main.GameState.Menu : Main.GameState.MapEditor;
					Button.ClickSound = Sound.Click;
				}
				else
				{
					if (Button_FirstExe)
					{
						Button_FirstExe = false;

						Button.SourceText = map == null ? "Leave" : "Back";
						ToggleGameState.Visible = true;

						pos = Button.GetCoordinates(ObjectsPanel.GetDimension, "right top", 0, 10);
						Button.SetPosition((int)pos.X, (int)pos.Y);

						Button.ClickSound = Sound.Go;
					}
					CurrentStatus = RoundStatus.Running;
				}
			});
			pos = Button.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 10);
			Button.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(Button);

			PlayerScore = new cLabel("SCORE : " + Player.Score, Font.Text, 0, 0)
			{
				ContentColor = Color.GhostWhite.Collection()
			};
			pos = PlayerScore.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, 50);
			PlayerScore.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(PlayerScore);

			PlayerLife = new cLabel("LIFE : " + Player.Life, Font.Text, 0, 0)
			{
				ContentColor = Color.LawnGreen.Collection()
			};
			pos = PlayerLife.GetCoordinates(ObjectsPanel.GetDimension, "xcenter", 0, (int)(pos.Y += 40));
			PlayerLife.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(PlayerLife);

			PlayerCash = new cLabel("CASH : $" + Player.Cash, Font.Text, 0, 0)
			{
				ContentColor = Color.Gold.Collection()
			};
			pos = PlayerCash.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, (int)(pos.Y += 40));
			PlayerCash.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(PlayerCash);

			cLabel Title = new cLabel("Shooters", Font.Title, 0, 0)
			{
				ContentColor = new Color(54, 151, 168).Collection()
			};
			pos = Title.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, (int)(pos.Y += 50));
			Title.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(Title);

			cBox item_square;
			cLabel item_name;
			for (int i = 0; i < Graphic.Shooters.Length; i++)
			{
				item_square = new cBox(Graphic.Shooters[i], (int)ObjectsPanel.GetPosition.X + 30 + (i % 2) * 70, (int)Title.GetPosition.Y + 30 + (i / 2) * 90, 50, 50)
				{
					ID = i
				};
				item_square.OnClick += eventSelectedObject;
				item_square.OnHover += eventHoveredObject;

				AvailableObjects.Add(item_square);

				item_name = new cLabel(Data.ShooterData[i].Name, Font.MenuTitle, 0, 0)
				{
					ContentColor = Color.Yellow.Collection()
				};
				pos = item_name.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(item_square.GetPosition.Y + item_square.ContentRadius.Y + 30));
				item_name.SetPosition((int)pos.X, (int)pos.Y);
				ComponentManager.New(item_name);

				item_price = new cLabel("$" + Data.ShooterData[i].Price, Font.MenuText, 0, 0)
				{
					ContentColor = Color.Red.Collection(),
					ID = item_square.ID
				};
				item_price.OnUpdate += UpdatePriceColor;
				pos = item_price.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(pos.Y + 15));
				item_price.SetPosition((int)pos.X, (int)pos.Y);
				ComponentManager.New(item_price);
			}

			Title = new cLabel("Features", Font.Title, 0, 0)
			{
				ContentColor = new Color(54, 151, 168).Collection()
			};
			pos = Title.GetCoordinates(ObjectsPanel.GetDimension, "xcenter top", 0, (int)item_price.GetPosition.Y + 20);
			Title.SetPosition((int)pos.X, (int)pos.Y);
			ComponentManager.New(Title);

			for (int i = 0; i < Graphic.Features.Length; i++)
			{
				item_square = new cBox(Graphic.Features[i], (int)ObjectsPanel.GetPosition.X + 30 + (i % 2) * 70, (int)Title.GetPosition.Y + 50 + (i / 2) * 90, 50, 50)
				{
					ID = 100 + i
				};
				item_square.OnClick += eventSelectedObject;
				item_square.OnHover += eventHoveredObject;
				item_square.OnUpdate += new EventHandler((obj, arg) => {
					cBox o = (obj as cBox);
					foreach (Feature f in EntityManager.Features)
						if (f.Type == o.ID - 100)
							o.ContentColor = Color.Gray.Collection();
				});

				AvailableObjects.Add(item_square);

				item_name = new cLabel(Data.FeatureData[i].Name, Font.MenuTitle, 0, 0)
				{
					ContentColor = Color.Yellow.Collection()
				};
				pos = item_name.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(item_square.GetPosition.Y + item_square.ContentRadius.Y + 30));
				item_name.SetPosition((int)pos.X, (int)pos.Y);
				ComponentManager.New(item_name);

				item_price = new cLabel("$" + Data.FeatureData[i].Price, Font.MenuText, 0, 0)
				{
					ContentColor = Color.Red.Collection(),
					ID = item_square.ID
				};
				item_price.OnUpdate += UpdatePriceColor;
				item_price.OnUpdate += new EventHandler((obj, arg) => {
					cLabel o = (obj as cLabel);
					foreach (Feature f in EntityManager.Features)
						if (f.Type == o.ID - 100)
							o.Remove = true;
				});
				pos = item_price.GetCoordinates(item_square.GetDimension, "xcenter", 0, (int)(pos.Y + 15));
				item_price.SetPosition((int)pos.X, (int)pos.Y);
				ComponentManager.New(item_price);
			}
		}

		private void LoadMap(object map = null)
		{
			if (map == null)
				Map = Data.OfficialMaps[Maths.Random.Next(0, Data.OfficialMaps.Count)];
			else
				Map = (Info.MapData)map;

			// Earth
			mapObjects.Add(new MapObject(Graphic.Earth[0], Map.Walkpoints.Count - 1, Map.Walkpoints.Count - 2));
			// UFO
			mapObjects.Add(new MapObject(Graphic.UFO, 0, 1));
		}

		List<cBox> Curtains = new List<cBox>();
		List<Component> cGameOver = new List<Component>();
		private void GameOver()
		{
			CurrentStatus = RoundStatus.GameOver;
			Player.Life = 0;

			EntityManager.Clear();
			WatchObject = null;
			SelectedObject.buildID = -1;

			mapObjects[0].texture = Graphic.Earth[1];

			cBox Curtain = new cBox(0, -Main.ViewPort.Bounds.Height / 2, Main.ViewPort.Bounds.Width, Main.ViewPort.Bounds.Height / 2)
			{
				ComponentColor = Color.Black.Collection(),
				Alpha = .6f
			};
			Curtains.Add(Curtain);
			Curtain = new cBox(0, Main.ViewPort.Bounds.Height, Main.ViewPort.Bounds.Width, Main.ViewPort.Bounds.Height / 2)
			{
				ComponentColor = Color.Black.Collection(),
				Alpha = .6f
			};
			Curtains.Add(Curtain);

			cLabel GameOverMessage = new cLabel("You failed. The Earth was destroyed!", Font.BigText, 0, 0);
			Vector2 Position = GameOverMessage.GetCoordinates("xcenter ycenter");
			GameOverMessage.SetPosition((int)Position.X, (int)Position.Y - 80);
			cGameOver.Add(GameOverMessage);

			cBox Button = new cBox("Try again!", Font.Text, 0, 0, 200, 40, true)
			{
				ComponentColor = Info.Colors["Button"],
				TextColor = Info.Colors["ButtonText"],
				Alpha = .5f
			};
			Button.OnClick += new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Playing);
			Position = Button.GetCoordinates("xcenter ycenter", 0);
			Button.SetPosition((int)Position.X, (int)Position.Y - 25);
			cGameOver.Add(Button);

			Button = new cBox("Quit", Font.Text, 0, 0, 200, 40, true)
			{
				ComponentColor = Info.Colors["Button"],
				TextColor = Info.Colors["ButtonText"],
				Alpha = .5f
			};
			Button.OnClick += new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Menu);
			Button.SetPosition((int)Position.X, (int)Position.Y + 25);
			cGameOver.Add(Button);

			Sound.GameOver.Play(.8f, 0, 0);
		}

		private void eventSelectedObject(object obj, EventArgs arg)
		{
			int id = (obj as cBox).ID;

			bool isShooter = id < 100;

			if (!isShooter)
				foreach (Feature f in EntityManager.Features)
					if (f.Type == id - 100)
						return;

			int Price = isShooter ? Data.ShooterData[id].Price : Data.FeatureData[id - 100].Price;
			if (Player.Cash >= Price)
			{
				SelectedObject.isShooter = isShooter;
				SelectedObject.buildID = id - (isShooter ? 0 : 100);
				SelectedObject.price = Price;
				if (isShooter || Data.FeatureData[SelectedObject.buildID].ActionArea > 0)
					SelectedObject.actionArea = Utils.CreateCircle(isShooter ? Data.ShooterData[id].ActionArea : Data.FeatureData[SelectedObject.buildID].ActionArea);
				else
					SelectedObject.actionArea = null;
				SelectedObject.allowBuild = true;

				Sound.Select.Play(.4f, 0, 0);
			}
			else
				Sound.Shift.Play(.4f, 0, 0);
		}
		public void eventHoveredObject(object obj, EventArgs arg)
		{
			CreateInfoPopUp(obj as cBox);
		}

		private void AlignLabel(cLabel label, string Text)
		{
			label.Text = Text;
			Vector2 pos = label.GetCoordinates(ObjectsPanel.GetDimension, "xcenter", 0, (int)label.GetPosition.Y);
			label.SetPosition((int)pos.X, (int)pos.Y);
		}
		public void AlignLabel(string labelName, string Text)
		{
			switch (labelName)
			{
				case "SCORE":
					AlignLabel(PlayerScore, Text);
					break;
				case "LIFE":
					AlignLabel(PlayerLife, Text);
					break;
				case "CASH":
					AlignLabel(PlayerCash, Text);
					break;
			}
		}

		private void UpdatePriceColor(object obj, EventArgs arg)
		{
			cLabel item_price = obj as cLabel;
			int id = item_price.ID;
			int price = id < 100 ? Data.ShooterData[id].Price : Data.FeatureData[id - 100].Price;
			item_price.ContentColor = ((Player.Cash >= price) ? Color.LimeGreen : Color.Red).Collection();
		}

		private void Sell(object obj, EventArgs arg)
		{
			Entity e = ((Entity)WatchObject[2]);

			int ID = e.Type;
			bool isShooter = (obj as cBox).ID == 1;

			e.Visible = false;

			int price;
			if (isShooter)
				price = Data.ShooterData[ID].Price;
			else
				price = Data.FeatureData[ID].Price;

			price = (int)Maths.Percent(60, price);

			AlignLabel("CASH", "CASH : $" + (Player.Cash += price));
		}

		private void CreateInfoPopUp(cBox obj)
		{
			Vector2 Position = Control.MouseCoordinates;
			Vector2 Size = ObjectsPanel.GetPosition + ObjectsPanel.Size;
			Position = Vector2.Clamp(new Vector2(Position.X - 100 / 2f, Position.Y - 110), ObjectsPanel.GetPosition, new Vector2(Size.X - 100, Size.Y - 110));

			InfoPopUp = new cBox("INFO", Font.PopUpTitle, (int)Position.X, (int)Position.Y, 100, 110, false)
			{
				ComponentColor = Info.Colors["InfoBox"],
				TextColor = Color.GreenYellow.Collection(),
				Alpha = .7f
			};
			InfoPopUp.SetLabelPosition("xcenter top");
			Vector2 NewSize = InfoPopUp.Size;

			Component component;

			string Damage = "", Speed = "", Cooldown = "";

			int ID = obj.ID;
			if (ID < 100)
			{
				Data.ShooterInfo ObjData = Data.ShooterData[obj.ID];

				if (ObjData.Damage > 0)
					Damage = ObjData.Damage.ToString();
				if (ObjData.SpeedDamage > 0)
					Damage = (Damage == "" ? "" : Damage + " / ") + "-" + ObjData.SpeedDamage.ToString() + "%";
				if (ObjData.Speed > 0)
					Speed = ObjData.Speed.ToString();
				if (ObjData.Damage > 0)
					Cooldown = ObjData.Cooldown.ToString();
			}
			else
			{
				ID -= 100;
				Data.FeatureInfo ObjData = Data.FeatureData[obj.ID - 100];

				if (ObjData.SpeedDamage > 0)
					Damage = "-" + ObjData.SpeedDamage.ToString() + "%";
				if (ObjData.Speed > 0)
					Speed = ObjData.Speed.ToString();
				if (ObjData.defaultCooldown > 0)
					Cooldown = ObjData.defaultCooldown.ToString();
			}

			// Images
			Position = InfoPopUp.GetCoordinates(InfoPopUp.GetDimension, "left top", 0, 20, 0);

			if (Damage != "")
			{
				component = new cBox(Graphic.InfoPower, 0, 0, 20, 20);
				component.SetPosition((int)Position.X, (int)Position.Y);
				PopUpComponents.Add(component as Component);
			}
			else
				NewSize.Y -= 30;

			if (Speed != "")
			{
				Position = new Vector2(Position.X, Position.Y + 30);
				component = new cBox(Graphic.InfoSpeed, 0, 0, 20, 20);
				component.SetPosition((int)Position.X, (int)Position.Y);
				PopUpComponents.Add(component as Component);
			}
			else
				NewSize.Y -= 30;

			if (Cooldown != "")
			{
				Position = new Vector2(Position.X, Position.Y + 30);
				component = new cBox(Graphic.InfoTimer, 0, 0, 20, 20);
				component.SetPosition((int)Position.X, (int)Position.Y);
				PopUpComponents.Add(component as Component);
			}
			else
				NewSize.Y -= 30;

			// Labels
			Position = InfoPopUp.GetCoordinates(InfoPopUp.GetDimension, "left top", 28, 25);

			if (Damage != "")
			{
				component = new cLabel(Damage, Font.MenuText, (int)Position.X, (int)Position.Y)
				{
					ContentColor = Color.Yellow.Collection()
				};
				PopUpComponents.Add(component as Component);
			}

			if (Speed != "")
			{
				Position = new Vector2(Position.X, Position.Y + 28);
				component = new cLabel(Speed, Font.MenuText, (int)Position.X, (int)Position.Y)
				{
					ContentColor = Color.Yellow.Collection()
				};
				PopUpComponents.Add(component as Component);
			}

			if (Cooldown != "")
			{
				Position = new Vector2(Position.X, Position.Y + 28);
				component = new cLabel(Cooldown, Font.MenuText, (int)Position.X, (int)Position.Y)
				{
					ContentColor = Color.Yellow.Collection()
				};
				PopUpComponents.Add(component as Component);
			}

			InfoPopUp.Size = NewSize;
		}

		private bool OnLineCollision(Vector2 Walkpoint1, Vector2 Walkpoint2, Vector2 PointPos, float PointRadius)
		{
			float angleAB = Walkpoint2.Angle(Walkpoint1);

			Vector2 subCA = PointPos - Walkpoint1, subBA = Walkpoint2 - Walkpoint1;

			float distAC = Vector2.Distance(PointPos, Walkpoint1);
			float distAB = Vector2.Distance(Walkpoint2, Walkpoint1);

			float sin = (float)Math.Sin(angleAB), cos = (float)Math.Cos(angleAB);

			Vector2 d = new Vector2(distAC * cos + Walkpoint1.X, distAC * sin + Walkpoint1.Y);

			float distAD = Vector2.Distance(d, Walkpoint1);
			if (distAC > distAB) return false;

			float distCD = Vector2.Distance(PointPos, d);

			return PointRadius >= distCD;
		}

		public override void Update()
		{
			if (InfoPopUp != null)
			{
				InfoPopUp = null;
				PopUpComponents.Clear();
			}
			if (CurrentStatus != RoundStatus.GameOver)
			{
				// Place new shooter
				if (SelectedObject.buildID >= 0)
				{
					SelectedObject.angle += (float)Control.MouseWheel / 5;
					if (Control.MouseClicked && SelectedObject.allowBuild)
					{
						AlignLabel("CASH", "CASH : $" + (Player.Cash -= SelectedObject.price));

						Vector2 Position = Control.MouseCoordinates;

						if (SelectedObject.isShooter)
							EntityManager.New(new Shooter(SelectedObject.buildID, Position, SelectedObject.angle));
						else
							EntityManager.New(new Feature(SelectedObject.buildID, Position, SelectedObject.angle));

						if (!((Control.KeyHolding(Keys.LeftShift) || Control.KeyHolding(Keys.RightShift)) && Player.Cash >= SelectedObject.price))
							SelectedObject.buildID = -1;
						Sound.Place.Play(.4f, 0, 0);
					}
				}

				if (CurrentStatus == RoundStatus.Running)
				{
					// Update enemy position
					foreach (Enemy e in EntityManager.Enemies.GetRange(0, EntityManager.Enemies.Count))
						if (Math.Floor(Vector2.Distance(e.Position, Map.Walkpoints[e.LastWalkpoint])) <= e.Speed * 2)
						{
							int index = e.LastWalkpoint + 1;
							if (index < Map.Walkpoints.Count)
							{
								e.LastWalkpoint = index;
								e.SetVelocity(Map.Walkpoints[index]);
							}
							else
							{
								bool stop = false;
								if ((Player.Life -= e.Damage) <= 0)
								{
									GameOver();
									stop = true;
								}
								else
									Sound.EarthDamage.Play(.15f, 0, 0);

								AlignLabel("LIFE", "LIFE : " + Math.Ceiling(Player.Life));
								e.Visible = false;

								if (stop)
									return;
							}
						}
				}

				EntityManager.Update();

				if (CurrentStatus == RoundStatus.Running)
					EnemySpawner.Update();

				foreach (cBox c in AvailableObjects)
					c.Update();

				// Gets the WatchObject
				if (WatchObject != null)
					((cBox)WatchObject[0]).Update();
				bool updated = false;
				if (!(updated = !Control.MouseClicked) && SelectedObject.buildID < 0)
				{
					foreach (Entity e in EntityManager.Entities)
					{
						float CollisionRadius;
						if (e is Shooter)
							CollisionRadius = Data.ShooterData[e.Type].CollisionRadius;
						else if (e is Feature)
							CollisionRadius = Data.FeatureData[e.Type].CollisionRadius;
						else
							continue;

						if (Maths.Pythagoras(Control.MouseCoordinates, e.Position, CollisionRadius))
						{
							int ActionArea = 0, isShooter = 1;
							if (e is Shooter)
								ActionArea = (e as Shooter).ActionArea;
							else if (e is Feature)
							{
								isShooter = 0;
								ActionArea = (e as Feature).ActionArea;
							}

							int Type = (e as Entity).Type;

							updated = true;

							cBox SellButton;
							if (isShooter == 1)
							{
								string Text = "SELL ($60%)";
								Vector2 Size = Font.MenuText.MeasureString(Text) + Font.Margin * 2 * Vector2.One;
								Vector2 Position = Vector2.Clamp(e.Position - Size / 2f, Vector2.Zero, new Vector2(Main.GameBound.Width - Size.X, Main.GameBound.Height - Size.Y));
								SellButton = new cBox(Text, Font.MenuText, (int)Position.X, (int)Position.Y + (Position.Y > 100 ? -30 : 30), (int)Size.X, (int)Size.Y, true)
								{
									ID = isShooter,
									ComponentColor = Info.Colors["Button"],
									TextColor = Info.Colors["ButtonText"],
									Alpha = .5f,
									ClickSound = Sound.Shift,
									HoverSound = null
								};
								SellButton.OnClick += Sell;
							}
							else
								SellButton = new cBox(0, 0)
								{
									Visible = false
								};

							WatchObject = new object[]
							{
								SellButton,
								Utils.CreateCircle(ActionArea),
								e
							};
							break;
						}
					}
				}
				if (!updated)
					WatchObject = null;

				// Allows to build a new object
				if (SelectedObject.buildID >= 0)
				{
					Rectangle Bounds = Main.GameBound;

					float Radius;
					if (SelectedObject.isShooter)
						Radius = Data.ShooterData[SelectedObject.buildID].CollisionRadius;
					else
						Radius = Data.FeatureData[SelectedObject.buildID].CollisionRadius;

					Bounds.Inflate(-(int)Radius, -(int)Radius);
					if (SelectedObject.allowBuild = Bounds.Contains(Control.MouseCoordinates.ToPoint()))
					{
						foreach (Entity e in EntityManager.Entities)
						{
							float CollisionRadius;
							if (e is Shooter)
								CollisionRadius = Data.ShooterData[e.Type].CollisionRadius;
							else if (e is Feature)
								CollisionRadius = Data.FeatureData[e.Type].CollisionRadius;
							else
								continue;

							if (!(SelectedObject.allowBuild = !Maths.Pythagoras(Control.MouseCoordinates, e.Position, CollisionRadius + Radius)))
								break;
						}

						if (SelectedObject.allowBuild)
							foreach (MapObject o in mapObjects)
								if (!(SelectedObject.allowBuild = !Maths.Pythagoras(Control.MouseCoordinates, o.position, o.radius + Radius)))
									break;

						if (SelectedObject.allowBuild)
							for (int w = 0; w < Map.Walkpoints.Count - 1; w++)
								if (!(SelectedObject.allowBuild = !OnLineCollision(Map.Walkpoints[w], Map.Walkpoints[w + 1], Control.MouseCoordinates, Radius)))
									break;
					}
				}
			}
			if (Curtains.Count > 0)
			{
				if (Curtains[0].GetPosition.Y < 0)
					Curtains[0].SetPosition(0, (int)Curtains[0].GetPosition.Y + 1);
				if (Curtains[1].GetPosition.Y > Main.ViewPort.Bounds.Height / 2)
					Curtains[1].SetPosition(0, (int)Curtains[1].GetPosition.Y - 1);

				foreach (Component c in cGameOver)
					c.Update();
			}
		}

		public override void Draw(SpriteBatch Layer)
		{
			foreach (cBox c in AvailableObjects)
				c.Draw(Layer);

			for (int p = 0; p < Map.Walkpoints.Count - 1; p++)
				new Utils.Line(Map.Walkpoints[p + 1], Map.Walkpoints[p], 2, Color.FloralWhite, .4f).Draw(Layer);

			EntityManager.Draw(Layer);

			if (SelectedObject.buildID >= 0)
			{
				if (SelectedObject.actionArea != null)
					Layer.Draw(SelectedObject.actionArea, Control.MouseCoordinates, null, Color.LightBlue, 0, SelectedObject.actionArea.Center(), 1f, SpriteEffects.None, 0f);
				Texture2D sprite = (SelectedObject.isShooter ? Graphic.Shooters : Graphic.Features)[SelectedObject.buildID];
				Layer.Draw(sprite, Control.MouseCoordinates, null, (SelectedObject.allowBuild ? Color.White : Color.Red) * .6f, SelectedObject.angle, sprite.Center(), 1f, SpriteEffects.None, 0f);
			}

			foreach (MapObject o in mapObjects)
				Layer.Draw(o.texture, o.position, null, Color.White, o.angle, o.origin, 1f, SpriteEffects.None, 1f);

			if (WatchObject != null)
			{
				Layer.Draw((Texture2D)WatchObject[1], ((Entity)WatchObject[2]).Position, null, Color.LightBlue, 0, ((Texture2D)WatchObject[1]).Center(), 1f, SpriteEffects.None, 0f);
				((cBox)WatchObject[0]).Draw(Layer);
			}
			if (InfoPopUp != null)
			{
				InfoPopUp.Draw(Layer);
				foreach (Component d in PopUpComponents)
					d.Draw(Layer);
			}

			if (Curtains.Count > 0)
			{
				foreach (cBox o in Curtains)
					o.Draw(Layer);

				foreach (Component c in cGameOver)
					c.Draw(Layer);
			}
		}
	}
}