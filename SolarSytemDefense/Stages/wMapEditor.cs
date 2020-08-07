using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SolarSystemDefense
{
	class wMapEditor : GameWindow
	{
		Info.MapData Map = new Info.MapData();

		bool DrawMap = true;
		int initialCooldown = 10;
		int exportCooldown = 100;

		List<cBox> Buttons = new List<cBox>();
		cBox ItemPanel;
		cLabel totalLines, mapCode;
		public wMapEditor(object map = null)
		{
			Main.Resize(900, 600);

			initialCooldown = 10;

			if (map != null)
				Map = (Info.MapData)map;

			ItemPanel = new cBox(Main.ViewPort.Width - 180, 0, 180, Main.ViewPort.Height)
			{
				ComponentColor = Info.Colors["Container"],
				Alpha = .5f
			};
			ComponentManager.New(ItemPanel);

			totalLines = new cLabel("", Font.Text, 0, 0)
			{
				ContentColor = Color.Yellow.Collection()
			};
			AlignLineCounter();
			ComponentManager.New(totalLines);

			mapCode = new cLabel("@0000", Font.PopUpTitle, 0, 0)
			{
				ContentColor = Color.GreenYellow.Collection()
			};
			Vector2 Position = mapCode.GetCoordinates(ItemPanel.GetDimension, "xcenter top", 0, 0);
			mapCode.SetPosition((int)Position.X, (int)totalLines.GetPosition.Y + 25);
			ComponentManager.New(mapCode);

			List<List<object>> ButtonTexts = new List<List<object>>() {
				new List<object> { "Play map", new EventHandler((obj, arg) => {
					Main.GameStateParameter = Map;
					Main.CurrentGameState = Main.GameState.Playing;
				}) },
				new List<object> { "Export code", new EventHandler((obj, arg) => {
					if (exportCooldown <= 0 && Internet.InternetConnection())
						if (Map.Walkpoints.Count > 1)
						{
							object[] ExportedMaps = GetExportedMaps();
							List<Info.MapData> Maps = (List<Info.MapData>)ExportedMaps[0];

							if (!Maps.Any(l => l.Walkpoints.SequenceEqual(Map.Walkpoints)))
							{
								exportCooldown = 1000;

								Regex regex = new Regex("\"sha\":\"(.+?)\",");
								Match match = regex.Match((string)ExportedMaps[1]);

								Map.Code = Math.Max(999, Maps.Count > 0 ? Maps.Max(o => o.Code) : 0) + 1;
								Maps.Add(Map);
								mapCode.Text = "@" + Map.Code.ToString();

                                // Map in Clipboard
                                System.Windows.Forms.Clipboard.SetText(Encoding.UTF8.GetString(Utils.toJSON(Map.GetType(), Map)));

								string content = Convert.ToBase64String(Utils.toJSON(Maps.GetType(), Maps));

								Info.GitHubJSONPut gJSON = new Info.GitHubJSONPut()
								{
									message = "Last update in " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " by " + Environment.MachineName.ToString() + " - @" + Map.Code,
									sha = match.Groups[1].Value,
									content = content
								};
								content = Encoding.UTF8.GetString(Utils.toJSON(gJSON.GetType(), gJSON));

								Internet.HTTPPut(@"https://api.github.com/repos/SolarSystemDefense/gamedb/contents/ExportedMaps.json", content, Info.headers);
							}
						}
				}) },
				new List<object> { "Reset building", new EventHandler((obj, arg) => {
					initialCooldown = 10;

					Map.Walkpoints.Clear();
					AlignLineCounter();
					mapCode.Text = "@0000";

					foreach (cBox btn in Buttons)
						btn.Visible = (DrawMap && Map.Walkpoints.Count > 1);
					DrawMap = true;
				})},
				new List<object> { "Load map", new EventHandler((obj, arg) => {
					string code = System.Windows.Forms.Clipboard.GetText();

					if (Regex.IsMatch(code, @"^@\d{4}$"))
						if (Internet.InternetConnection())
						{
							object[] ExportedMaps = GetExportedMaps();
							List<Info.MapData> Maps = (List<Info.MapData>)ExportedMaps[0];

							Info.MapData search = Maps.Find(l => l.Code.ToString() == code.Substring(1));

							if (search != null)
							{
								Map = search;
								AlignLineCounter();
								mapCode.Text = code;
							}
						}
					else if (code.Contains("Walkpoints"))
					{
						Map = (Info.MapData)Utils.fromJSON(typeof(Info.MapData), code);
						AlignLineCounter();
					}
				}) },
				new List<object> { "Exit", new EventHandler((obj, arg) => Main.CurrentGameState = Main.GameState.Menu) },
			};

			cBox Button;
			Position = ItemPanel.GetCoordinates(ItemPanel.GetDimension, "xcenter", 0, 20);
			foreach (List<object> buttonInfo in ButtonTexts)
			{
				Button = new cBox((string)buttonInfo[0], Font.Text, 0, 0, 180, 40, true)
				{
					Visible = false,
					ComponentColor = Info.Colors["Button"],
					TextColor = Info.Colors["ButtonText"],
					Alpha = .5f
				};
				Position.Y += 50;
				Button.SetPosition((int)Position.X, (int)Position.Y);
				Button.OnClick += (EventHandler)buttonInfo[1];
				Buttons.Add(Button);
				ComponentManager.New(Button);
			}

			cLabel tutorial = new cLabel(
				"Click in the map besides\n" +
				"this container to define\n" +
				"the enemies route.\n\n" +
				"The initial line is\n" +
				"highlighted in green and\n" +
				"the final line will be red!\n\n" +
				"Press delete to remove\n" +
				"the last walkpoint or\n" +
				"press space to finalize.\n\n" +
				"Click in \"Export code\" to\n" +
				"publish your map or in\n" +
				"\"Load code\" to load\n" +
				"someone's map.", Font.SmallText, 0, 0)
			{
				ContentColor = new Color(146, 91, 255).Collection()
			};
			Position = tutorial.GetCoordinates(ItemPanel.GetDimension, "xcenter bottom", 0, 0);
			tutorial.SetPosition((int)Position.X, (int)Position.Y);
			ComponentManager.New(tutorial);

			Main.GameBound = new Rectangle(0, 0, Main.ViewPort.Width - (int)ItemPanel.Size.X, Main.ViewPort.Height);
		}

		private object[] GetExportedMaps()
		{
			string ExportedMaps = Internet.HTTPGet(@"https://api.github.com/repos/SolarSystemDefense/gamedb/contents/ExportedMaps.json").ToString();

			Regex regex = new Regex("\"content\":\"(.+?)\",");
			Match match = regex.Match(ExportedMaps);

			string currentContent = Encoding.UTF8.GetString(Convert.FromBase64String(match.Groups[1].Value.Replace("\\n", "\n")));

			// List, Content
			return new object[] { Utils.fromJSON(typeof(List<Info.MapData>), currentContent), ExportedMaps };
		}

		public void AlignLineCounter()
		{
			int index = (Map.Walkpoints.Count - 1);
			totalLines.Text = (index < 0 ? 0 : index) + " / 50";
			Vector2 pos = totalLines.GetCoordinates(ItemPanel.GetDimension, "xcenter top", 0, 10);
			totalLines.SetPosition((int)pos.X, (int)pos.Y);
		}

		public override void Update()
		{
			if (exportCooldown > 0)
				exportCooldown--;

			if (Control.KeyDown(Keys.Space))
			{
				if (Map.Walkpoints.Count > 1 || !DrawMap)
				{
					foreach (cBox btn in Buttons)
						btn.Visible = (DrawMap && Map.Walkpoints.Count > 1);
					DrawMap = !DrawMap;
				}
			}
			else if (DrawMap)
			{
				bool alter = false;
				if (alter = Control.KeyDown(Keys.Delete))
				{
					int index = Map.Walkpoints.Count - 1;
					if (index >= 0)
						Map.Walkpoints.RemoveAt(index);
					Sound.Shift.Play(.2f, 0, 0);
				}
				else if (alter = (--initialCooldown <= 0 && Control.MouseClicked && Map.Walkpoints.Count < 51))
				{
					Map.Walkpoints.Add(Vector2.Clamp(Control.MouseCoordinates, Vector2.Zero, new Vector2(Main.GameBound.Width, Main.GameBound.Height)));
					Sound.Place.Play(.2f, 0, 0);
				}
				if (alter)
					AlignLineCounter();
			}
		}

		public override void Draw(SpriteBatch Layer)
		{
			for (int p = 0; p < Map.Walkpoints.Count; p++)
			{
				Vector2 p1, p2;

				if (p == Map.Walkpoints.Count - 1)
				{
					p1 = Map.Walkpoints[p];
					p2 = DrawMap ? Vector2.Clamp(Control.MouseCoordinates, Vector2.Zero, new Vector2(Main.GameBound.Width, Main.GameBound.Height)) : p1;
				}
				else
				{
					p1 = Map.Walkpoints[p + 1];
					p2 = Map.Walkpoints[p];
				}

				new Utils.Line(p1, p2, 2, p == 0 ? Color.LimeGreen : p == Map.Walkpoints.Count - 2 ? Color.DarkRed : Color.Yellow).Draw(Layer);
			}
		}
	}
}