using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SolarSystemDefense
{
    public class Main : Game
    {
        public enum GameState
        {
            Running,
        }
        static GameState hCurrentGameState = GameState.Running;
        public static GameState CurrentGameState
        {
            get
            {
                return CurrentGameState;
            }
        }

        wGame PLAY_GAME;

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

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Title = "Solar System Defense";
            Instance = this;
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

            PLAY_GAME = new wGame();
        }

        protected override void UnloadContent(){}

        protected override void Update(GameTime gameTime)
        {
            Control.Update();
            ComponentManager.Update();

            switch (hCurrentGameState)
            {
                case GameState.Running:
                    PLAY_GAME.Update();
                    break;
            }

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
            switch (hCurrentGameState)
            {
                case GameState.Running:
                    PLAY_GAME.Draw(BackgroundDepth, MediumDepth, ForegroundDepth);
                    break;
            }            
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
