#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine.Content;
using Engine.Utilities;
using CaptureTheFlag.Entities.Screens;
using Engine.GameStateManagement;
using Engine.Camera;
#endregion

namespace CaptureTheFlag
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        // The necessary things
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private BasicEffect basicEffect;
        private KeyboardState PreviousKeyboardState;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here

            // Set up event handling
            ScreenController.Instance.Closed += delegate(object sender, EventArgs e)
            {
                this.Exit();
            };

            // Load application settings
            AppSettingsFacade.Refresh();
            this.graphics.PreferredBackBufferWidth = AppSettingsFacade.WindowWidth;
            this.graphics.PreferredBackBufferHeight = AppSettingsFacade.WindowHeight;
            this.graphics.IsFullScreen = AppSettingsFacade.IsFullScreen;
            this.graphics.SynchronizeWithVerticalRetrace = AppSettingsFacade.SynchronizeWithVerticalRetrace;
            this.graphics.PreferMultiSampling = true;

            this.basicEffect = new BasicEffect(graphics.GraphicsDevice);
            this.basicEffect.VertexColorEnabled = true;
            this.basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0,
                this.graphics.PreferredBackBufferWidth,
                this.graphics.PreferredBackBufferHeight, 0, 0, 1);

            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load game content here

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
            if (keyboardState.IsKeyDown(Keys.F1) && !this.PreviousKeyboardState.IsKeyDown(Keys.F1))
            {
                AppSettingsFacade.IsDebugModeOn = !AppSettingsFacade.IsDebugModeOn;
            }

            if (keyboardState.IsKeyDown(Keys.F2) && !this.PreviousKeyboardState.IsKeyDown(Keys.F2))
            {
                AppSettingsFacade.IsPathfindingDebugModeOn = !AppSettingsFacade.IsPathfindingDebugModeOn;
            }

            // Update logic
            ScreenController.Instance.ActiveScreen.Update();

            // Update global current game time
            Engine.Utilities.TimeHelper.SetCurrentGameTime(gameTime.TotalGameTime.TotalMilliseconds);

            base.Update(gameTime);

            this.PreviousKeyboardState = keyboardState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(10, 10, 15));

            // TODO: Add your drawing code here

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw active screen and its components
            ScreenController.Instance.ActiveScreen.Draw(GraphicsDevice, this.basicEffect, spriteBatch);

            spriteBatch.DrawString(SpriteFontRepository.Instance.Get("test"), "Press F1 to toggle debug mode", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(SpriteFontRepository.Instance.Get("test"), "Press F2 to toggle pathfinding debug mode", new Vector2(10, 40), Color.White);

            // If debug mode is on, display useful information on the screen
            if (AppSettingsFacade.IsDebugModeOn)
            {
                // Display mouse position
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"), "Mouse position: <" + Mouse.GetState().X + ", " + Mouse.GetState().Y + ">", new Vector2(10, 30), Color.Yellow);

                // Display camera position
                Vector2 cameraPosition = Camera2D.Instance.GetPosition();
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"), "Camera position: <" + cameraPosition.X + ", " + cameraPosition.Y + ">", new Vector2(10, 50), Color.Yellow);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}