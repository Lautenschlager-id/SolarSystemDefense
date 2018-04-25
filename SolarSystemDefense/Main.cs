using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SolarSystemDefense
{
    public class Main : Game
    {
        public enum GameState
        {
            StageEditor,
            Playing,
        }
        public static GameState CurrentGameState = GameState.Playing;

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

            Window.Title = "Solar System Defense";
            Instance = this;
        }

        static GameStage CurrentStage;
        public static void SetStage()
        {
            ComponentManager.Clear();
            switch (CurrentGameState)
            {
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

        protected override void Update(GameTime gameTime)
        {
            Control.Update();
            ComponentManager.Update();

            CurrentStage?.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            BackgroundDepth.Begin();
            BackgroundDepth.Draw(Graphic.Background, Vector2.Zero, Color.White);
            BackgroundDepth.End();

            ForegroundDepth.Begin();
            MediumDepth.Begin();
            BackgroundDepth.Begin();
            ComponentManager.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
            CurrentStage?.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
            BackgroundDepth.End();
            MediumDepth.End();
            ForegroundDepth.End();

            ForegroundDepth.Begin();
            ForegroundDepth.Draw(Graphic.MousePointer, Control.MouseCoordinates, Color.White);
            ForegroundDepth.End();

            base.Draw(gameTime);
        }
    }
}
