using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SolarSystemDefense
{
    public class Main : Game
    {
        public enum GameState
        {
            Presentation,
            Menu,
            Help,
            MapEditor,
            Playing,
        }
        static GameState CurrentGameWindow = GameState.Menu;
        public static GameState CurrentGameState
        {
            get
            {
                return CurrentGameWindow;
            }
            set
            {
                CurrentGameWindow = value;
                EntityManager.Clear();
                ComponentManager.Clear();
                SetGameState();
            }
        }

        static GraphicsDeviceManager graphics;
        SpriteBatch Layer;

        public static object GameStateParameter = null;

        public static Main Instance { get; private set; }
        public static Viewport ViewPort
        {
            get
            {
                return Instance.GraphicsDevice.Viewport;
            }
        }
        public static Vector2 ScreenDimension
        {
            get
            {
                return new Vector2(ViewPort.Width, ViewPort.Height);
            }
        }
        public static Rectangle GameBound { get; set; }

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Solar System Defense";
            Instance = this;
        }

        static GameWindow CurrentWindow;
        static void SetGameState()
        {
            ComponentManager.Clear();
            switch (CurrentGameWindow)
            {
                case GameState.Presentation:
                    CurrentWindow = new wPresentation();
                    break;
                case GameState.Menu:
                    CurrentWindow = new wMenu();
                    break;
                case GameState.Playing:
                    CurrentWindow = new wGame(GameStateParameter);
                    break;
                case GameState.MapEditor:
                    CurrentWindow = new wMapEditor(GameStateParameter);
                    break;
                case GameState.Help:
                    CurrentWindow = new wHelp();
                    break;
            }
            GameStateParameter = null;
        }

        public static void Resize(int Width, int Height)
        {
            graphics.PreferredBackBufferWidth = (int)MathHelper.Clamp(Width, 50, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            graphics.PreferredBackBufferHeight = (int)MathHelper.Clamp(Height, 50f, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            graphics.ApplyChanges();
        }

        static void UpdateMapList()
        {
            Type type = typeof(List<Info.MapData>);

            if (Internet.InternetConnection())
            {
                string JSON = Internet.HTTPGet(@"https://raw.githubusercontent.com/SolarSystemDefense/gamedb/master/OfficialMaps.json");
                Data.OfficialMaps = (List<Info.MapData>)Utils.fromJSON(type, JSON);
            }
            else
                Data.OfficialMaps = (List<Info.MapData>)Utils.fromJSON(type, "[{\"Code\":0,\"Walkpoints\":[{\"X\":388,\"Y\":182},{\"X\":389,\"Y\":327},{\"X\":470,\"Y\":376},{\"X\":545,\"Y\":344},{\"X\":545,\"Y\":123},{\"X\":678,\"Y\":263},{\"X\":677,\"Y\":486},{\"X\":366,\"Y\":573},{\"X\":90,\"Y\":500},{\"X\":89,\"Y\":169},{\"X\":167,\"Y\":298},{\"X\":222,\"Y\":133},{\"X\":143,\"Y\":58},{\"X\":393,\"Y\":60},{\"X\":305,\"Y\":129},{\"X\":275,\"Y\":376}]},{\"Code\":1,\"Walkpoints\":[{\"X\":565,\"Y\":576},{\"X\":563,\"Y\":442},{\"X\":99,\"Y\":443},{\"X\":97,\"Y\":299},{\"X\":549,\"Y\":303},{\"X\":548,\"Y\":175},{\"X\":94,\"Y\":169},{\"X\":94,\"Y\":50}]},{\"Code\":2,\"Walkpoints\":[{\"X\":347,\"Y\":225},{\"X\":347,\"Y\":288},{\"X\":380,\"Y\":313},{\"X\":407,\"Y\":318},{\"X\":432,\"Y\":321},{\"X\":457,\"Y\":322},{\"X\":485,\"Y\":316},{\"X\":508,\"Y\":298},{\"X\":517,\"Y\":280},{\"X\":530,\"Y\":232},{\"X\":513,\"Y\":172},{\"X\":451,\"Y\":144},{\"X\":383,\"Y\":125},{\"X\":324,\"Y\":132},{\"X\":249,\"Y\":170},{\"X\":199,\"Y\":247},{\"X\":194,\"Y\":300},{\"X\":202,\"Y\":360},{\"X\":248,\"Y\":403},{\"X\":347,\"Y\":444},{\"X\":406,\"Y\":455},{\"X\":500,\"Y\":459},{\"X\":558,\"Y\":428},{\"X\":588,\"Y\":376},{\"X\":622,\"Y\":289},{\"X\":613,\"Y\":224},{\"X\":581,\"Y\":121},{\"X\":504,\"Y\":65},{\"X\":401,\"Y\":45},{\"X\":316,\"Y\":35},{\"X\":230,\"Y\":59},{\"X\":169,\"Y\":102},{\"X\":140,\"Y\":156},{\"X\":113,\"Y\":255},{\"X\":95,\"Y\":343},{\"X\":106,\"Y\":412},{\"X\":162,\"Y\":477},{\"X\":248,\"Y\":526},{\"X\":337,\"Y\":548},{\"X\":619,\"Y\":548}]}]");
        }

        protected override void Initialize()
        {
            UpdateMapList();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Layer = new SpriteBatch(GraphicsDevice);

            Font.LoadContent(Content);
            Graphic.LoadContent(Content);
            Sound.LoadContent(Content);

            SetGameState();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime CurrentTime)
        {
            Control.Update();
            ComponentManager.Update();

            CurrentWindow?.Update();

            base.Update(CurrentTime);
        }

        protected override void Draw(GameTime CurrentTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Layer.Begin();
            Background.Draw(Layer);
            ComponentManager.Draw(Layer);
            CurrentWindow?.Draw(Layer);
            Layer.End();

            base.Draw(CurrentTime);
        }
    }
}
