using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    public class Main : Game
    {
        public enum GameState
        {
            Presentation,
            Menu,
            StageEditor,
            Playing,
        }
        static GameState CurrentGameWindow = GameState.Presentation;
        public static GameState CurrentGameState
        {
            get
            {
                return CurrentGameWindow;
            }
            set
            {
                CurrentGameWindow = value;
                SetStage();
            }
        }

        static GraphicsDeviceManager graphics;
        SpriteBatch BackgroundDepth, MediumDepth, ForegroundDepth;

        public static Main Instance { get; private set; }
        public static Viewport ViewPort
        {
            get
            {
                return Instance.GraphicsDevice.Viewport;
            }
        }
        public static Vector2 ScreenDimension {
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

        static GameStage CurrentStage;
        static void SetStage()
        {
            ComponentManager.Clear();
            switch (CurrentGameWindow)
            {
                case GameState.Presentation:
                    CurrentStage = new wPresentation();
                    break;
                case GameState.Menu:
                    CurrentStage = new wMenu();
                    break;
                case GameState.Playing:
                    CurrentStage = new wGame();
                    break;
                case GameState.StageEditor:
                    CurrentStage = new wStageEditor();
                    break;
            }
        }

        public static void Resize(int Width, int Height)
        {
            graphics.PreferredBackBufferWidth = (int)MathHelper.Clamp(Width, 50, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            graphics.PreferredBackBufferHeight = (int)MathHelper.Clamp(Height, 50f, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            BackgroundDepth = new SpriteBatch(GraphicsDevice);
            MediumDepth = new SpriteBatch(GraphicsDevice);
            ForegroundDepth = new SpriteBatch(GraphicsDevice);

            Graphic.LoadContent(Content);
            Font.LoadContent(Content);

            SetStage();
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime CurrentTime)
        {
            Control.Update();
            ComponentManager.Update();

            CurrentStage?.Update();

            base.Update(CurrentTime);
        }

        protected override void Draw(GameTime CurrentTime)
        {
            GraphicsDevice.Clear(Color.Black);

            BackgroundDepth.Begin();
            Background.Draw(BackgroundDepth);
            BackgroundDepth.End();

            ForegroundDepth.Begin();
            MediumDepth.Begin();
            BackgroundDepth.Begin();
            ComponentManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
            CurrentStage?.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
            BackgroundDepth.End();
            MediumDepth.End();
            ForegroundDepth.End();

            base.Draw(CurrentTime);
        }
    }
}
