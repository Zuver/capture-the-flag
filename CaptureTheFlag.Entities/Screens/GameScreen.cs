using CaptureTheFlag.Entities.Maps;
using Engine.Camera;
using Engine.Entities;
using Engine.GameStateManagement;
using Engine.Physics.Bodies.Collisions;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private static GameScreen _instance;

        public static GameScreen Instance => _instance ?? (_instance = new GameScreen());

        /// <summary>
        /// Constructor
        /// </summary>
        private GameScreen()
        {
            Map = new MyMap(new Vector2(AppSettingsFacade.WindowWidth, AppSettingsFacade.WindowHeight));
            Camera2D.Instance.Initialize(Vector2.Zero, 2.0f, 1.0f);
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

            Map.Update();

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
            Map.Draw(graphicsDevice, basicEffect, spriteBatch);
        }
    }
}
