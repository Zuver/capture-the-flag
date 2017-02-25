using Engine.Content;
using Engine.GameStateManagement;
using Engine.UI.Buttons;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Screens
{
    public class MainMenuScreen : AbstractScreen
    {
        /// <summary>
        /// Buttons
        /// </summary>
        private TextButton StartButton;
        private TextButton ExitButton;

        /// <summary>
        /// Singleton
        /// </summary>
        private static MainMenuScreen instance;
        public static MainMenuScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainMenuScreen();
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private MainMenuScreen()
            : base()
        {
            // Define start button
            this.StartButton = new TextButton(new Engine.UI.Labels.TextLabel("Start",
                SpriteFontRepository.Instance.Get("test"),
                new Vector2(AppSettingsFacade.WindowWidth / 2, AppSettingsFacade.WindowHeight / 2),
                Engine.UI.Labels.LabelAlignment.Center,
                Color.White),
                Color.Yellow);

            this.StartButton.Clicked += OnStartButtonClicked;

            // Define exit button
            this.ExitButton = new TextButton(new Engine.UI.Labels.TextLabel("Exit",
                SpriteFontRepository.Instance.Get("test"),
                new Vector2(AppSettingsFacade.WindowWidth / 2, AppSettingsFacade.WindowHeight / 2 + (0.1f * AppSettingsFacade.WindowHeight)),
                Engine.UI.Labels.LabelAlignment.Center,
                Color.White),
                Color.Yellow);

            this.ExitButton.Clicked += OnExitButtonClicked;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            this.StartButton.Update();
            this.ExitButton.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            this.StartButton.Draw(spriteBatch);
            this.ExitButton.Draw(spriteBatch);
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