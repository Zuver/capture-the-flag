using Engine.Content;
using Engine.GameStateManagement;
using Engine.UI.Buttons;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CaptureTheFlag.Entities.Screens
{
    public class MainMenuScreen : AbstractScreen
    {
        /// <summary>
        /// Start button
        /// </summary>
        private readonly TextButton _startButton;

        /// <summary>
        /// Exit button
        /// </summary>
        private readonly TextButton _exitButton;

        /// <summary>
        /// Singleton
        /// </summary>
        private static MainMenuScreen _instance;

        public static MainMenuScreen Instance => _instance ?? (_instance = new MainMenuScreen());

        /// <summary>
        /// Constructor
        /// </summary>
        private MainMenuScreen()
        {
            // Define start button
            _startButton = new TextButton(new Engine.UI.Labels.TextLabel("Start",
                    SpriteFontRepository.Instance.Get("test"),
                    new Vector2((float) AppSettingsFacade.WindowWidth / 2, (float) AppSettingsFacade.WindowHeight / 2),
                    Engine.UI.Labels.LabelAlignment.Center,
                    Color.White),
                Color.Yellow);

            _startButton.Clicked += OnStartButtonClicked;

            // Define exit button
            _exitButton = new TextButton(new Engine.UI.Labels.TextLabel("Exit",
                    SpriteFontRepository.Instance.Get("test"),
                    new Vector2((float) AppSettingsFacade.WindowWidth / 2,
                        (float) AppSettingsFacade.WindowHeight / 2 + (0.1f * AppSettingsFacade.WindowHeight)),
                    Engine.UI.Labels.LabelAlignment.Center,
                    Color.White),
                Color.Yellow);

            _exitButton.Clicked += OnExitButtonClicked;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            _startButton.Update();
            _exitButton.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            _startButton.Draw(spriteBatch);
            _exitButton.Draw(spriteBatch);
        }

        /// <summary>
        /// Handles the event that the exit button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExitButtonClicked(object sender, EventArgs e)
        {
            ScreenController.Instance.Close();
        }

        /// <summary>
        /// Handles the event that the start button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartButtonClicked(object sender, EventArgs e)
        {
            ScreenController.Instance.NavigateTo(GameScreen.Instance);
        }
    }
}
