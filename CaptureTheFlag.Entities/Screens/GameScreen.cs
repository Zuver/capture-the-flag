using CaptureTheFlag.Entities.Maps;
using Engine.Camera;
using Engine.Content;
using Engine.Drawing;
using Engine.Entities;
using Engine.GameStateManagement;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Engine.UI.Buttons;
using Engine.UI.Labels;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CaptureTheFlag.Entities.Screens
{
    public class GameScreen : AbstractScreen
    {
        /// <summary>
        /// Map
        /// </summary>
        public AbstractMap Map;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static GameScreen instance;
        public static GameScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameScreen();
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private GameScreen()
            : base()
        {
            this.Map = new MyMap(new Vector2(AppSettingsFacade.WindowWidth, AppSettingsFacade.WindowHeight));
            Camera2D.Instance.Initialize(Vector2.Zero, 20.0f, 3.0f);
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Did the user press the escape button?
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenController.Instance.NavigateBack();
            }

            this.Map.Update();

            Camera2D.Instance.Update();
            CollisionPool.Instance.Update();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            base.Draw(graphicsDevice, basicEffect, spriteBatch);
            this.Map.Draw(graphicsDevice, basicEffect, spriteBatch);
        }
    }
}