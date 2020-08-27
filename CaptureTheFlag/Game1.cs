using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine.Utilities;
using CaptureTheFlag.Entities.Screens;
using Engine.GameStateManagement;
using Engine.Camera;
using Engine.Content.Fonts;

namespace CaptureTheFlag
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        // The necessary things
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect _basicEffect;
        private KeyboardState _previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Set up event handling
            ScreenController.Instance.Closed += delegate
            {
                Exit();
            };

            // Load application settings
            AppSettingsFacade.Refresh();
            _graphics.PreferredBackBufferWidth = AppSettingsFacade.WindowWidth;
            _graphics.PreferredBackBufferHeight = AppSettingsFacade.WindowHeight;
            _graphics.IsFullScreen = AppSettingsFacade.IsFullScreen;
            _graphics.SynchronizeWithVerticalRetrace = AppSettingsFacade.SynchronizeWithVerticalRetrace;
            _graphics.PreferMultiSampling = true;

            _basicEffect = new BasicEffect(_graphics.GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.CreateOrthographicOffCenter(0,
                    _graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight, 0, 0, 1)
            };

            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load fonts
            SpriteFontRepository.Instance.Initialize(Content);
            SpriteFontRepository.Instance.Add("debug");
            SpriteFontRepository.Instance.Add("test");

            // Initialize main menu
            ScreenController.Instance.Initialize(MainMenuScreen.Instance);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Toggle various debug modes
            if (keyboardState.IsKeyDown(Keys.F1) && !_previousKeyboardState.IsKeyDown(Keys.F1))
            {
                AppSettingsFacade.IsDebugModeOn = !AppSettingsFacade.IsDebugModeOn;
            }

            if (keyboardState.IsKeyDown(Keys.F2) && !_previousKeyboardState.IsKeyDown(Keys.F2))
            {
                AppSettingsFacade.IsPathfindingDebugModeOn = !AppSettingsFacade.IsPathfindingDebugModeOn;
            }

            if (keyboardState.IsKeyDown(Keys.P) && !_previousKeyboardState.IsKeyDown(Keys.P))
            {
                AppSettingsFacade.IsPaused = !AppSettingsFacade.IsPaused;
            }

            if (!AppSettingsFacade.IsPaused)
            {
                // Update logic
                ScreenController.Instance.ActiveScreen.Update();

                // Update global current game time
                TimeHelper.SetCurrentGameTime(gameTime.TotalGameTime.TotalMilliseconds);

                base.Update(gameTime);
            }

            _previousKeyboardState = keyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(10, 10, 15));

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw active screen and its components
            ScreenController.Instance.ActiveScreen.Draw(GraphicsDevice, _basicEffect, _spriteBatch);

            // If debug mode is on, display useful information on the screen
            if (AppSettingsFacade.IsDebugModeOn)
            {
                // Display mouse position
                _spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"),
                    "Mouse position: <" + Mouse.GetState().X + ", " + Mouse.GetState().Y + ">", new Vector2(10, 30),
                    Color.Yellow);

                // Display camera position
                Vector2 cameraPosition = Camera2D.Instance.GetPosition();
                _spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"),
                    "Camera position: <" + cameraPosition.X + ", " + cameraPosition.Y + ">", new Vector2(10, 50),
                    Color.Yellow);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
