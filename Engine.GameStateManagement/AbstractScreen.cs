using Engine.Content;
using Engine.UI.Labels;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.GameStateManagement
{
    public abstract class AbstractScreen
    {
        /// <summary>
        /// Text label queue
        /// </summary>
        private readonly TextLabelQueue _textLabelQueue;

        /// <summary>
        /// Enqueue message
        /// </summary>
        /// <param name="message"></param>
        public void EnqueueMessage(string message)
        {
            _textLabelQueue.Enqueue(message);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AbstractScreen()
        {
            _textLabelQueue = new TextLabelQueue(SpriteFontRepository.Instance.Get("test"),
                new Vector2(10f, AppSettingsFacade.WindowHeight - 30),
                LabelAlignment.TopLeft,
                Color.White,
                2500);
        }

        /// <summary>
        /// Update
        /// </summary>
        public virtual void Update()
        {
            _textLabelQueue.Update();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            _textLabelQueue.Draw(spriteBatch);
        }
    }
}