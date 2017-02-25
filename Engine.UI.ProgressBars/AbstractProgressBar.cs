using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.ProgressBars
{
    public abstract class AbstractProgressBar
    {
        /// <summary>
        /// Primitive used to draw the progress bar
        /// </summary>
        protected AbstractPrimitive Primitive;

        /// <summary>
        /// Progress
        /// </summary>
        private float _progress;

        /// <summary>
        /// Set progress
        /// </summary>
        /// <param name="progress"></param>
        public void SetProgress(float progress)
        {
            this._progress = progress;
        }

        /// <summary>
        /// Set position
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.Primitive.SetPosition(position);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        public void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect)
        {
            this.Primitive.Draw(graphicsDevice, basicEffect);
        }
    }
}